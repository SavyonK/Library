using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLib.Exceptions
{
    /// <summary>
    /// missing item from db
    /// </summary>
    public class DB_ItemUnreachableException : Exception
    {
        private static string error;
        public string GeneralError { get; private set; } //for error logger

        public DB_ItemUnreachableException(string itemDetails) : base(error)
        {

            error = $"Item {itemDetails} is not contained properly in database.";

            GeneralError = error;
        }
    }
}
