using System;
using System.Collections.Generic;
using System.Text;

namespace dcp
{
    /*
     This problem was asked by Two Sigma.

     Using a function rand7() that returns an integer from 1 to 7 (inclusive) with uniform probability, 
     implement a function rand5() that returns an integer from 1 to 5 (inclusive).
     */

    public static class Dcp71
    {
        private static readonly Random _rng = new Random();

        public static void Test()
        {
            var results = new int[6];
            for (var i = 0; i < 100000; i++)
            {
                results[Rand5()]++;
            }

            for (int i = 1; i < results.Length; i++)
            {
                Console.WriteLine("{0} -> {1}", i, results[i]);
            }
        }

        public static int Rand5()
        {
            var r = Rand7();
            if (r <= 5)
                return r;
            else
                return Rand5();
        }

        private static int Rand7()
        {
            return _rng.Next(1, 8);
        }
    }
}
