using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BookLib.Exceptions;
using System.Text;
using System.Threading.Tasks;

namespace BookLib
{
    public class Book : AbstractItem
    {
        private string _author;
        public string Author
        {
            get { return _author; }
            set
            {
                try
                {
                    if (PropertyGuidelines.Check_Author_Length(value))
                    { _author = value; }

                    else
                    { throw new PropertyModificationException(PropertyGuidelines.Message_Author_Length_Invalid, GetShortID()); }
                }
                catch (PropertyModificationException e)
                {
                    LibraryManager.ErrorLogger(e, e.Message);
                    throw e;
                }
            }
        }


        public Book(string name, string author, string publisher, string summery, double price, string isbn, DateTime printDate,
                        CatagoryEnum catagory, int quantity, bool is_active, int discountPercent = 0) :
                    base(name, publisher, summery, price, isbn, printDate, catagory, quantity, is_active, discountPercent)
        {
            Author = author;
        }

        //copy constructor
        public Book(Book book) : base(book.Title, book.Publisher, book.Summery, book.OriginalPrice, book.ISBN, book.PrintDate, book.Catagory, book.Quantity, book.isActive, book.DiscountPercent)
        {

        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder($"{base.ToString()}");
            sb.Replace("***", "Book");
            sb.Replace("Catagory", $"Author: {Author},  Catagory");
            return sb.ToString();
        }
    }
}
