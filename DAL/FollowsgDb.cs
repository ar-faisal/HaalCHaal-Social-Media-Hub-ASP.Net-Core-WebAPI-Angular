using BOL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IFollowsgDb
    {
        IQueryable<Follower> GetFollowers(string currentUserId);

        public Boolean IsFollowing(string currentUser, string userIdToCheck);

        Task<bool> Follow(Follower follower);

        IQueryable<Follower> GetFollowerRelationships(string currentUser, string userIdToUnfollow);

        
        Task UnfollowAsync(IQueryable<Follower> followerRelationship);
    }

    public class FollowsgDb : IFollowsgDb
    {
        HCDbContext dbContext;
        public FollowsgDb(HCDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        

        public IQueryable<Follower> GetFollowers(string currentUserId)
        {

            var followers =  dbContext.Followers
                          .Where(f => f.FollowerUserId == currentUserId)
                          .Select(x => new Follower
                          {
                              FollowingUserId = x.FollowingUserId
                          });
                         

            return followers;
        }

        public bool IsFollowing(string currentUser, string userIdToCheck)
        {
            return dbContext.Followers
                .Any(f => f.FollowerUserId == currentUser && f.FollowingUserId == userIdToCheck);
        }

        public async Task<bool> Follow(Follower follower)
        {
            dbContext.Followers.Add(follower);
            var result = await dbContext.SaveChangesAsync();

            if (result != 0)
                return true;
            else
                return false;
        }


        public IQueryable<Follower> GetFollowerRelationships(string currentUser, string userIdToUnfollow)
        {
            return dbContext.Followers
                .Where(f => f.FollowerUserId == currentUser && f.FollowingUserId == userIdToUnfollow);
        }
        
    

        
        public async Task UnfollowAsync(IQueryable<Follower> followerRelationships)
        {
            if (followerRelationships != null)
            {
                foreach (var followerRelationship in followerRelationships)
                {
                    dbContext.Followers.Remove(followerRelationship);
                }

                await dbContext.SaveChangesAsync();
            }
        }

        
    }
}   
