using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class BlogRepository
    {
        public DataContext dataContext;

        public BlogRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }


        public IEnumerable<Post> GetAll()
        {
            return dataContext.Posts.ToList().OrderByDescending(x => x.Date).Take(10);
        }



    }
}
