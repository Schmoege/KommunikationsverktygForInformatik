using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    class MeetingRepository
    {
        public DataContext dataContext;

        public MeetingRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }
    }
}
