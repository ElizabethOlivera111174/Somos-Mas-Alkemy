using Microsoft.EntityFrameworkCore;
using OngProject.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Helper
{
    public class TestsContext
    {
        public ApplicationDbContext GetTestContext(string testDb)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(testDb)
                .Options;
            
            var dbcontext = new ApplicationDbContext(options);

            return dbcontext;
        }
    }
}
