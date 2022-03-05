using BookLib.Exceptions;
using JournalLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store.Preview.InstallControl;
using Windows.UI.Composition;
using Windows.UI.Xaml.Media.Animation;

namespace BookLib
{
    /// <summary>
    /// filter class will purposely not validate parameters with property guidelines in order to find abnormalities in the system
    /// </summary>
    public static class Filter
    {
        private static FilteredProducts GeneralProducts { get; set; }
        private static IEnumerable<AbstractItem> FilteredGeneralProductsResult { get; set; }//should contain the result of linq expression pulled on Items variable

        private static FilteredBooks Books { get; set; }
        private static IEnumerable<Book> FilteredBooksResult { get; set; }//should contain the result of linq expression pulled on Books variable

        private static FilteredJournals Journals { get; set; }
        private static IEnumerable<Journal> FilteredJournalsResult { get; set; }//will contain the result of linq expression pulled on Journals variable

        private static FilteredOrders Orders { get; set; }
        private static IEnumerable<Order> FilteredOrdersResult { get; set; }//should contain the result of linq expression pulled on Orders variable

        static Filter()
        {

        }





        /// <summary>
        /// "restart button" for variable of type FilterProducts 
        /// only method to access database - reloads FilteredAbstractItems with all products
        /// </summary>
        public static void ResetFilteredListByType(LibraryObjects type)
        {
            try
            {
                if (type == LibraryObjects.General_Product)
                {
                    GeneralProducts = new FilteredProducts(DataBase.LibraryItems.Values.ToList<AbstractItem>());
                    FilteredGeneralProductsResult = GeneralProducts;
                }
                else if (type == LibraryObjects.Book)
                {
                    List<AbstractItem> abstractTemp = DataBase.LibraryItems.Values.ToList();
                    List<Book> bookTemp = new List<Book>();

                    foreach (var item in abstractTemp)
                    {
                        if (item.GetType().Equals(typeof(Book)))
                        {
                            bookTemp.Add((Book)item);
                        }
                    }
                    Books = new FilteredBooks(bookTemp);
                    FilteredBooksResult = Books;
                }
                else if (type == LibraryObjects.Journal)
                {
                    List<AbstractItem> abstractTemp = DataBase.LibraryItems.Values.ToList();
                    List<Journal> journalTemp = new List<Journal>();

                    foreach (var item in abstractTemp)
                    {
                        if (item.GetType().Equals(typeof(Journal)))
                        {
                            journalTemp.Add((Journal)item);
                        }
                    }

                    Journals = new FilteredJournals(journalTemp);
                    FilteredJournalsResult = Journals;
                }
                else if (type == LibraryObjects.Order)
                {
                    Orders = new FilteredOrders(DataBase.Orders.Values.ToList<Order>());
                    FilteredOrdersResult = Orders;
                }
                else
                {
                    throw new LogicalFailException($"Reset Filtered List By Type method failed: it is not set to handle type - ({type.ToString()})");
                }
            }
            catch (LogicalFailException e)
            {
                LibraryManager.ErrorLogger(e, e.Message);
                throw e;
            }
        }

        #region internal methods

        /// <summary>
        /// loading Items with new sub-set for next query use
        /// </summary>
        private static void LoadWithNewSubSet(LibraryObjects type)
        {
            try
            {
                if (type == LibraryObjects.General_Product)
                {
                    GeneralProducts.ReplaceCurrentList(FilteredGeneralProductsResult.ToList());
                }
                else if (type == LibraryObjects.Book)
                {
                    Books.ReplaceCurrentList(FilteredBooksResult.ToList());
                }
                else if (type == LibraryObjects.Journal)
                {
                    Journals.ReplaceCurrentList(FilteredJournalsResult.ToList());
                }
                else if (type == LibraryObjects.Order)
                {
                    Orders.ReplaceCurrentList(FilteredOrdersResult.ToList());
                }
                else
                {
                    throw new Exception($"Reload subset method failed: it is currently not capable of working with type {type}");
                }
            }
            catch (Exception e)
            {
                LibraryManager.ErrorLogger(e, e.Message);
                throw e;
            }
        }

        #endregion

