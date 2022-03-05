using BookLib.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BookLib
{
    public class Journal : AbstractItem
    {
        private int _editionNumber;
        public int EditionNumber
        {
            get { return _editionNumber; }
            set
            {
                try
                {
                    if (PropertyGuidelines.Check_Edition_Number(value))
                    {
                        _editionNumber = value;
                    }
                    else
                    {
                        throw new PropertyModificationException(PropertyGuidelines.Message_Edition_Number_Invalid, GetShortID());
                    }
                }
                catch (PropertyModificationException e)
                {
                   LibraryManager.ErrorLogger(e, e.Message);
                    throw e;
                }
            }
        }

        public Journal(string name, int editionNumber, string publisher, string summery, double price, string isbn,
                       DateTime printDate, CatagoryEnum catagory, int quantity, bool is_active, int discountPercent = 0) :
                  base(name, publisher, summery, price, isbn, printDate, catagory, quantity, is_active, discountPercent)
        {
            EditionNumber = editionNumber;
        }


        //copy constructor
        public Journal(Journal journal) : base(journal.Title, journal.Publisher, journal.Summery, journal.OriginalPrice, journal.ISBN, journal.PrintDate, journal.Catagory, journal.Quantity, journal.isActive, journal.DiscountPercent)
        {
            EditionNumber = journal.EditionNumber;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder($"{base.ToString()}");
            sb.Replace("***", "Journal");
            sb.Replace("Catagory", $"Edition: {EditionNumber},  Catagory");
            return sb.ToString();
        }
    }
}
