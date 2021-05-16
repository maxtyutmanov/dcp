using System;
using System.Collections.Generic;
using System.Text;

namespace dcp
{
    /*
    This problem was asked by Google.

    Implement integer exponentiation. That is, implement the pow(x, y) function, where x and y are integers and returns x^y.

    Do this faster than the naive method of repeated multiplication.

    For example, pow(2, 10) should return 1024.
    */

    public static class Dcp61
    {
        public static long Pow(long x, int y)
        {
            var multiplicationsFast = 0;

            var naive = PowNaive(x, y);
            var fast = PowFast(x, y, ref multiplicationsFast);

            if (naive != fast)
                throw new Exception("Oops, better right than fast");

            Console.WriteLine("Multiplications for slow algo: {0}", y);
            Console.WriteLine("Multiplications for fast algo: {0}", multiplicationsFast);

            return fast;
        }

        private static long PowFast(long x, int y, ref int multiplications)
        {
            if (y == 0)
                return 1;

            if (y % 2 == 0)
            {
                // the main idea is that 2^10 = (2^5)^2, as powers are multiplied

                var subPow = PowFast(x, y / 2, ref multiplications);
                multiplications++;
                return subPow * subPow;
            }
            else
            {
                multiplications++;
                return x * PowFast(x, y - 1, ref multiplications);
            }
        }

        private static long PowNaive(long x, int y)
        {
            var result = 1L;

            for (int i = 0; i < y; i++)
            {
                result *= x;
            }

            return result;
        }
    }
}
