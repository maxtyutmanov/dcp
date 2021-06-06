using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcp
{
    /*
    This problem was asked by Microsoft.

    A number is considered perfect if its digits sum up to exactly 10.

    Given a positive integer n, return the n-th perfect number.

    For example, given 1, you should return 19. Given 2, you should return 28.
     */

    public static class Dcp70
    {
        public static void Test()
        {
            Console.WriteLine("for n = 1: {0}", Solve(1));
            Console.WriteLine("for n = 10: {0}", Solve(10));
            Console.WriteLine("for n = 100: {0}", Solve(100));
            Console.WriteLine("for n = 1000: {0}", Solve(1000));
            Console.WriteLine("for n = 100000: {0}", Solve(100000));
            Console.WriteLine("for n = 1000000: {0}", Solve(1000000));
        }

        public static long Solve(int n)
        {
            //var nSmarter = GetPerfectNumbersSlow().Skip(n - 1).First();
            //var nStupid = GetPerfectNumbersFaster().Skip(n - 1).First();
            var nSmart = GetNthPerfectNumber(n);

            //if (nSmart != nStupid)
            //    throw new Exception("oops");

            return nSmart;
        }

        /// <summary>
        /// it seems any perfect number minus 19 is divisible by 9, so let's use the increments of 9
        /// </summary>
        private static IEnumerable<long> GetPerfectNumbersFaster()
        {
            var number = 19L;
            yield return number;

            while (true)
            {
                // by adding 9 we subtract 1 from the least significant bit and add it to the 2nd significant bit
                number += 9;
                if (IsPerfect(number))
                    yield return number;
            }
        }

        /// <summary>
        /// the most naive approach: just enumerate all natural numbers and check each number
        /// if it's perfect
        /// </summary>
        private static IEnumerable<long> GetPerfectNumbersSlow()
        {
            var number = 19L;
            yield return number;

            while (true)
            {
                number++;
                if (IsPerfect(number))
                    yield return number;
            }
        }

        private static bool IsPerfect(long number)
        {
            var sumDigits = 0L;
            while (number != 0)
            {
                sumDigits += number % 10;
                number /= 10;
            }
            return sumDigits == 10;
        }

        private static long GetNthPerfectNumber(int n)
        {
            var i = 0;
            var decimalBits = 2;
            byte[] buffer = null;

            while (i < n)
            {
                // enumerating all perfect numbers having <decimalBits> decimal bits
                buffer = new byte[decimalBits];
                var segment = new ArraySegment<byte>(buffer);
                foreach (var _ in GenAllNumbersWithGivenDigitSum(segment, 10, false))
                {
                    if (++i == n)
                    {
                        break;
                    }
                }
                decimalBits++;
            }

            return GetNumberFromDecBits(buffer);
        }

        private static IEnumerable<ArraySegment<byte>> GenAllNumbersWithGivenDigitSum(
            ArraySegment<byte> segment, byte targetSum, bool allowLeadingZero)
        {
            // recursively enumerates all possible numbers digits of which sum up to <targetSum>

            if (segment.Count == 0)
            {
                if (targetSum == 0)
                    yield return segment;
                yield break;
            }

            if (targetSum == 0)
            {
                for (int i = 0; i < segment.Count; i++)
                {
                    segment[i] = 0;
                }
                yield return segment;
            }
            else
            {
                var subSegment = segment.Count > 1 
                    ? segment.Slice(1) 
                    : new ArraySegment<byte>();

                for (byte i = (allowLeadingZero ? (byte)0 : (byte)1); i <= Math.Min(targetSum, (byte)9); i++)
                {
                    segment[0] = i;
                    foreach (var _ in GenAllNumbersWithGivenDigitSum(subSegment, (byte)(targetSum - i), true))
                    {
                        yield return segment;
                    }
                }
            }
        }

        private static long GetNumberFromDecBits(byte[] bits)
        {
            return long.Parse(string.Join("", bits));
        }
    }
}