        #region UI "refresh"
        public static List<AbstractItem> GetUpdatedListForTypeAbstractItem()
        {
            return FilteredGeneralProductsResult.ToList();
        }
        public static List<Book> GetUpdatedListForTypeBook()
        {
            return FilteredBooksResult.ToList();
        }
        public static List<Journal> GetUpdatedListForTypeJournal()
        {
            return FilteredJournalsResult.ToList();
        }
        public static List<Order> GetUpdatedListForTypeOrder()
        {
            return FilteredOrdersResult.ToList();
        }

        #endregion




        #region General product
        public static void ByISBN(string ISBN, LibraryObjects type)
        {
            try
            {
                if (string.IsNullOrEmpty(ISBN))
                {
                    throw new ArgumentNullException("Filtering by ISBN failed: argument recieved for ISBN is null.");
                }

                if (type == LibraryObjects.General_Product || type == LibraryObjects.Book || type == LibraryObjects.Journal)
                {
                    if (type == LibraryObjects.General_Product)
                    {
                        FilteredGeneralProductsResult = GeneralProducts.Where(item => item.ISBN.Equals(ISBN));
                    }

                    else if (type == LibraryObjects.Book)
                    {
                        FilteredBooksResult = Books.Where(item => item.ISBN.Equals(ISBN));
                    }

                    else if (type == LibraryObjects.Journal)
                    {
                        FilteredJournalsResult = Journals.Where(item => item.ISBN.Equals(ISBN));
                    }

                    LoadWithNewSubSet(type);
                }
                else
                {
                    throw new LogicalFailException($"Filtering by ISBN failed - this method is not set to work with type {type.ToString()}");

                }
            }
            catch (LogicalFailException e)
            {
                LibraryManager.ErrorLogger(e, e.GeneralError);
                throw e;
            }
            catch (ArgumentNullException e)
            {
                LibraryManager.ErrorLogger(e, e.Message);
                throw e;
            }

        }
        public static void ByStatus(bool isActive, LibraryObjects type)
        {
            try
            {
                if (type == LibraryObjects.General_Product)
                {
                    FilteredGeneralProductsResult = GeneralProducts.Where(item => item.isActive == isActive);
                    LoadWithNewSubSet(type);

                }
                else if (type == LibraryObjects.Book)
                {
                    FilteredBooksResult = Books.Where(item => item.isActive == isActive);
                    LoadWithNewSubSet(type);

                }
                else if (type == LibraryObjects.Journal)
                {
                    FilteredJournalsResult = Journals.Where(item => item.isActive == isActive);
                    LoadWithNewSubSet(type);

                }
                else
                {
                    throw new LogicalFailException($"Filtering by status failed - this method is not set to work with type {type.ToString()}");
                }
            }
            catch (LogicalFailException e)
            {
                LibraryManager.ErrorLogger(e, e.GeneralError);
                throw e;
            }
        }
        public static void ByCatagory(CatagoryEnum catagory, LibraryObjects type)
        {
            try
            {
                if (type == LibraryObjects.General_Product || type == LibraryObjects.Book || type == LibraryObjects.Journal)
                {

                    if (type == LibraryObjects.General_Product)
                    {
                        FilteredGeneralProductsResult = GeneralProducts.Where(item => item.Catagory == catagory);
                    }
                    else if (type == LibraryObjects.Book)
                    {
                        FilteredBooksResult = Books.Where(item => item.Catagory == catagory);
                    }
                    else if (type == LibraryObjects.Journal)
                    {
                        FilteredJournalsResult = Journals.Where(item => item.Catagory == catagory);
                    }
                    LoadWithNewSubSet(type);
                }
                else
                {
                    throw new LogicalFailException($"Filtering by catagory failed - this method is not set to work with type {type.ToString()}");
                }
            }
            catch (LogicalFailException e)
            {
                LibraryManager.ErrorLogger(e, e.GeneralError);
                throw e;
            }

        }
        public static void ByPublisher(LibraryObjects type, string publisher)
        {
            try
            {
                if (type == LibraryObjects.General_Product || type == LibraryObjects.Book || type == LibraryObjects.Journal)
                {
                    if (string.IsNullOrEmpty(publisher))
                    { throw new ArgumentNullException("Argument for publisher can't be null"); }

                    else if (type == LibraryObjects.General_Product)
                    {
                        FilteredGeneralProductsResult = GeneralProducts.Where(item => item.Publisher.Contains(publisher));
                    }
                    else if (type == LibraryObjects.Book)
                    {
                        FilteredBooksResult = Books.Where(item => item.Publisher.Contains(publisher));
                    }
                    else if (type == LibraryObjects.Journal)
                    {
                        FilteredJournalsResult = Journals.Where(item => item.Publisher.Contains(publisher));
                    }

                    LoadWithNewSubSet(type);
                }

                else
                {
                    throw new LogicalFailException($"Filtering by publisher failed - this method is not set to work with type {type.ToString()}");
                }
            }
            catch (LogicalFailException e)
            {
                LibraryManager.ErrorLogger(e, e.GeneralError);
                throw e;
            }
            catch (ArgumentNullException e)
            {
                LibraryManager.ErrorLogger(e, e.Message);
                throw e;
            }
        }
        public static void ByTitle(LibraryObjects type, string title)
        {
            try
            {
                if (type == LibraryObjects.General_Product || type == LibraryObjects.Book || type == LibraryObjects.Journal)
                {
                    if (string.IsNullOrEmpty(title))
                    { throw new ArgumentNullException("Value passed for title can't be null"); }

                    if (type == LibraryObjects.General_Product)
                    {
                        FilteredGeneralProductsResult = GeneralProducts.Where(item => item.Title.Contains(title));
                    }

                    else if (type == LibraryObjects.Book)
                    {
                        FilteredBooksResult = Books.Where(item => item.Title.Contains(title));
                    }

                    else if (type == LibraryObjects.Journal)
                    {
                        FilteredJournalsResult = Journals.Where(item => item.Title.Contains(title));
                    }
                    LoadWithNewSubSet(type);
                }

                else
                {
                    throw new LogicalFailException($"Filtering by title failed - this method is not set to work with type {type.ToString()}");
                }
            }
            catch (LogicalFailException e)
            {
                LibraryManager.ErrorLogger(e, e.GeneralError);
                throw e;
            }
            catch (ArgumentNullException e)
            {
                LibraryManager.ErrorLogger(e, e.Message);
                throw e;
            }
        }
        public static void ByPrice(LibraryObjects type, double minPrice = double.MinValue, double maxPrice = double.MaxValue)
        {
            try
            {
                if (type == LibraryObjects.General_Product || type == LibraryObjects.Book || type == LibraryObjects.Journal)
                {
                    bool checkMinPrice = minPrice != double.MinValue;
                    bool checkMaxPrice = maxPrice != double.MaxValue;

                    if (!checkMinPrice && !checkMaxPrice)
                    {
                        throw new ArgumentNullException("Filtering failed: at least one price parameter must be addressed for this method to work");
                    }
                    else if (checkMaxPrice && maxPrice < minPrice)
                    {
                        throw new LogicalFailException($"Filtering failed: value for max price ({maxPrice}) cannot be smaller than value for min price ({minPrice})");
                    }

                    if (type == LibraryObjects.General_Product)
                    {
                        FilteredGeneralProductsResult = GeneralProducts.Where(item => (item.CurrentPrice >= minPrice && item.CurrentPrice <= maxPrice));
                    }
                    else if (type == LibraryObjects.Book)
                    {
                        FilteredBooksResult = Books.Where(item => (item.CurrentPrice >= minPrice && item.CurrentPrice <= maxPrice));
                    }
                    else if (type == LibraryObjects.Journal)
                    {
                        FilteredJournalsResult = Journals.Where(item => (item.CurrentPrice >= minPrice && item.CurrentPrice <= maxPrice));
                    }

                    LoadWithNewSubSet(type);
                }
                else
                {
                    throw new LogicalFailException($"Filtering by price failed - this method is not set to work with type {type.ToString()}");
                }
            }
            catch (LogicalFailException e)
            {
                LibraryManager.ErrorLogger(e, e.GeneralError);
                throw e;
            }
            catch (ArgumentNullException e)
            {
                LibraryManager.ErrorLogger(e, e.Message);
                throw e;
            }

        }
        public static void ByQuantity(LibraryObjects type, int minQuantity = int.MinValue, int maxQuantity = int.MaxValue)
        {
            try
            {
                if (type == LibraryObjects.General_Product || type == LibraryObjects.Book || type == LibraryObjects.Journal)
                {
                    bool checkMinQuantity = minQuantity != int.MinValue;
                    bool checkMaxQuantity = maxQuantity != int.MaxValue;

                    if (!checkMinQuantity && !checkMaxQuantity)
                    {
                        throw new ArgumentNullException("Filtering failed: at least one price parameter must be addressed for this method to work");
                    }
                    else if (checkMaxQuantity && maxQuantity < minQuantity)
                    {
                        throw new LogicalFailException($"Filtering failed: value for max quantity ({maxQuantity}) cannot be smaller than value for min quantity ({minQuantity})");
                    }

                    if (type == LibraryObjects.General_Product)
                    {
                        FilteredGeneralProductsResult = GeneralProducts.Where(item => (item.Quantity >= minQuantity && item.Quantity <= maxQuantity));
                    }
                    else if (type == LibraryObjects.Book)
                    {
                        FilteredBooksResult = Books.Where(item => (item.Quantity >= minQuantity && item.Quantity <= maxQuantity));
                    }
                    else if (type == LibraryObjects.Journal)
                    {
                        FilteredJournalsResult = Journals.Where(item => (item.Quantity >= minQuantity && item.Quantity <= maxQuantity));
                    }

                    LoadWithNewSubSet(type);
                }
                else
                {
                    throw new LogicalFailException($"Filtering by quantity failed - this method is not set to work with type {type.ToString()}");
                }
            }
            catch (LogicalFailException e)
            {
                LibraryManager.ErrorLogger(e, e.GeneralError);
                throw e;
            }
            catch (ArgumentNullException e)
            {
                LibraryManager.ErrorLogger(e, e.Message);
                throw e;
            }

        }
        public static void ByDiscount(LibraryObjects type, int minDiscount = int.MinValue, int maxDiscount = int.MaxValue)
        {
            //not checking discount validity because: in a situation where price went up (above original price) it would count as invalid - but it possible.
            try
            {
                if (type == LibraryObjects.General_Product || type == LibraryObjects.Book || type == LibraryObjects.Journal)
                {
                    bool checkMinDiscount = minDiscount != int.MinValue;
                    bool checkMaxDiscount = maxDiscount != int.MaxValue;


                    if (!checkMinDiscount && !checkMaxDiscount)
                    {
                        throw new ArgumentNullException("At least one of method parameters must have valid discount value (0-99)!");
                    }
                    else if (checkMinDiscount && checkMaxDiscount && minDiscount > maxDiscount)
                    {
                        throw new LogicalFailException($"Filtering failed: value for max discount ({maxDiscount}) cannot be smaller than value for min discount ({minDiscount})");
                    }


                    if (type == LibraryObjects.General_Product)
                    {
                        FilteredGeneralProductsResult = GeneralProducts.Where(item => (item.DiscountPercent >= minDiscount && item.DiscountPercent <= maxDiscount));
                    }

                    else if (type == LibraryObjects.Book)
                    {

                        FilteredBooksResult = Books.Where(item => (item.DiscountPercent >= minDiscount && item.DiscountPercent <= maxDiscount));

                    }

                    else if (type == LibraryObjects.Journal)
                    {
                        FilteredJournalsResult = Journals.Where(item => (item.DiscountPercent >= minDiscount && item.DiscountPercent <= maxDiscount));
                    }
                    LoadWithNewSubSet(type);
                }
                else
                {
                    throw new LogicalFailException($"Filtering by Discount failed - this method is not set to work with type {type.ToString()}");
                }
            }
            catch (LogicalFailException e)
            {
                LibraryManager.ErrorLogger(e, e.GeneralError);
                throw e;
            }
            catch (ArgumentNullException e)
            {
                LibraryManager.ErrorLogger(e, e.Message);
                throw e;
            }
        }

