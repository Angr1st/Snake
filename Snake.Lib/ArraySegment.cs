using System;

namespace Snake.Lib
{
    public class ArraySegment<T>
    {
        private T[,] InnerArray { get; }
        private int Dimension { get; }
        private int OffsetX { get; }
        private int CountX { get; }

        private ArraySegment(T[,] multiDimensionalArray, int dimension, int offsetX,  int countX)
        {
            InnerArray = multiDimensionalArray;
            OffsetX = offsetX;
            CountX = countX;
        }

        public T this[int i]
        {
            get
            {
                CheckBound(i);
                return InnerArray[Dimension, i + OffsetX];
            }
            set
            {
                CheckBound(i);
                InnerArray[Dimension, i + OffsetX] = value;
            }
        }

        private void CheckBound(int i)
        {
            if (i < 0 || i + OffsetX > CountX)
            {
                throw new IndexOutOfRangeException($"i:{i}; OffsetX:{OffsetX}; CountX:{CountX}");
            }
        }

        public static ArraySegment<T> Create(T[,] multiDimensionalArray, int dimension, int offsetX, int countX)
        {
            if (multiDimensionalArray is null)
            {
                throw new ArgumentNullException(nameof(multiDimensionalArray));
            }

            CheckBound(multiDimensionalArray, offsetX, nameof(offsetX));
            CheckCount(multiDimensionalArray, offsetX, countX, nameof(countX));

            return new ArraySegment<T>(multiDimensionalArray, dimension, offsetX,  countX);

            static void CheckBound(T[,] multiDimensionalArray, int bound, string name)
            {
                if (bound < 0 || bound >= multiDimensionalArray.Length)
                {
                    throw new ArgumentOutOfRangeException(name);
                }
            }

            static void CheckCount(T[] array, int bound, int count, string name)
            {
                if (count < 0)
                {
                    throw new ArgumentOutOfRangeException(name);
                }
                CheckBound(array, bound + count, name);
            }
        }
    }
}
