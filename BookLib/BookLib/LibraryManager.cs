using BookLib.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.VoiceCommands;
using Windows.Media.Streaming.Adaptive;
using Windows.UI.Input.Preview.Injection;
using Windows.UI.Text;

namespace BookLib
{
    public static class LibraryManager
    {
        public static int test;
        private static string _logFileName;

        static LibraryManager()
        {

            _logFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "myLog.txt");
            InitializeLog();


            //fake loop to make Database.DB Get accessor to invoke Database constructor
            while (DataBase.DB != null)
            { break; }
        }


        #region modifying UI lists

        /// <summary>
        /// Coppies all items in current inventory from db lists to reffered observablecollection from ui
        /// </summary>
        public static void RefreshCurrentInventory(ref ObservableCollection<AbstractItem> store_Items)
        {
            store_Items.Clear();
            foreach (var item in DataBase.LibraryItems)
            {
                if (item.Value.isActive)
                {
                    store_Items.Add(item.Value);
                }
            }
        }

        /// <summary>
        /// Coppies all retired items from db lists to reffered observablecollection from ui
        /// </summary>
        public static void RefreshRetiredInventory(ref ObservableCollection<AbstractItem> retired_Items)
        {
            retired_Items.Clear();
            foreach (var item in DataBase.LibraryItems)
            {
                if (!item.Value.isActive)
                {
                    retired_Items.Add(item.Value);
                }
            }
        }

        #endregion

        #region for testing purposes
        public static void InsertExampleStockAndOrders()
        {
            Book a = new Book(name: "1984", author: "George Orwell", publisher: "Secker & Warburg", summery: "Summery1",
                                  price: 17.75, isbn: "0141036141", printDate: new DateTime(1949, 6, 8),
                                  catagory: CatagoryEnum.Fiction, quantity: 79, true, discountPercent: 0);

            Book b = new Book(name: "The Handmaid's Tale", author: "Margaret Atwood", publisher: "McClelland and Stewart", summery: "Summery2",
                              price: 17.25, isbn: "0099511665", printDate: new DateTime(1985, 1, 1),
                              catagory: CatagoryEnum.Fiction, quantity: 43, true, discountPercent: 20);

            Book c = new Book(name: "Harry Potter and the Chamber of Secrets", author: "J.K. Rowling", publisher: "Bloomsbury Publishing PLC", summery: "Summery3",
                             price: 10.40, isbn: "0747538492", printDate: new DateTime(1998, 7, 2),
                             catagory: CatagoryEnum.Fantasy, quantity: 20, true, discountPercent: 5);

            Book d = new Book(name: "Animal Farm", author: "George Orwell", publisher: "Secker & Warburg", summery: "Summery4",
                             price: 17.75, isbn: "0141036133", printDate: new DateTime(1945, 8, 17),
                             catagory: CatagoryEnum.Political_Satire, quantity: 10, true, discountPercent: 0);

            Book e = new Book(name: "Mind Hunter", author: "John Douglas", publisher: "Cornerstone", summery: "Summery5",
                                  price: 15, isbn: "1787460614", printDate: new DateTime(2017, 11, 2),
                                  catagory: CatagoryEnum.True_Crime, quantity: 98, true, discountPercent: 0);

            Book f = new Book(name: "Bullshit Jobs", author: "David Graeber", publisher: "Penguin Books Ltd", summery: "Summery6",
                              price: 8.99, isbn: "0141983477", printDate: new DateTime(2019, 2, 7),
                              catagory: CatagoryEnum.Financial, quantity: 55, false, discountPercent: 0);

            Book g = new Book(name: "Lonely Planet New Zealand", author: "Lonely Planet", publisher: "Lonely Planet Global Limited", summery: "Summery7",
                             price: 27.99, isbn: "1786570793", printDate: new DateTime(2018, 9, 18),
                             catagory: CatagoryEnum.Travel, quantity: 45, false, discountPercent: 0);

            Book h = new Book(name: "Fox in Socks", author: "Dr. Seuss", publisher: "Random House USA Inc", summery: "Summery8",
                             price: 7.72, isbn: "0307931803", printDate: new DateTime(2011, 12, 27),
                             catagory: CatagoryEnum.Fairytale, quantity: 14, true, discountPercent: 50);

            Journal a_j = new Journal(name: "Forks Over Knives", editionNumber: 1, publisher: "Forks Over Knives", summery: "Fall 2020 Single Issue Magazine",
                             price: 9.99, isbn: "1547853611", printDate: new DateTime(2020, 9, 11),
                             CatagoryEnum.Nutrition, quantity: 26, true, discountPercent: 0);

            Journal b_j = new Journal(name: "National Geographic Readers: Titanic", editionNumber: 1, publisher: "Conde Nast Publications", summery: "Single Issue Magazine – Illustrated",
                             price: 4.99, isbn: "1426310595", printDate: new DateTime(2012, 3, 27),
                             CatagoryEnum.History, quantity: 14, false, discountPercent: 0);

            Journal c_j = new Journal(name: "LIFE", editionNumber: 4, publisher: "The Editors of LIFE", summery: "Jaws Special Edition",
                             price: 13.99, isbn: "1547851732", printDate: new DateTime(2020, 6, 19),
                             CatagoryEnum.Entertainment, quantity: 8, true, discountPercent: 0);



            AddToLibrary(a, b, c, d, e, f, g, h, a_j, b_j, c_j);

            List<AbstractItem> list = new List<AbstractItem>();
            list.Add(new Journal(c_j));
            list[0].Quantity = 2;
            list.Add(new Book(h));
            list[1].Quantity = 3;
            Order order_a = new Order(list, 222);

            List<AbstractItem> list_b = new List<AbstractItem>();
            list_b.Add(new Journal(a_j));
            list_b[0].Quantity = 11;
            Order order_b = new Order(list_b, 334);


            List<AbstractItem> list_c = new List<AbstractItem>();
            list_c.Add(new Book(c));
            list_c[0].Quantity = 3;
            list_c.Add(new Book(d));
            list_c[1].Quantity = 1;
            Order order_c = new Order(list_c, 175.5);
            AddOrderToDB(order_a);
            AddOrderToDB(order_b);
            AddOrderToDB(order_c);

        }


