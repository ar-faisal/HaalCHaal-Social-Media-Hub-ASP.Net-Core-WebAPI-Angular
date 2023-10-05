using BOL;
using DAL;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.Runtime.Intrinsics.X86;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;


builder.Services.AddCors();
builder.Services.AddControllers(
    config => config.Filters.Add(new AuthorizeFilter())
    );
builder.Services.AddDbContext<HCDbContext>(opt => opt.UseSqlServer(configuration.GetSection("HC:ssConStr").Value));
builder.Services.AddIdentity<HCUser, IdentityRole>()
                .AddEntityFrameworkStores<HCDbContext>()
                .AddDefaultTokenProviders();

builder.Services.AddTransient<IHCDb, HCDb>();

//Step-1: Create signingKey from Secretkey
var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("HC:JWTKey").Value));

//Step-2:Create Validation Parameters using signingKey
var tokenValidationParameters = new TokenValidationParameters()
{
    IssuerSigningKey = signingKey,
    ValidateIssuer = false,
    ValidateAudience = false,
    ClockSkew = TimeSpan.Zero
};

//Step-3: Set Authentication Type as JwtBearer
builder.Services.AddAuthentication(auth =>
{
    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
})
        //Step-4: Set Validation Parameter created above
        .AddJwtBearer(jwt =>
        {
            jwt.TokenValidationParameters = tokenValidationParameters;
        })
        .AddGoogle("Google", options =>
        {
            options.CallbackPath = new
        PathString(configuration.GetSection("HC:CallbackPath").Value);
            options.ClientId = configuration.GetSection("HC:ClientId").Value;
            options.ClientSecret =
            configuration.GetSection("HC:ClientSecret").Value;
        });



var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    #region Creating Roles
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roleNames = { "Admin", "User" };
    IdentityResult roleResult;
    foreach (var roleName in roleNames)
    {
        var roleExist = roleManager.RoleExistsAsync(roleName).Result;
        if (!roleExist)
        {
            roleResult = roleManager.CreateAsync(new IdentityRole(roleName)).Result;
        }
    }
    #endregion
    #region Creating Admin
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<HCUser>>();
    var userExist = userManager.FindByNameAsync("Superuser").Result;
    if (userExist == null)
    {
        var user = new HCUser() { UserName = "Superuser", Email = "admin@ss.com" };
        var userResult = userManager.CreateAsync(user, "Superuser@123").Result;
        var assignRoleResult = userManager.AddToRoleAsync(user, "Admin").Result;
    }
    #endregion
}



app.UseExceptionHandler(options =>
{
    options.Run(async context => //Here we are creating the object of HTTP context which has predefined Request and response,status code,etc.
    {
        context.Response.StatusCode = 500;//Internal Server Error //Here we are creating the response
        context.Response.ContentType = "application/json";
        var ex = context.Features.Get<IExceptionHandlerFeature>();
        if (ex != null)
        { //ex.Error
            var msg = (ex.Error.InnerException != null) ? ex.Error.InnerException.Message : ex.Error.Message;
            await context.Response.WriteAsync("Admin is working on it at application level " + msg); //writing the response directly
        }
    });
}
);


app.UseCors(x => x.WithOrigins(configuration.GetSection("HC:Origins").GetChildren().Select(x => x.Value).ToArray())
                              .AllowAnyMethod()
                              .AllowAnyHeader()
                              .AllowCredentials());
app.UseCors("AllowAngularApp");

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
