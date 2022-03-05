using BookLib.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLib
{
    public class AbstractItem
    {
        #region Properties
        public CatagoryEnum Catagory { get; set; }
        public char Currency { get; private set; }

        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                try
                {
                    if (!PropertyGuidelines.Check_Title_Validity(value))
                    {
                        throw new PropertyModificationException($"{PropertyGuidelines.Message_Title_Length_Invalid}", $"ISBN: {ISBN}, Title given: {value}");
                    }
                    else
                    {
                        _title = value;
                    }
                }
                catch (PropertyModificationException e)
                {
                    LibraryManager.ErrorLogger(e, e.Message);
                    throw e;
                }
            }
        }

        private string _publisher;
        public string Publisher
        {
            get { return _publisher; }
            set
            {
                try
                {
                    if (PropertyGuidelines.Check_Publisher_Length(value))
                    {
                        _publisher = value;
                    }
                    else
                    {
                        throw new PropertyModificationException(PropertyGuidelines.Message_Publisher_Length_Invalid, GetShortID());
                    }
                }
                catch (PropertyModificationException e)
                {
                    LibraryManager.ErrorLogger(e, e.Message);
                    throw e;
                }
            }
        }

        private string _summery;
        public string Summery
        {
            get { return _summery; }
            set
            {
                try
                {
                    if (!PropertyGuidelines.Check_Summery_Length(value))
                    {
                        throw new PropertyModificationException($"{PropertyGuidelines.Message_Summery_Length_Invalid}", GetShortID());
                    }
                    else
                    {
                        _summery = value;
                    }
                }
                catch (PropertyModificationException e)
                {
                    LibraryManager.ErrorLogger(e, e.Message);
                    throw e;
                }
            }
        }
        private int _quantity;
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                try
                {
                    if (!PropertyGuidelines.Check_Quantity_Validity(value))
                    {
                        throw new PropertyModificationException($"{PropertyGuidelines.Message_Quantity_Invalid}", GetShortID());
                    }
                    else
                    {
                        _quantity = value;
                    }
                }
                catch (PropertyModificationException e)
                {
                    LibraryManager.ErrorLogger(e, e.Message);
                    throw e;
                }
            }
        }

        public DateTime PrintDate { get; set; }
        private string _isbn;
        public string ISBN
        {
            //isbn duplication is handled when item is added to library
            get { return _isbn; }
            set
            {
                try
                {
                    if (!PropertyGuidelines.Check_ISBN_Validity(value))
                    {
                        throw new PropertyModificationException(PropertyGuidelines.Message_ISBN_Invalid, $"isbn given: {value}, Title: {Title}");
                    }

                    else
                    { _isbn = value; }
                }
                catch (PropertyModificationException e)
                {
                    LibraryManager.ErrorLogger(e, e.Message);
                    throw e;
                }
            }
        }
        private int _discount;
        public int DiscountPercent
        {
            get { return _discount; }
            set
            {
                try
                {
                    if (PropertyGuidelines.Check_Discount_Validity(value))
                    {
                        _discount = value;

                        //change current price by discount
                        _currentPrice = ((100 - DiscountPercent) / 100.00) * OriginalPrice;
                    }
                    else
                    {
                        throw new PropertyModificationException($"Discount percent cannot be {value} - {PropertyGuidelines.Message_Discount_Invalid}", GetShortID());
                    }
                }
                catch (PropertyModificationException e)
                {
                    LibraryManager.ErrorLogger(e, e.Message);
                    throw e;
                }
            }
        }
        private double _originalPrice;
        public double OriginalPrice
        {
            get { return _originalPrice; }
            private set
            {
                try
                {
                    if (PropertyGuidelines.Check_Price_Validity(value))
                    {
                        _originalPrice = value;
                    }
                    else
                    {
                        throw new PropertyModificationException($"Discount percent cannot be {value} - {PropertyGuidelines.Message_Discount_Invalid}", GetShortID());
                    }
                }
                catch (PropertyModificationException e)
                {
                    LibraryManager.ErrorLogger(e, e.Message);
                    throw e;
                }
            }
        }
        private double _currentPrice;
        public double CurrentPrice
        {
            get { return _currentPrice; }
            internal set
            {
                try
                {
                    if (PropertyGuidelines.Check_Price_Validity(value))
                    {
                        _currentPrice = value;

                        //change discount by price
                        _discount = (int)((OriginalPrice - CurrentPrice) / OriginalPrice);
                    }
                    else
                    {
                        throw new PropertyModificationException($"{PropertyGuidelines.Message_Price_Invalid}", GetShortID());
                    }
                }
                catch (PropertyModificationException e)
                {
                    LibraryManager.ErrorLogger(e, e.Message);
                    throw e;
                }
            }
        }
        /// <summary>
        /// mark false if item is retired and is not to be restocked
        /// active state of item does not deny possibility 0 units in stock which means - temporarily out of stock
        /// </summary>
        public bool isActive { get; set; }
        #endregion
        public AbstractItem(string title, string publisher, string summery, double price, string isbn, DateTime printDate,
                            CatagoryEnum catagory, int quantity, bool is_active, int discountPercent = 0)
        {
            Title = title;
            Publisher = publisher;
            Summery = summery;
            ISBN = isbn;
            PrintDate = printDate;
            Catagory = catagory;
            OriginalPrice = price;
            DiscountPercent = discountPercent;
            Quantity = quantity;
            isActive = is_active;
            Currency = PropertyGuidelines.Currency;

        }

        public string GetShortID()
        {
            return $"[{ISBN} - {Title}]";
        }


        public override string ToString()
        {
            string status = isActive ? "Active" : "Retired";

            StringBuilder sb = new StringBuilder();
           
            sb.Append("Type: ***,  ");
            sb.Append($"ISBN: {ISBN},  ");
            sb.AppendLine($"Status: {status}");
            sb.Append($"Title: {Title},  ");
            sb.AppendLine($"Catagory: {Catagory}");
            
            sb.Append($"Print Date: {PrintDate.Day}/{PrintDate.Month}/{PrintDate.Year},  ");
            sb.Append($"Quantity: {Quantity},  ");
            sb.AppendLine($"Publisher: {Publisher}");

            sb.Append($"Original Price: {OriginalPrice},  ");
            sb.Append($"Discount: {DiscountPercent}%,  ");
            sb.Append($"Current Price: {CurrentPrice}");
            return sb.ToString();


        }


    }
}
