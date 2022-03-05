using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLib
{
    class FilteredBooks : IEnumerable<Book>
    {

        public List<Book> list { get; set; }
        public FilteredBooks(List<Book> bList)
        {
            list = new List<Book>();
            for (int i = 0; i < bList.Count; i++)
            { list.Add(bList[i]); }
         
        }

        public void ReplaceCurrentList(List<Book> newList)
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
        public IEnumerator<Book> GetEnumerator()
        {
            return new BookEnum(list);
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

        private class BookEnum : IEnumerator<Book>
        {
            private List<Book> _list;
            int position = -1;

            public BookEnum(List<Book> list)
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

            public Book Current => _list[position];

        }
    }
}
