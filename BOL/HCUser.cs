using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace BOL
{
    [Table("HCUser")]
    public class HCUser : IdentityUser
    {

        

        public IEnumerable<StoryPost>? StoryPosts { get; set; }

        public IEnumerable<Follower> Followers { get; set; } 
        public IEnumerable<Follower> Following { get; set; } 

        


    }

}