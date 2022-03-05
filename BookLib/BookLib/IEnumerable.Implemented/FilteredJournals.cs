using BookLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JournalLib
{
    class FilteredJournals : IEnumerable<Journal>
    {

        public List<Journal> list { get; set; }
        public FilteredJournals(List<Journal> jList)
        {
            list = new List<Journal>();
            for (int i = 0; i < jList.Count; i++)
            { list.Add(jList[i]); }

        }

        public void ReplaceCurrentList(List<Journal> newList)
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
        public IEnumerator<Journal> GetEnumerator()
        {
            return new JournalEnum(list);
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

        private class JournalEnum : IEnumerator<Journal>
        {
            private List<Journal> _list;
            int position = -1;

            public JournalEnum(List<Journal> list)
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

            public Journal Current => _list[position];

        }

    }
}