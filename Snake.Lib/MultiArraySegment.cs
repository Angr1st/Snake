using System;

namespace Snake.Lib
{
    public class MultiArraySegment<T>
    {
        private T[,] InnerArray { get; }
        private int OffsetX { get; }
        private int OffsetY { get; }
        private int CountX { get; }
        private int CountY { get; }

        private MultiArraySegment(T[,] multiDimensionalArray, int offsetX, int offsetY, int countX, int countY)
        {
            InnerArray = multiDimensionalArray;
            OffsetX = offsetX;
            OffsetY = offsetY;
            CountX = countX;
            CountY = countY;
        }

        public T this[int i, int j]
        {
            get
            {
                CheckBounds(i, j);
                return InnerArray[i + OffsetX, j + OffsetY];
            }
            set
            {
                CheckBounds(i, j);
                InnerArray[i + OffsetX, j + OffsetY] = value;
            }
        }

        private void CheckBounds(int i, int j)
        {
            if (i < 0 || i > CountX)
            {
                throw new IndexOutOfRangeException($"i:{i}; OffsetX:{OffsetX}; CountX:{CountX}");
            }
            if (j < 0 || j > CountY)
            {
                throw new IndexOutOfRangeException($"j:{j}; OffsetY:{OffsetY}; CountY:{CountY}");
            }
        }

        public static MultiArraySegment<T> Create(T[,] multiDimensionalArray, int offsetX, int offsetY, int countX, int countY)
        {
            if (multiDimensionalArray is null)
            {
                throw new ArgumentNullException(nameof(multiDimensionalArray));
            }

            CheckBound(multiDimensionalArray, offsetX, 0, nameof(offsetX));
            CheckBound(multiDimensionalArray, offsetY, 1, nameof(offsetY));
            CheckCount(multiDimensionalArray, offsetX, countX, 0, nameof(countX));
            CheckCount(multiDimensionalArray, offsetY, countY, 1, nameof(countY));

            return new MultiArraySegment<T>(multiDimensionalArray, offsetX, offsetY, countX, countY);

            static void CheckBound(T[,] multiDimensionalArray, int bound, int dimension, string name)
            {
                if (bound < 0 || bound >= multiDimensionalArray.GetLength(dimension))
                {
                    throw new ArgumentOutOfRangeException(name);
                }
            }

            static void CheckCount(T[,] multiDimensionalArray, int bound, int count, int dimension, string name)
            {
                if (count < 0)
                {
                    throw new ArgumentOutOfRangeException(name);
                }
                CheckBound(multiDimensionalArray, bound + count, dimension, name);
            }
        }
    }
}
