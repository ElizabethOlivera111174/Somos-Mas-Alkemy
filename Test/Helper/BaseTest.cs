using Microsoft.EntityFrameworkCore;
using OngProject.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Helper
{
    public class BaseTest
    {
        protected ApplicationDbContext MakeContext(string nombreDB)
        {
            var opciones = new DbContextOptionsBuilder<ApplicationDbContext>()
                            .UseInMemoryDatabase(nombreDB).Options;
            var dbcontext = new ApplicationDbContext(opciones);
            return dbcontext;
        }
    }
}
