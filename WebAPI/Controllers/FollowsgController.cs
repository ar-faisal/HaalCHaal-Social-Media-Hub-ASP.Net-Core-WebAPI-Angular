using BOL;
using DAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.ViewModels;

namespace WebAPI.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {

        private readonly IHCDb hcDb;

        public UserController(IHCDb _hcDb)
        {
            hcDb = _hcDb;
        }



        [HttpGet("GetPostsOfFollowedUsers")]
        public async Task<IActionResult> GetPostsOfFollowedUsers()
        {
            try
            {
                        // Get the currently logged-in user's ID
                        var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                        if (currentUserId == null)
                        {
                            return BadRequest("User ID not found.");
                        }

                        // Get the IDs of users the current user is following
                        var followingUserIds = await hcDb.FollowsgDb.GetFollowers(currentUserId).Select(f => f.FollowingUserId).ToListAsync();
                            

                        // Get all the posts from the users the current user is following
                        var posts = await hcDb.PostDb.getfollowingposts(followingUserIds).ToListAsync();
                       


                var storyWithImages = new List<StoryWithImage>();


                foreach (var story in posts)
                {
                    var imagePath = $"{story.PId}.jpeg";
                    var absoluteImagePath = Path.Combine(Directory.GetCurrentDirectory(), "Pics", story.HCUserNav.Id.ToString(), "Posts", imagePath);


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
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        [HttpPost("follow/{userIdToFollow}")]
        public async Task<IActionResult> Follow(string userIdToFollow)
        {
            try
            {
                var currentUser = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                if (currentUser == null)
                {
                    // Handle the case where the current user is not found.
                    return RedirectToAction("Login", "Account"); // Redirect to the login page or any other suitable action.
                }


                bool isFollowing = hcDb.FollowsgDb.IsFollowing(currentUser, userIdToFollow);
                // Check if the current user is already following the userToFollow.
                



                if (!isFollowing)
                {
                    // Create a new follower relationship.
                    var newFollower = new Follower
                    {
                        FollowerUserId = currentUser,
                        FollowingUserId = userIdToFollow
                    };

                    bool followed = await hcDb.FollowsgDb.Follow(newFollower);

                    

                    // Return a 201 Created status code indicating that the user was followed.
                    return StatusCode(201, "User followed successfully");
                }
                else
                {
                    // Throw a custom exception indicating that the user is not authorized to follow again.
                    throw new Exception("User is not authorized to follow again");
                }
            }
            catch (Exception E)
            {
                var msg = (E.InnerException != null) ? (E.InnerException.Message) : (E.Message);
                return StatusCode(500, "Admin is working on it! " + msg);
            }
        }





        [HttpPost("unfollow/{userIdToUnfollow}")]
        public async Task<IActionResult> Unfollow(string userIdToUnfollow)
        {
        var currentUser =  User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (currentUser == null)
        {
            return RedirectToAction("Login", "Account");
        }


           


            // Find and remove the follower relationship if it exists.
             var followerRelationship = hcDb.FollowsgDb.GetFollowerRelationships(currentUser, userIdToUnfollow);


            if (followerRelationship != null)
            {
                
                await hcDb.FollowsgDb.UnfollowAsync(followerRelationship);

            }

            // Redirect to a suitable action, e.g., the user's profile page.
            return Ok();
        }


        [HttpGet("isFollowing/{userIdToCheck}")]
        public async Task<IActionResult> IsFollowing(string userIdToCheck)
        {
            try
            {
                var currentUser =  User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                if (currentUser == null)
                {
                    // Handle the case where the current user is not found.
                    return RedirectToAction("Login", "Account"); // Redirect to the login page or any other suitable action.
                }




                // Check if the current user is following the userToCheck.
                bool isFollowing = hcDb.FollowsgDb.IsFollowing(currentUser, userIdToCheck);

                return Ok(new { IsFollowing = isFollowing });
            }
            catch (Exception ex)
            {
                // Handle the exception and log the details for debugging.
                Console.WriteLine($"Error in IsFollowing action: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
