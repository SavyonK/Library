using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;

namespace BookLib
{
    internal class DataBase
    {
        static DataBase _db;
        public static double MoneyEarned;
        public static DataBase DB
        {
            get
            {
                if (_db is null)
                {
                    _db = new DataBase();
                }
                return _db;
            }
        }

        /// <summary>
        /// holds entire collection of abstract items
        /// </summary>
        public static Dictionary<string, AbstractItem> LibraryItems { get; private set; }


        /// <summary>
        /// hold all orders. key is order number
        /// </summary>
        public static Dictionary<int, Order> Orders { get; private set; }



        private DataBase()//private constructor, invoked only once - when prop 'DB' is called for the first time
        {
        //lists are initialized in this ctor and then available for entire runtime
            LibraryItems = new Dictionary<string, AbstractItem>();
            Orders = new Dictionary<int, Order>();


            MoneyEarned = 0;



        }

       

    }
}