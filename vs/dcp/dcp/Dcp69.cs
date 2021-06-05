using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcp
{
    /*
    This problem was asked by Facebook.

    Given a list of integers, return the largest product that can be made by multiplying any three integers.

    For example, if the list is [-10, -10, 5, 2], we should return 500, since that's -10 * -10 * 5.

    You can assume the list has at least three integers.
     */

    public static class Dcp69
    {
        public static void Test()
        {
            var xs = new List<int>() { -10, -10, 5, 2 };
            Console.WriteLine("Test case #1 (500): {0}", Solve(xs));

            xs = new List<int>() { -10, 10, 5, 2 };
            Console.WriteLine("Test case #2 (100): {0}", Solve(xs));

            xs = new List<int>() { -10, -10, -5, -2 };
            Console.WriteLine("Test case #3 (-100): {0}", Solve(xs));

            xs = new List<int>() { -10, -10, 0, -5, -2 };
            Console.WriteLine("Test case #4 (0): {0}", Solve(xs));

            xs = new List<int>() { -10, -10, 0, -5, -2, 100, 100, 100 };
            Console.WriteLine("Test case #5 (1000000): {0}", Solve(xs));
        }

        public static long Solve(IReadOnlyList<int> xs)
        {
            // find max product of
            // 1. two negative numbers + 1 positive number
            // 2. 3 largest numbers
            // 3. if there are no positive numbers, find max product of 3 largest negative numbers

            // max product can be composed of: 
            // 1. two smallest numbers (if they are negative) multiplied by 1 largest number (if it is positive)
            // 2. 3 largest numbers, regardless of their sign

            xs = xs.OrderBy(x => x).ToList();
            var prodWithNegatives = xs[0] * xs[1] * xs[^1];
            var prodOfLargest = xs[^1] * xs[^2] * xs[^3];
            return Math.Max(prodWithNegatives, prodOfLargest);
        }
    }
}
