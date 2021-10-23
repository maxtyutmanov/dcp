using System;
using System.Collections.Generic;
using System.Text;

namespace dcp
{
    /*
    This problem was asked by Microsoft.

    Given an array of numbers, find the length of the longest increasing subsequence in the array. The subsequence does not necessarily have to be contiguous.

    For example, given the array [0, 8, 4, 12, 2, 10, 6, 14, 1, 9, 5, 13, 3, 11, 7, 15], the longest increasing subsequence has length 6: it is 0, 2, 6, 9, 11, 15.
     */

    public static class Dcp75
    {
        public static void Test()
        {
            var xs = new[] { 0, 8, 4, 12, 2, 10, 6, 14, 1, 9, 5, 13, 3, 11, 7, 15 };
            Console.WriteLine("Case 1 result (expected 6): {0}", Solve(xs));

            xs = new int[0];
            Console.WriteLine("Case 2 result (expected 0): {0}", Solve(xs));

            xs = new[] { 5, 4, 3, 2, 5 };
            Console.WriteLine("Case 3 result (expected 2): {0}", Solve(xs));

            xs = new[] { 5, 4, 3, 2, 1 };
            Console.WriteLine("Case 4 result (expected 1): {0}", Solve(xs));
        }

        public static int Solve(int[] xs)
        {
            var subResults = new int[xs.Length];
            for (int i = 0; i < subResults.Length; i++)
            {
                subResults[i] = -1;
            }

            var maxLen = 0;
            for (int i = 0; i < xs.Length; i++)
            {
                var len = SubSolveNaive(xs, i, subResults);
                if (len > maxLen)
                    maxLen = len;
            }

            return maxLen;
        }

        private static int SubSolveNaive(int[] xs, int index, int[] subResults)
        {
            if (index >= xs.Length)
                return 0;

            var currentItem = xs[index];
            var maxLen = 1;

            for (int i = index + 1; i < xs.Length; i++)
            {
                if (xs[i] > currentItem)
                {
                    int len;

                    if (subResults[i] != -1)
                        len = 1 + subResults[i];
                    else
                        len = 1 + SubSolveNaive(xs, i, subResults);

                    if (len > maxLen)
                        maxLen = len;
                }
            }

            subResults[index] = maxLen;
            return maxLen;
        }
    }
}
