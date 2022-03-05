using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLib.Exceptions
{
    public class ItemUnavailableException : Exception
    {
        private static string error;
        public string GeneralError { get; private set; } //for error logger
        public ItemUnavailableException(string itemDetails) : base(error)
        {
            error = $"Item selected {itemDetails} was removed from stock and is currently unavailable.";
            GeneralError = error;
        }
    }
}
