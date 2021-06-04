using System;
using System.Collections.Generic;
using System.Text;

namespace dcp
{
    /*
    This problem was asked by Square.

    Assume you have access to a function toss_biased() which returns 0 or 1 with a probability 
    that's not 50-50 (but also not 0-100 or 100-0). You do not know the bias of the coin.

    Write a function to simulate an unbiased coin toss.
    */

    public class Dcp66
    {
        private static readonly Random _rng = new Random();

        public static void Test()
        {
            const int totalRuns = 100000;
            var ones = 0;
            for (int i = 0; i < totalRuns; i++)
            {
                ones += TossUnbiased();
            }
            Console.WriteLine("Probability of 1: {0}", ones / (double)totalRuns);
        }

        public static int TossUnbiased()
        {
            /*
             * let the probabilities of biased toss be:
             * P(t)=x
             * P(h)=1-x
             * where t - "tails" (0) and h - "heads" (1)
             * Then P(th)=P(ht)=x(x-1). If double toss produces something different from th and ht,
             * we just toss two more times.
             */

            var (t1, t2) = (TossBiased(), TossBiased());
            if (t1 == 0 && t2 == 1)
                return 0;
            else if (t1 == 1 && t2 == 0)
                return 1;
            else
                return TossUnbiased();
        }

        private static int TossBiased()
        {
            var x = _rng.Next(1, 10);
            return (x > 6) ? 1 : 0;
        }
    }
}
