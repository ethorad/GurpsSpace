using System.Collections.Generic;

namespace GurpsSpace
{
    internal class IndexedTable1D<T>
    {
        private readonly T[] table;
        private readonly int startIndex;

        public T this[int i]
        {
            get
            {
                i -= startIndex;
                if (i < 0)
                    i = 0;
                if (i >= table.Length)
                    i = table.Length - 1;
                return table[i];
            }
        }

        public IndexedTable1D(T[] table, int startIndex = 0)
        {
            this.table = table;
            this.startIndex = startIndex;
        }
    }

    internal class IndexedList<T>
    {
        private List<T> list;
        private int startIndex;

        public T this[int i]
        {
            get
            {
                i = i - startIndex;
                if (i >= list.Count)
                    i = list.Count - 1;
                if (i<0)
                    i = 0;
                return list[i];
            }
        }
        public int Count { get { return list.Count; } }

        public IndexedList(List<T> list, int startIndex = 0)
        {
            this.list = list;
            this.startIndex = startIndex;
        }
        public IndexedList()
        {
            list = new List<T>();
            startIndex = 0;
        }

        public void Add(T item)
        {
            list.Add(item);
        }
        public void Clear()
        {
            list.Clear();
        }
        public bool Contains(T item)
        {
            return list.Contains(item);
        }
    }
}
