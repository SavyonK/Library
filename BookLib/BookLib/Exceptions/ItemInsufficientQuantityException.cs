using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLib.Exceptions
{
    public class ItemInsufficientQuantityException : Exception
    {

        private static string error;
        public  string GeneralError { get; private set; } //for error logger

        public ItemInsufficientQuantityException(string itemDetails) : base(message: error)
        {
            error = $"There are not enough units of {itemDetails} in stock to make this quantity reduction.";
            GeneralError = error;

        }
    }
}
