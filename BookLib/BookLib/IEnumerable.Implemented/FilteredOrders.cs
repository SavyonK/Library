using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLib
{
    class FilteredOrders : IEnumerable<Order>
    {


        public List<Order> list { get; set; }
        public FilteredOrders(List<Order> oList)
        {
            list = new List<Order>();
            for (int i = 0; i < oList.Count; i++)
            { list.Add(oList[i]); }
          
        }

        public void ReplaceCurrentList(List<Order> newList)
        {
            list.Clear();
            for (int i = 0; i < newList.Count; i++)
            { list.Add(newList[i]); }
        }
        public void ClearAll()
        {
            list.Clear();
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in list)
            { sb.AppendLine(list.ToString()); }
            return sb.ToString();
        }



        public IEnumerator<Order> GetEnumerator()
        {
            return new OrdersEnum(list);
        }

        // Must also implement IEnumerable.GetEnumerator, but implement as a private method.
        private IEnumerator GetEnumerator1()
        {
            return this.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator1();

        }

        private class OrdersEnum : IEnumerator<Order>
        {
            private List<Order> _list;
            // private Order[] _products;
            int position = -1;

            public OrdersEnum(List<Order> list)
            {
                _list = list;

            }
            public bool MoveNext()
            {
                return (++position < _list.Count);
            }

            public void Reset()
            {
                position = -1;

            }

            public void Dispose()
            {
                GC.SuppressFinalize(this);

            }




            /// <summary>
            /// when referring to object in certain iteration - its by using this property
            /// </summary>
            object IEnumerator.Current
            {
                get
                {
                    try
                    {
                        return _list[position];
                    }

                    catch (IndexOutOfRangeException)
                    { throw new InvalidOperationException(); }
                }

            }

            public Order Current => _list[position];
        }



    }
}
