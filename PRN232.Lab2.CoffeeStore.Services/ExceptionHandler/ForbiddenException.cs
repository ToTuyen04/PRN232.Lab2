using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232.Lab2.CoffeeStore.Services.ExceptionHandler
{
    public class ForbiddenException : BaseException
    {
        public ForbiddenException(string message) : base(message, "FORBIDDEN") { }
    }
}
