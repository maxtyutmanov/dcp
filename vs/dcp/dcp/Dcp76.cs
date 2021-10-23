using System;
using System.Collections.Generic;
using System.Text;

namespace dcp
{
    /*
     This problem was asked by Google.

     You are given an N by M 2D matrix of lowercase letters. Determine the minimum number of columns that can be removed to ensure that each row is ordered from top to bottom lexicographically. 
     That is, the letter at each column is lexicographically later as you go down each row. It does not matter whether each row itself is ordered lexicographically.
     
     For example, given the following table:
     
     cba
     daf
     ghi
     This is not ordered because of the a in the center. We can remove the second column to make it ordered:
     
     ca
     df
     gi
     So your function should return 1, since we only needed to remove 1 column.
     
     As another example, given the following table:
     
     abcdef
     Your function should return 0, since the rows are already ordered (there's only one row).
     
     As another example, given the following table:
     
     zyx
     wvu
     tsr
     Your function should return 3, since we would need to remove all the columns to order it.
      */

    public static class Dcp76
    {
        public static void Test()
        {
            var m = new[,]
            {
                { 'c', 'b', 'a' },
                { 'd', 'a', 'f' },
                { 'g', 'h', 'i' },
            };

            Console.WriteLine("Case 1 result (expected 1): {0}", Solve(m));

            m = new[,] { { 'a', 'b', 'c', 'd', 'e', 'f' } };

            Console.WriteLine("Case 2 result (expected 0): {0}", Solve(m));

            m = new[,]
            {
                { 'z', 'y', 'x' },
                { 'w', 'v', 'u' },
                { 't', 's', 'r' },
            };

            Console.WriteLine("Case 3 result (expected 3): {0}", Solve(m));
        }

        public static int Solve(char[,] m)
        {
            var n = m.GetLength(0);
            var toDelete = 0;

            for (int j = 0; j < n; j++)
            {
                if (!IsColumnOrdered(m, j))
                    toDelete++;
            }

            return toDelete;
        }

        private static bool IsColumnOrdered(char[,] m, int j)
        {
            var n = m.GetLength(0);

            var prev = m[0, j];

            for (int i = 1; i < n; i++)
            {
                if (m[i, j].CompareTo(prev) < 0)
                    return false;
            }

            return true;
        }
    }
}
