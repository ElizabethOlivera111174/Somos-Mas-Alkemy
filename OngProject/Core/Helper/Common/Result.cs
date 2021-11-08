using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Common
{
    public class Result
    {
        public bool HasErrors { get; set; }
        public IList<string> Messages { get; set; }
        public Result()
        {
            HasErrors = false;
            Messages = new List<string>();
        }
        public Result Success(string message)
            => new Result() { HasErrors = false, Messages = new List<string>() { message } };
        public Result Fail(string message)
            => new Result() { HasErrors = true, Messages = new List<string>() { message } };
        public Result NotFound()
            => new Result() { HasErrors = true, Messages = new List<string>() 
            { "No se ha podido encontrar un registro con los datos proporcionados" }};
    }
}