        #endregion


        /// <summary>
        /// adds items of type Book & Journal to general list
        /// </summary>
        /// <param name="items">Book or Journal typed objects only</param>
        public static void AddToLibrary(params AbstractItem[] items)
        {
            try
            {
                for (int i = 0; i < items.Length; i++)
                {
                    if (PropertyGuidelines.Check_ISBN_Availability(items[i].ISBN))
                    {
                        DataBase.LibraryItems.Add(items[i].ISBN, items[i]);
                    }
                    else if (DataBase.LibraryItems.ContainsKey(items[i].ISBN))
                    {
                        throw new DB_UnnecessaryDuplicationException(items[i].GetShortID(), LibraryObjects.General_Product);
                    }

                }
            }
            catch (DB_UnnecessaryDuplicationException e)
            {
                ErrorLogger(e, e.GeneralError);
                throw e;
            }
            catch (ArgumentNullException e)
            {
                ErrorLogger(e, "Item with requested ISBN already exists in DataBase - it should be managed from there. Re-creating & Re-adding it to DB failed.");
                throw new DB_UnnecessaryDuplicationException();
            }
        }

        public static int CheckQuantityForItem(string isbn)
        {
            try
            {
                if (!PropertyGuidelines.Check_ISBN_Validity(isbn))
                {
                    throw new PropertyModificationException(PropertyGuidelines.Message_ISBN_Invalid, $"[ISBN given: {isbn}]");

                }
                else if (!DataBase.LibraryItems.ContainsKey(isbn))
                {
                    throw new DB_ItemUnreachableException($"[ISBN: {isbn}]");
                }

                return DataBase.LibraryItems[isbn].Quantity;


            }
            catch (PropertyModificationException e)
            {
                ErrorLogger(e, e.GeneralError);
                throw e;
            }
            catch (DB_ItemUnreachableException e)
            {
                ErrorLogger(e, e.GeneralError);
                throw e;
            }

        }

