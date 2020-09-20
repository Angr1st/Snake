using System;

namespace Snake.Lib
{
    public enum TwoDimensions
    {
        First = 0,
        Second = 1
    }

    public class ArraySegment<T>
    {
        private T[,] InnerArray { get; }
        private int Dimension { get; }
        private int OffsetX { get; }
        private int CountX { get; }
        private int Length { get; }

        private ArraySegment(T[,] multiDimensionalArray, int dimension, int offsetX,  int countX)
        {
            InnerArray = multiDimensionalArray;
            OffsetX = offsetX;
            CountX = countX;
            Length = multiDimensionalArray.GetLength(dimension) - countX;
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

        public int Count { 
            get
            {
                return Length;
            } 
        }

        private void CheckBound(int i)
        {
            if (i < 0 || i + OffsetX > CountX)
            {
                throw new IndexOutOfRangeException($"i:{i}; OffsetX:{OffsetX}; CountX:{CountX}");
            }
        }

        public static ArraySegment<T> Create(T[,] multiDimensionalArray, TwoDimensions dimension, int offsetX, int countX)
        {
            if (multiDimensionalArray is null)
            {
                throw new ArgumentNullException(nameof(multiDimensionalArray));
            }

            CheckBound(multiDimensionalArray, offsetX, (int)dimension, nameof(offsetX));
            CheckCount(multiDimensionalArray, offsetX, countX, (int)dimension, nameof(countX));

            return new ArraySegment<T>(multiDimensionalArray, (int)dimension, offsetX,  countX);

            static void CheckBound(T[,] multiDimensionalArray, int bound, int dimension, string name)
            {
                if (bound < 0 || bound >= multiDimensionalArray.GetLength(dimension))
                {
                    throw new ArgumentOutOfRangeException(name);
                }
            }

            static void CheckCount(T[,] array, int bound, int count, int dimension, string name)
            {
                if (count < 0)
                {
                    throw new ArgumentOutOfRangeException(name);
                }
                CheckBound(array, bound + count, dimension, name);
            }
        }
    }
}