        #endregion


        #region Book 
        public static void ByAuthor(LibraryObjects type, string author)
        {
            try
            {
                if (type == LibraryObjects.Book)
                {
                    if (string.IsNullOrEmpty(author))
                    { throw new ArgumentNullException("Value passed for title can't be null"); }

                    FilteredBooksResult = Books.Where(item => item.Author.Contains(author));
                    LoadWithNewSubSet(type);
                }

                else
                {
                    throw new LogicalFailException($"Filtering by author failed - this method is not set to work with type {type.ToString()}");
                }
            }
            catch (LogicalFailException e)
            {
                LibraryManager.ErrorLogger(e, e.GeneralError);
                throw e;
            }
            catch (ArgumentNullException e)
            {
                LibraryManager.ErrorLogger(e, e.Message);
                throw e;
            }
        }

        #endregion


        #region Order 

        public static void ByOrderID(int orderID)
        {
            FilteredOrdersResult = Orders.Where(item => item.OrderID == orderID);
            LoadWithNewSubSet(LibraryObjects.Order);
        }


        public static void ByTransactionPrice(double minPrice = double.MinValue, double maxPrice = double.MaxValue)
        {
            try
            {
                bool checkMinPrice = minPrice != double.MinValue;
                bool checkMaxPrice = maxPrice != double.MaxValue;


                if (!checkMinPrice && !checkMaxPrice)
                {
                    throw new ArgumentNullException("Filtering failed: at least one price parameter must be addressed for this method to work");
                }
                else if (checkMaxPrice && maxPrice < minPrice)
                {
                    throw new LogicalFailException($"Filtering failed: value for max price ({maxPrice}) cannot be smaller than value for min price ({minPrice})");
                }

                else
                {
                    FilteredOrdersResult = Orders.Where(item => (item.TransactionPrice >= minPrice && item.TransactionPrice <= maxPrice));
                }

                LoadWithNewSubSet(LibraryObjects.Order);
            }
            catch (LogicalFailException e)
            {
                LibraryManager.ErrorLogger(e, e.GeneralError);
                throw e;
            }
            catch (ArgumentNullException e)
            {
                LibraryManager.ErrorLogger(e, e.Message);
                throw e;
            }

        }
        public static void ByOrderDate(DateTime minDate, DateTime maxDate)
        {
            try
            {
                //check if date given is not datetime default value
                bool checkMinDate = minDate != DateTime.MinValue;
                bool checkMaxDate = maxDate != DateTime.MinValue;

                //if dates are equal or minDate is earlier than maxDate
                bool areDatesOK = checkMaxDate && DateTime.Compare(minDate, maxDate) <= 0;

                if (!checkMinDate && !checkMaxDate)
                {
                    throw new LogicalFailException("Filtering failed: at least one of the DateTime parameters must contain a value");
                }
                else if (checkMaxDate && !areDatesOK) //if maxDate was picked by user & minDate is later than maxDate
                {
                    throw new LogicalFailException("Filtering failed: value in minDate cannot be later than maxDate");
                }
                else if (checkMinDate && !checkMaxDate)
                {
                    maxDate = DateTime.MaxValue;
                }
            
              if (DateTime.Compare(minDate, maxDate) <= 0) //if dates are equal or minDate is earlier than maxDate
                {
                    FilteredOrdersResult = Orders.Where(item => item.DateOrdered >= minDate && item.DateOrdered <= maxDate);
                }

                LoadWithNewSubSet(LibraryObjects.Order);
            }
            catch (LogicalFailException e)
            {
                LibraryManager.ErrorLogger(e, e.GeneralError);
                throw e;
            }
        }
        #endregion


