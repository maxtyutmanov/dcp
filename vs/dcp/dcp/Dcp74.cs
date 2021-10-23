using System;
using System.Collections.Generic;
using System.Text;

/*
This problem was asked by Apple.

Suppose you have a multiplication table that is N by N. That is, a 2D array where the value at the i-th row and j-th column is (i + 1) * (j + 1) (if 0-indexed) or i * j (if 1-indexed).

Given integers N and X, write a function that returns the number of times X appears as a value in an N by N multiplication table.

For example, given N = 6 and X = 12, you should return 4, since the multiplication table looks like this:

| 1 | 2 | 3 | 4 | 5 | 6 |

| 2 | 4 | 6 | 8 | 10 | 12 |

| 3 | 6 | 9 | 12 | 15 | 18 |

| 4 | 8 | 12 | 16 | 20 | 24 |

| 5 | 10 | 15 | 20 | 25 | 30 |

| 6 | 12 | 18 | 24 | 30 | 36 |

And there are 4 12's in the table.
 */

namespace dcp
{
    public static class Dcp74
    {
        public static void Test()
        {
            Console.WriteLine("N=6, X=12, result={0} (should be 4)", Solve(6, 12));

            Console.WriteLine("N=6, X=16, result={0} (should be 1)", Solve(6, 16));
        }

        public static int Solve(int n, int x)
        {
            return SolveNaive(n, x);
        }

        private static int SolveNaive(int n, int x)
        {
            // O(n^2) solution

            var minMultiplier = (int)Math.Floor(Math.Sqrt(x));
            var maxMultiplier = x;

            var result = 0;

            for (int i = 1; i <= n; i++)
            {
                for (int j = i + 1; j <= maxMultiplier; j++)
                {
                    if (i * j == x)
                        result++;

                    if (i * j > x)
                        break;
                }
            }

            result *= 2;

            // diagonal
            for (int i = minMultiplier; i <= maxMultiplier; i++)
            {
                if (i * i == x)
                    result++;
            }

            return result;
        }
    }
}
