using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BookLib.Exceptions
{
    public class PropertyModificationException : Exception
    {
        private static string error;
        public string GeneralError { get; private set; } //for error logger
        public PropertyModificationException(string message, string itemDetails) : base(error)
        {
            error = $"{message} Product Details: {itemDetails}.";

            GeneralError = message;
        }
    }
}
