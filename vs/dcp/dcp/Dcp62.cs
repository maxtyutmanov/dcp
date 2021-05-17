using System;
using System.Collections.Generic;
using System.Text;

namespace dcp
{
    /*
     *  This problem was asked by Facebook.

        There is an N by M matrix of zeroes. Given N and M, write a function to count the number of ways of starting at the 
        top-left corner and getting to the bottom-right corner. You can only move right or down.

        For example, given a 2 by 2 matrix, you should return 2, since there are two ways to get to the bottom-right:

        Right, then down
        Down, then right
        Given a 5 by 5 matrix, there are 70 ways to get to the bottom-right.
     */

    public static class Dcp62
    {
        public static long GetNumberOfOptions(int n, int m)
        {
            var cache = new long[n, m];
            return GetNumberOfOptionsDp(n - 1, m - 1, cache);
        }

        private static long GetNumberOfOptionsDp(int n, int m, long[,] cache)
        {
            if (n == 0 && m == 0)
                return 1;   // there is only one "option" to start at 0,0 

            if (n < 0 || m < 0)
                return 0;

            if (cache[n, m] > 0)
                return cache[n, m];

            // sum of options if we got here from above PLUS if we got here from left
            var totalOptions = GetNumberOfOptionsDp(n - 1, m, cache) + GetNumberOfOptionsDp(n, m - 1, cache);
            cache[n, m] = totalOptions;

            return totalOptions;
        }

        private static long GetNumberOfOptionsNaive(int n, int m)
        {
            // n - num of columns, m - num of rows

            if (n > 1 && m > 1)
            {
                // what if we 1) move right? 2) move down? 
                return GetNumberOfOptionsNaive(n - 1, m) + GetNumberOfOptionsNaive(n, m - 1);
            }

            return 1; // there is only 1 way to finish the path - only right or only down
        }
    }
}
