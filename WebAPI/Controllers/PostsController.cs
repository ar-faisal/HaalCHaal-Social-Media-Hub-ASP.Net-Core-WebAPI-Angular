using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BOL;
using DAL;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using WebAPI.ViewModels;
using Microsoft.AspNetCore.Hosting;

namespace WebAPI.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IHCDb hcDb;

        public PostsController(IHCDb _hcDb)
        {
            hcDb = _hcDb;
        }

        


        [HttpGet("stories")]
        public async Task<IActionResult> GetStoriesWithImagesAsync()
        {
            try
            {
                // Fetch data for all available stories (e.g., from a database)
                var stories = await hcDb.PostDb.GetStories().ToListAsync();
                    


                // Create a list to store the combined data for all stories
                var storyWithImages = new List<StoryWithImage>();

                foreach (var story in stories)
                {                   
                    var imagePath = $"{story.PId}.jpeg";
                    var absoluteImagePath = Path.Combine(Directory.GetCurrentDirectory(),"Pics", story.HCUserNav.Id.ToString(), "Posts", imagePath);

                    
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Pics", story.HCUserNav.Id.ToString(), "ProfilePic.jpeg");


                    if (!System.IO.File.Exists(filePath))
                    {
                        filePath = Path.Combine(Directory.GetCurrentDirectory(), "Pics", "DefaultProfilePicture.jpeg");

                    }

                    // Check if the image file exists
                    if (System.IO.File.Exists(absoluteImagePath))
                    {
                        var imageBytes = System.IO.File.ReadAllBytes(absoluteImagePath);                        
                        var imageBase64 = Convert.ToBase64String(imageBytes);

                        var imageBytesPP = System.IO.File.ReadAllBytes(filePath);
                        var imageBase64PP = Convert.ToBase64String(imageBytesPP);

                        // Create the combined response object
                        var response = new StoryWithImage
                        {
                            PId = story.PId,
                            PTitle = story.PTitle,
                            PDescription = story.PDescription,
                            CreatedOn = story.CreatedOn,
                            Like = story.Like,
                            HCUserId = story.HCUserNav.Id,
                            HCUserName = story.HCUserNav.UserName,
                            ImageBase64 = imageBase64,
                            ImageBase64PP = imageBase64PP
                        };

                        storyWithImages.Add(response); // Add each story to the list
                    }
                }

                // Return the list of combined data as JSON
                return Ok(storyWithImages);



            }
            catch (Exception ex)
            {
                // Handle any errors and return an appropriate response
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        [HttpGet("profile/{id}")]
        public async Task<IActionResult> GetProfileAsync(string id)
        {
            try
            {
                var storyPosts = await hcDb.PostDb.GetProfile(id).ToListAsync();

                string HCUserName = "";


                    // Create a list to store the combined data for all stories
                    var storyWithImages = new List<StoryWithImage>();

                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Pics", id, "ProfilePic.jpeg");


                    if (!System.IO.File.Exists(filePath))
                    {
                        filePath = Path.Combine(Directory.GetCurrentDirectory(), "Pics", "DefaultProfilePicture.jpeg");

                    }
                        var imageBytesPP = System.IO.File.ReadAllBytes(filePath);
                        var imageBase64PP = Convert.ToBase64String(imageBytesPP);


                foreach (var story in storyPosts)

                {
                    HCUserName = story.HCUserNav.UserName;
                    var imagePath = $"{story.PId}.jpeg";
                    var absoluteImagePath = Path.Combine(Directory.GetCurrentDirectory(), "Pics", id, "Posts", imagePath);

                   
                    // Check if the image file exists
                    if (System.IO.File.Exists(absoluteImagePath))
                    {
                        var imageBytes = System.IO.File.ReadAllBytes(absoluteImagePath);
                        var imageBase64 = Convert.ToBase64String(imageBytes);

                        

                        // Create the combined response object
                        var response = new StoryWithImage
                        {
                            PId = story.PId,
                            PTitle = story.PTitle,
                            PDescription = story.PDescription,
                            CreatedOn = story.CreatedOn,
                            Like = story.Like,                            
                            ImageBase64 = imageBase64
                        };

                        storyWithImages.Add(response); // Add each story to the list
                    }

                }
                var responseData = new
                {
                    StoryResponse = storyWithImages,
                    
                    ImageBase64PP = imageBase64PP,


                    hcUserName = HCUserName
                };

                // Return the combined data as JSON
                return Ok(responseData);



            }
            catch (Exception ex)
            {
                // Handle any errors and return an appropriate response
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        

        // POST: api/Stories
        [HttpPost("postss"), DisableRequestSizeLimit]
        public async Task<ActionResult<StoryPost>> Post()
        {
            try
            {
                StoryPost? model = JsonConvert.DeserializeObject<StoryPost>(Request.Form["myModel"].ToString());
                
                if (ModelState.IsValid && Request.Form.Files.Count == 1)
                {
                    model.HCUserId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
                    model.CreatedOn = DateTime.Now;

                    bool created = await hcDb.PostDb.Create(model);

                    if (created)
                    {

                        int recentlyGeneratedPId = model.PId;
                        string userId = model.HCUserId;
                        string profilePicsPath = "Pics/"+ userId + "/Posts"; // Relative to the application's base directory

                        // Combine the user-specific path
                        string userFolderPath = Path.Combine(profilePicsPath);

                        // Ensure the user folder exists
                        Directory.CreateDirectory(userFolderPath);

                        // Construct the full file path
                        string fileName = recentlyGeneratedPId  + ".jpeg";
                        string filePath = Path.Combine(userFolderPath, fileName);


                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            Request.Form.Files[0].CopyTo(stream);
                        }
                        

                         return Ok();
                    }
                    else
                    {
                        // Handle the case where creation in the database failed
                        return StatusCode(500, "Failed to create the StoryPost.");
                    }

                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception E)
            {
                var msg = (E.InnerException != null) ? (E.InnerException.Message) : (E.Message);
                return StatusCode(500, "Admin is working on it! " + msg);
            }
        }



        


        [HttpGet("getProfilePic"), DisableRequestSizeLimit]
        public async Task<IActionResult> GetProfilePic()
        {
            if (User.Identity.IsAuthenticated)
            {
                string? id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Pics", id, "ProfilePic.jpeg");
               
                
                if (!System.IO.File.Exists(filePath))
                {
                    filePath = Path.Combine(Directory.GetCurrentDirectory(), "Pics", "DefaultProfilePicture.jpeg");

                }
                var profilePic = System.IO.File.OpenRead(filePath);
                return File(profilePic, "image/jpeg");
                
            }
            else
            {
                return BadRequest("Invalid UserName Or Password");
            }
        }

        
    }
}
