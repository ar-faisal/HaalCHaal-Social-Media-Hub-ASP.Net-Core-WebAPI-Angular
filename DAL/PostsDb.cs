using BOL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IPostDb
    {
        IQueryable<StoryPost> GetStories();
        IQueryable<StoryPost> GetProfile(string id);
                
        Task<bool> Create(StoryPost story);
        IQueryable<StoryPost> getfollowingposts(List<string> followingUserIds);

    }
    public class PostDb : IPostDb
    {
        HCDbContext dbContext;
        public PostDb(HCDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public IQueryable<StoryPost> GetStories()
        {
            var storyPostsQuery = dbContext.StoryPosts
                .Include(x => x.HCUserNav)
                .Select(x => new StoryPost
                {
                    PId = x.PId,
                    PTitle = x.PTitle,
                    PDescription = x.PDescription,
                    CreatedOn = x.CreatedOn,
                    Like = x.Like,
                    HCUserNav = x.HCUserNav
                });

            return storyPostsQuery;
        }



        public IQueryable<StoryPost> GetProfile(string id)
        {
            var userStoryPosts = dbContext.StoryPosts
        .Include(x => x.HCUserNav)
        .Select(x => new StoryPost
        {
            PId = x.PId,
            PTitle = x.PTitle,
            PDescription = x.PDescription,
            CreatedOn = x.CreatedOn,
            Like = x.Like,
            HCUserNav = x.HCUserNav


        })
        .Where(x => x.HCUserNav.Id == id)
        .OrderByDescending(sp => sp.CreatedOn);
        
        

            return userStoryPosts;


        }
        public async Task<bool> Create(StoryPost story)
        {

            dbContext.StoryPosts.Add(story);
            var result = await dbContext.SaveChangesAsync();

           
            if (result != 0)
                return true;
            else
                return false;
        }

        public IQueryable<StoryPost> getfollowingposts(List<string> followingUserIds)
        {
            return dbContext.StoryPosts
                .Where(post => followingUserIds.Contains(post.HCUserId))
                .Select(post => new StoryPost
                {
                    PId = post.PId,
                    PTitle = post.PTitle,
                    PDescription = post.PDescription,
                    CreatedOn = post.CreatedOn,
                    Like = post.Like,
                    HCUserNav = post.HCUserNav
                })
                .OrderByDescending(post => post.CreatedOn);
        }

        
    }
}