        #region status changing methods for items in DB
        /// <summary>
        /// De-activate method: changes item state to unactive, or some would say "retired" - is not meant to be shown in storefront
        /// </summary>
        /// <param name="items">Book or Journal typed objects only</param>
        public static void RemoveLibraryItems(params AbstractItem[] items)
        {
            try
            {
                for (int i = 0; i < items.Length; i++)
                {
                    if (DataBase.LibraryItems.ContainsKey(items[i].ISBN))  //check if item is in database
                    {
                        DataBase.LibraryItems[items[i].ISBN].isActive = false;

                    }
                    else if (!DataBase.LibraryItems.ContainsKey(items[i].ISBN))
                    {
                        throw new DB_ItemUnreachableException(items[i].GetShortID());
                    }
                }
            }
            catch (DB_ItemUnreachableException e)
            { ErrorLogger(e, e.GeneralError); }
        }

        /// <summary>
        /// Re-activate method: changes removed item state back to active, is meant to be seen in storefront
        /// active state of item does not deny possibility 0 units in stock which means - temporarily out of stock
        /// </summary>
        /// <param name="items">Book or Journal typed objects only</param>
        public static void RestoreLibraryItems(params AbstractItem[] items)
        {
            try
            {
                for (int i = 0; i < items.Length; i++)
                {
                    if (DataBase.LibraryItems.ContainsKey(items[i].ISBN))
                    {
                        DataBase.LibraryItems[items[i].ISBN].isActive = true;
                    }

                    else if (!DataBase.LibraryItems.ContainsKey(items[i].ISBN))
                    {
                        throw new DB_ItemUnreachableException(items[i].GetShortID());
                    }

                }
            }
            catch (DB_ItemUnreachableException e)
            { ErrorLogger(e, e.GeneralError); }

        }
        #endregion


        public static void AddOrderToDB(Order order)
        {
            try
            {
                if (!DataBase.Orders.ContainsKey(order.OrderID))
                {
                    DataBase.Orders.Add(order.OrderID, order);
                }
                else
                {
                    throw new DB_UnnecessaryDuplicationException(order.OrderID.ToString(), LibraryObjects.Order);
                }
            }
            catch (DB_UnnecessaryDuplicationException e)
            {
                ErrorLogger(e, e.GeneralError);
                throw e;
            }
        }

        /// <summary>
        /// shows items full details - each property in new line
        /// </summary>
        /// <param name="isbn"></param>
        /// <returns></returns>
        public static string GetLongID(string isbn)
        {
            try
            {
                if (PropertyGuidelines.Check_ISBN_Validity(isbn))
                {
                    if (DataBase.LibraryItems.ContainsKey(isbn))
                    {
                        StringBuilder sb = new StringBuilder(DataBase.LibraryItems[isbn].ToString());
                        sb.Append($"\nSummery: {DataBase.LibraryItems[isbn].Summery}");
                        sb.Replace(",  ", "\n");
                        return sb.ToString();
                    }
                    else
                    {
                        throw new DB_ItemUnreachableException(isbn);
                    }
                }
                else
                {
                    throw new PropertyModificationException(PropertyGuidelines.Message_ISBN_Invalid, isbn);
                }
            }
            catch (PropertyModificationException e)
            {
                ErrorLogger(e, e.GeneralError);
                throw e;
            }
            catch (DB_ItemUnreachableException e)
            {
                ErrorLogger(e, e.GeneralError);
                throw e;
            }
        }

