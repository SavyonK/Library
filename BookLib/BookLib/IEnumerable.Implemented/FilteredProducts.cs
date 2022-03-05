using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store;

namespace BookLib
{
    public class FilteredProducts : IEnumerable<AbstractItem>
    {

        public List<AbstractItem> list { get; set; }
        public FilteredProducts(List<AbstractItem> pList)
        {
            list = new List<AbstractItem>();
            for (int i = 0; i < pList.Count; i++)
            { list.Add(pList[i]); }
           
        }

        public void ReplaceCurrentList(List<AbstractItem> newList)
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

     

        // Must implement GetEnumerator, which returns a new StreamReaderEnumerator.
        public IEnumerator<AbstractItem> GetEnumerator()
        {
            return new ProductsEnum(list);
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

        private class ProductsEnum : IEnumerator<AbstractItem>
        {
            private List<AbstractItem> _list;
            // private AbstractItem[] _products;
            int position = -1;

            public ProductsEnum(List<AbstractItem> list)
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

                //                throw new NotImplementedException();
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

            public AbstractItem Current => _list[position];
        }
    }
}