        #region Journal 
        /// <summary>
        /// when looking for exact edition number - pass same value in BOTH parameters
        /// </summary>
        public static void FilterEditionNumber(LibraryObjects type, int minEdition = int.MinValue, int maxEdition = int.MaxValue)
        {
            try
            {
                if (type == LibraryObjects.Journal)
                {

                    bool checkMinEdition = minEdition != int.MinValue;
                    bool checkMaxEdition = maxEdition != int.MaxValue;

                    if (!checkMinEdition && !checkMaxEdition)
                    {
                        throw new LogicalFailException("Filtering failed: at least one price parameter must be addressed for this method to work");
                    }
                    else if (minEdition > maxEdition)
                    {
                        throw new LogicalFailException($"Filtering failed: value for max quantity ({maxEdition}) cannot be smaller than value for min quantity ({minEdition})");
                    }

                    FilteredJournalsResult = Journals.Where(item => item.EditionNumber >= minEdition && item.EditionNumber <= maxEdition);

                    LoadWithNewSubSet(type);

                }
                else
                {
                    throw new LogicalFailException($"Filtering by edition number failed - this method is not set to work with type {type.ToString()}");

                }
            }
            catch (LogicalFailException e)
            {
                LibraryManager.ErrorLogger(e, e.GeneralError);
                throw e;
            }

        }
        #endregion





    }
}