        #region modification methods for items in DB
        public static AbstractItem GetItemByISBN(string isbn)
        {
            try
            {

                if (PropertyGuidelines.Check_ISBN_Validity(isbn))
                {
                    if (DataBase.LibraryItems.ContainsKey(isbn))
                    {
                        return DataBase.LibraryItems[isbn];
                    }
                    else
                    {
                        throw new DB_ItemUnreachableException(isbn);
                    }
                }
                else
                {
                    throw new PropertyModificationException(PropertyGuidelines.Message_ISBN_Invalid, isbn);
                }
            }
            catch (PropertyModificationException e)
            {
                ErrorLogger(e, e.GeneralError);
                throw e;
            }
            catch (DB_ItemUnreachableException e)
            {
                ErrorLogger(e, e.GeneralError);
                throw e;
            }
        }
        public static void ChangeObjTypeForISBN(AbstractItem objToReplace, AbstractItem newObj)
        {
            try
            {
                if (!PropertyGuidelines.Check_ISBN_Availability(objToReplace.ISBN) && objToReplace.ISBN == newObj.ISBN)
                {
                    DataBase.LibraryItems[objToReplace.ISBN] = newObj;
                }
                else
                {
                    throw new DB_ItemUnreachableException(objToReplace.GetShortID());
                }
            }

            catch (DB_ItemUnreachableException e)
            {
                ErrorLogger(e, e.GeneralError);
                throw e;
            }
        }


