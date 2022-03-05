using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLib.Exceptions
{
   public class LogicalFailException:Exception
    {
        public string GeneralError { get; private set; } //for error logger

        public LogicalFailException(string exceptionMessage) : base($"{exceptionMessage}.")
        {
            GeneralError = exceptionMessage;
        }
    }
}
