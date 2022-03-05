using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLib.Exceptions
{
    public class DB_UnnecessaryDuplicationException : Exception
    {
        private static string error;
        public string GeneralError { get; private set; } //for error logger

        public DB_UnnecessaryDuplicationException() : base(message:error)
        {
            error = $"Requested item already exists in DataBase - it should be managed from there. Re-adding it to DB failed.";
            GeneralError = error;

        }
        public DB_UnnecessaryDuplicationException(string itemDetails,LibraryObjects type) : base(message:error)
        {
            if (type.Equals(LibraryObjects.Book) || type.Equals(LibraryObjects.Journal)|| type.Equals(LibraryObjects.General_Product))
            {
                error = $"Item with requested ISBN already exists in DataBase - it should be managed from there. Re-adding it to DB failed. {itemDetails}.";
                GeneralError = error;
            }
            else if (type.Equals(LibraryObjects.Order)) 
            {
                error = $"DataBase already contains order with ID {itemDetails}. Re-adding it to DB failed.";
                GeneralError = error;
            }

        }
    }
}