        /// <summary>
        /// alters item' quantity in DB, substracting/adding quantity is depend on value passed for 'isRestocking' parameter
        /// </summary>
        /// <param name="isRestocking">true for restocking, false for substracting quantity</param>
        /// <param name="item">item to change</param>
        /// <param name="units">value shouldnt be negative</param>
        public static void ChangeItemQuantity(bool isRestocking, AbstractItem item, int units)
        {
            try
            {
                if (DataBase.LibraryItems.ContainsKey(item.ISBN))  //check if item is in database
                {
                    if (isRestocking)
                    {

                        if (PropertyGuidelines.Check_Quantity_Validity(DataBase.LibraryItems[item.ISBN].Quantity + Math.Abs(units)))
                        {
                            DataBase.LibraryItems[item.ISBN].Quantity += Math.Abs(units);
                        }
                        else
                        {
                            throw new PropertyModificationException($"{PropertyGuidelines.Message_Quantity_Maxed_Out}", item.GetShortID()); ;
                        }

                    }
                    else if (!isRestocking)
                    {

                        if (PropertyGuidelines.Check_Quantity_Validity(DataBase.LibraryItems[item.ISBN].Quantity - Math.Abs(units)))
                        {
                            DataBase.LibraryItems[item.ISBN].Quantity -= Math.Abs(units);
                        }
                        else
                        {
                            throw new ItemInsufficientQuantityException(item.GetShortID());
                        }

                    }
                }
                else
                {
                    throw new DB_ItemUnreachableException(item.GetShortID());
                }
            }
            catch (PropertyModificationException e)
            {
                ErrorLogger(e, e.GeneralError);
                throw e;
            }
            catch (ItemInsufficientQuantityException e)
            {
                ErrorLogger(e, e.GeneralError);
                throw e;
            }
            catch (DB_ItemUnreachableException e)
            {
                ErrorLogger(e, e.GeneralError);
                throw e;
            }

        }
        public static void ChangeItemTitle(AbstractItem item, string newTitle)
        {
            try
            {
                if (DataBase.LibraryItems.ContainsKey(item.ISBN))
                {
                    if (PropertyGuidelines.Check_Title_Validity(newTitle))
                    {
                        DataBase.LibraryItems[item.ISBN].Title = newTitle;
                    }
                    else
                    {
                        throw new PropertyModificationException(PropertyGuidelines.Message_Title_Length_Invalid, item.GetShortID());
                    }
                }
                else
                {
                    throw new DB_ItemUnreachableException(item.GetShortID());
                }
            }
            catch (PropertyModificationException e)
            {
                ErrorLogger(e, e.GeneralError);
                throw e;
            }
            catch (DB_ItemUnreachableException e)
            {
                ErrorLogger(e, e.GeneralError);
                throw e;
            }
        }
        public static void ChangeItemSummery(AbstractItem item, string newSummery)
        {
            try
            {
                if (DataBase.LibraryItems.ContainsKey(item.ISBN))
                {
                    if (PropertyGuidelines.Check_Summery_Length(newSummery))
                    {
                        DataBase.LibraryItems[item.ISBN].Summery = newSummery;
                    }
                    else
                    {
                        throw new PropertyModificationException($"{PropertyGuidelines.Message_Summery_Length_Invalid}", item.GetShortID());
                    }
                }
                else
                {
                    throw new DB_ItemUnreachableException(item.GetShortID());
                }
            }
            catch (PropertyModificationException e)
            {
                ErrorLogger(e, e.GeneralError);
                throw e;
            }
            catch (DB_ItemUnreachableException e)
            {
                ErrorLogger(e, e.GeneralError);
                throw e;
            }
        }
        public static void ChangeItemPublisher(AbstractItem item, string newPublisher)
        {
            try
            {
                if (DataBase.LibraryItems.ContainsKey(item.ISBN))
                {
                    if (PropertyGuidelines.Check_Publisher_Length(newPublisher))
                    {
                        DataBase.LibraryItems[item.ISBN].Publisher = newPublisher;
                    }
                    else
                    {
                        throw new PropertyModificationException($"{PropertyGuidelines.Message_Publisher_Length_Invalid}", item.GetShortID());
                    }
                }
                else
                {
                    throw new DB_ItemUnreachableException(item.GetShortID());
                }
            }
            catch (PropertyModificationException e)
            {
                ErrorLogger(e, e.GeneralError);
                throw e;
            }
            catch (DB_ItemUnreachableException e)
            {
                ErrorLogger(e, e.GeneralError);
                throw e;
            }
        }
        public static void ChangeAuthor(Book item, string newAuthor)
        {
            try
            {
                if (DataBase.LibraryItems.ContainsKey(item.ISBN) && DataBase.LibraryItems[item.ISBN] is Book)
                {
                    if (PropertyGuidelines.Check_Author_Length(newAuthor))
                    {
                        //casting item from dictionary to access derived property
                        Book book = (Book)DataBase.LibraryItems[item.ISBN];
                        book.Author = newAuthor;
                        DataBase.LibraryItems[item.ISBN] = book;    //replacing with casted object that contains the neccessary changes
                    }
                    else
                    {
                        throw new PropertyModificationException($"{PropertyGuidelines.Message_Author_Length_Invalid}", item.GetShortID());
                    }
                }
                else //if isbn is not assigned to Book typed object or given isbn is not in DB
                {
                    throw new DB_ItemUnreachableException(item.GetShortID());
                }
            }
            catch (PropertyModificationException e)
            {
                ErrorLogger(e, e.GeneralError);
                throw e;
            }
            catch (DB_ItemUnreachableException e)
            {
                ErrorLogger(e, e.GeneralError);
                throw e;
            }
        }
        public static void ChangeEditionNumber(Journal item, int newEditionNum)
        {
            try
            {
                if (DataBase.LibraryItems.ContainsKey(item.ISBN) && DataBase.LibraryItems[item.ISBN] is Journal)
                {
                    if (PropertyGuidelines.Check_Edition_Number(newEditionNum))
                    {
                        //casting item from dictionary to access derived property
                        Journal journal = (Journal)DataBase.LibraryItems[item.ISBN];
                        journal.EditionNumber = newEditionNum;
                        DataBase.LibraryItems[item.ISBN] = journal;    //replacing with casted object that contains the neccessary changes
                    }
                    else
                    {
                        throw new PropertyModificationException($"{PropertyGuidelines.Message_Edition_Number_Invalid}", item.GetShortID());
                    }
                }
                else //if isbn is not assigned to Book typed object or given isbn is not in DB
                {
                    throw new DB_ItemUnreachableException(item.GetShortID());
                }
            }
            catch (PropertyModificationException e)
            {
                ErrorLogger(e, e.GeneralError);
                throw e;
            }
            catch (DB_ItemUnreachableException e)
            {
                ErrorLogger(e, e.GeneralError);
                throw e;
            }
        }
        public static void ChangePublishingDate(AbstractItem item, DateTime dt)
        {
            try
            {
                if (DataBase.LibraryItems.ContainsKey(item.ISBN))
                {
                    DataBase.LibraryItems[item.ISBN].PrintDate = dt;
                }
                else
                {
                    throw new DB_ItemUnreachableException(item.GetShortID());
                }
            }

            catch (DB_ItemUnreachableException e)
            {
                ErrorLogger(e, e.GeneralError);
                throw e;
            }
        }
        public static void ChangePrice(AbstractItem item, double newPrice)
        {
            try
            {
                if (DataBase.LibraryItems.ContainsKey(item.ISBN))
                {
                    if (PropertyGuidelines.Check_Price_Validity(newPrice))
                    {
                        //update price - discount is updated automatically through the CurrentPrice property
                        DataBase.LibraryItems[item.ISBN].CurrentPrice = newPrice;
                    }
                    else
                    {
                        throw new PropertyModificationException($"{PropertyGuidelines.Message_Price_Invalid}", item.GetShortID());
                    }
                }
                else
                {
                    throw new DB_ItemUnreachableException(item.GetShortID());
                }
            }
            catch (PropertyModificationException e)
            {
                ErrorLogger(e, e.GeneralError);
                throw e;
            }
            catch (DB_ItemUnreachableException e)
            {
                ErrorLogger(e, e.GeneralError);
                throw e;
            }
        }

