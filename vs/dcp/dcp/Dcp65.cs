using System;
using System.Collections.Generic;
using System.Text;

namespace dcp
{
    /*
     This problem was asked by Amazon.

     Given a N by M matrix of numbers, print out the matrix in a clockwise spiral.
     
     For example, given the following matrix:
     
     [[1,  2,  3,  4,  5],
      [6,  7,  8,  9,  10],
      [11, 12, 13, 14, 15],
      [16, 17, 18, 19, 20]]

     You should print out the following:
     
     1
     2
     3
     4
     5
     10
     15
     20
     19
     18
     17
     16
     11
     6
     7
     8
     9
     14
     13
     12*/

    public class Dcp65
    {
        public static void Test()
        {
            var matrix1 = new int[,] {
                {1, 2, 3, 4, 5},
                {6, 7, 8, 9, 10},
                {11, 12, 13, 14, 15},
                {16, 17, 18, 19, 20}
            };

            Console.WriteLine(string.Join(", ", EnumerateClockwise(matrix1)));

            var matrix2 = new int[,] { { 1, 2 } };
            Console.WriteLine(string.Join(", ", EnumerateClockwise(matrix2)));

            var matrix3 = new int[,] { { 1 }, { 2 } };
            Console.WriteLine(string.Join(", ", EnumerateClockwise(matrix3)));

            var matrix4 = new int[,] { { 1 } };
            Console.WriteLine(string.Join(", ", EnumerateClockwise(matrix4)));
        }

        public static IEnumerable<int> EnumerateClockwise(int[,] matrix)
        {
            var (rows, cols) = (matrix.GetLength(0), matrix.GetLength(1));
            return EnumerateClockwise(matrix, 0, 0, rows - 1, cols - 1);
        }

        private static IEnumerable<int> EnumerateClockwise(int[,] matrix, int startI, int startJ, int endI, int endJ)
        {
            if (startI > endI || startJ > endJ)
                yield break;

            // enumerate first row
            for (var j = startJ; j <= endJ; j++)
            {
                yield return matrix[startI, j];
            }

            // enumerate last column
            for (var i = startI + 1; i <= endI; i++)
            {
                yield return matrix[i, endJ];
            }

            // enumerate last row, reversed
            if (startI != endI)
            {
                for (int j = endJ - 1; j >= startJ; j--)
                {
                    yield return matrix[endI, j];
                }
            }

            // enumerate first column, reversed
            if (startJ != endJ)
            {
                for (int i = endI - 1; i >= startI + 1; i--)
                {
                    yield return matrix[i, startJ];
                }
            }

            var rest = EnumerateClockwise(matrix, startI + 1, startJ + 1, endI - 1, endJ - 1);
            foreach (var item in rest)
                yield return item;
        }
    }
}
