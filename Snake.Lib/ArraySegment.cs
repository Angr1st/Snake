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
        private TwoDimensions Dimension { get; }
        private int Offset { get; }
        private int OtherDimensionOffset { get; }
        private int Count { get; }
        public int Length { get; }

        private ArraySegment(T[,] multiDimensionalArray, TwoDimensions dimension, int offset,  int count, int otherDimensionOffset)
        {
            InnerArray = multiDimensionalArray;
            Offset = offset;
            OtherDimensionOffset = otherDimensionOffset;
            Count = count;
            Length = multiDimensionalArray.GetLength((int)dimension) - (offset + 1);
        }

        public T this[int i]
        {
            get
            {
                CheckBound(i);
                return Dimension switch
                {
                    TwoDimensions.First => InnerArray[i + Offset, OtherDimensionOffset],
                    TwoDimensions.Second => InnerArray[OtherDimensionOffset, i + Offset],
                    _ => throw new InvalidOperationException($"The value of the enum is not recognised!")
                };
            }
            set
            {
                CheckBound(i);
                switch (Dimension)
                {
                    case TwoDimensions.First:
                        InnerArray[i + Offset, OtherDimensionOffset] = value;
                        break;

                    case TwoDimensions.Second:
                        InnerArray[OtherDimensionOffset, i + Offset] = value;
                        break;

                    default:
                        throw new InvalidOperationException($"The value of the enum is not recognised!");
                }    
            }
        }

        private void CheckBound(int i)
        {
            if (i < 0 || i > Count)
            {
                throw new IndexOutOfRangeException($"i:{i}; Offset:{Offset}; Count:{Count}; Dimension:{Dimension};");
            }
        }

        public static ArraySegment<T> Create(T[,] multiDimensionalArray, TwoDimensions dimension, int offset, int count, int otherDimensionOffset)
        {
            if (multiDimensionalArray is null)
            {
                throw new ArgumentNullException(nameof(multiDimensionalArray));
            }

            CheckBound(multiDimensionalArray, offset, (int)dimension, nameof(offset));
            CheckBound(multiDimensionalArray, otherDimensionOffset, GetOpposite(dimension), nameof(otherDimensionOffset));
            CheckCount(multiDimensionalArray, offset, count, (int)dimension, nameof(count));

            return new ArraySegment<T>(multiDimensionalArray, dimension, offset,  count, otherDimensionOffset);

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

            static int GetOpposite(TwoDimensions twoDimensions)
                => twoDimensions switch
                {
                    TwoDimensions.First => (int)TwoDimensions.Second,
                    TwoDimensions.Second => (int)TwoDimensions.First,
                    _ => throw new InvalidOperationException($"The value of the enum is not recognised!")
                };
        }
    }
}