        public static void ChangeDiscountPrecent(AbstractItem item, int newDiscount)
        {
            try
            {
                if (DataBase.LibraryItems.ContainsKey(item.ISBN))
                {
                    if (PropertyGuidelines.Check_Discount_Validity(newDiscount))
                    {
                        DataBase.LibraryItems[item.ISBN].DiscountPercent = newDiscount;
                        //currentPrice is updated automatically through the Discount property
                    }
                    else
                    {
                        throw new PropertyModificationException($"{PropertyGuidelines.Message_Discount_Invalid}", item.GetShortID());
                    }
                }
                else
                {
                    throw new DB_ItemUnreachableException(item.GetShortID());
                }
            }
            catch (PropertyModificationException e)
            {
                ErrorLogger(e, e.GeneralError);
                throw e;
            }
            catch (DB_ItemUnreachableException e)
            {
                ErrorLogger(e, e.GeneralError);
                throw e;
            }

        }
        public static void ChangeCatagory(AbstractItem item, CatagoryEnum newCatagory)
        {
            try
            {
                if (DataBase.LibraryItems.ContainsKey(item.ISBN))
                {
                    DataBase.LibraryItems[item.ISBN].Catagory = newCatagory;
                }
                else
                {
                    throw new DB_ItemUnreachableException(item.GetShortID());
                }
            }

            catch (DB_ItemUnreachableException e)
            {
                ErrorLogger(e, e.GeneralError);
                throw e;
            }
        }
        #endregion







        public static void ErrorLogger(Exception e, string logMessage)
        {
            File.AppendAllText(_logFileName, $"[{DateTime.Now.ToString()}]\t[{logMessage}]\n");
        }
        private static string InitializeLog()
        {
            _logFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"[{DateTime.Now.Day}-{DateTime.Now.Month}-{DateTime.Now.Year}]ErrorLog.txt");
            string errorMassase;

            try
            {
                if (File.Exists(_logFileName))
                {
                    File.Delete(_logFileName);
                }
            }
            catch (Exception e)
            {
                errorMassase = e.Message;
                return errorMassase;
            }

            return "Log initialized";
        }

    }
}
