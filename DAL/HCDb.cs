using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IHCDb
    {
        IPostDb PostDb { get; }
        IFollowsgDb FollowsgDb { get; }
    }

    public class HCDb : IHCDb
    {
        HCDbContext context;
        public HCDb(HCDbContext _context)
        {
            context = _context;
        }

        IPostDb _postDb;
        IFollowsgDb _FollowsgDb;

        public IPostDb PostDb
        {
            get
            {
                if (_postDb == null)
                {
                    _postDb = new PostDb(context);
                }
                return _postDb;
            }
        }

        public IFollowsgDb FollowsgDb
        {
            get
            {
                if (_FollowsgDb == null)
                {
                    _FollowsgDb = new FollowsgDb(context);
                }
                return _FollowsgDb;
            }
        }

    }
}