using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace dcp
{
    /*
    This problem was asked by Google.

    On our special chessboard, two bishops attack each other if they share the same diagonal. 
    This includes bishops that have another bishop located between them, i.e. bishops can attack through pieces.

    You are given N bishops, represented as (row, column) tuples on a M by M chessboard. Write a function 
    to count the number of pairs of bishops that attack each other. The ordering of the pair doesn't matter: 
    (1, 2) is considered the same as (2, 1).

    For example, given M = 5 and the list of bishops:

    (0, 0)
    (1, 2)
    (2, 2)
    (4, 0)
    The board would look like this:

    [b 0 0 0 0]
    [0 0 b 0 0]
    [0 0 b 0 0]
    [0 0 0 0 0]
    [b 0 0 0 0]
    You should return 2, since bishops 1 and 3 attack each other, as well as bishops 3 and 4.
     */

    public static class Dcp68
    {
        public static void Test()
        {
            var bishops = new List<(int i, int j)>()
            {
                (0, 0),
                (1, 2),
                (2, 2),
                (4, 0)
            };

            Console.WriteLine("Num of collisions: {0}", Solve(5, bishops));

            bishops.Add((1, 1));
            Console.WriteLine("Num of collisions: {0}", Solve(5, bishops));

            bishops.Add((1, 4));
            Console.WriteLine("Num of collisions: {0}", Solve(5, bishops));
        }

        public static int Solve(int m, List<(int i, int j)> bishops)
        {
            // notice we don't even need m (the size of the board) since we can easily determine 
            // if two bishops are on the same diagonal just by looking at their coordinates

            var totalCollisionsCount = 0;
            foreach (var (bi, bj) in bishops)
            {
                totalCollisionsCount += GetCollisionsCount(bi, bj, bishops);
            }
            return totalCollisionsCount;
        }

        private static int GetCollisionsCount(int si, int sj, IEnumerable<(int i, int j)> bishops)
        {
            return bishops.Count(x => SameDiagonal(si, sj, x.i, x.j));
        }

        private static bool SameDiagonal(int i1, int j1, int i2, int j2)
        {
            /* check this kind of diagonal:
             * x
             *  x
             *   x
             *    x
             * i, j - both increase, (i - j) == const
             */
            if (i2 > i1 && j2 > j1 && (i1 - j1) == (i2 - j2))
                return true;

            /* check this kind of diagonal:
             *    x
             *   x
             *  x
             * x
             * i increases, j decreases, (i + j) == const
             */
            if (i2 > i1 && j2 < j1 && (i1 + j1) == (i2 + j2))
                return true;

            // we deliberately don't check for the opposite direction (when e.g. i decreases)
            // to skip duplicates

            return false;
        }
    }
}
