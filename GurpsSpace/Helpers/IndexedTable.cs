
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
}
