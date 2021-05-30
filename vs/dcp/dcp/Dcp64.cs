using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace dcp
{
    /*
     This problem was asked by Google.

     A knight's tour is a sequence of moves by a knight on a chessboard such that all squares are visited once.
     
     Given N, write a function to return the number of knight's tours on an N by N chessboard.
     */

    public static class Dcp64
    {
        public static void Test()
        {
            var sw = Stopwatch.StartNew();
            Console.WriteLine("Knight tours for n=5: {0}, elapsed time: {1}", Solve(5), sw.Elapsed);
            sw.Restart();
            Console.WriteLine("Check solution existence for n=8: {0}, elapsed time: {1}", SolutionExists(8), sw.Elapsed);
        }

        public static long Solve(int n)
        {
            return SolveWithBacktracking(n);
        }

        private static long SolveWithBacktracking(int n)
        {
            // finding ALL possible tours takes LOTS of time... more than a minute for n=5
            var board = GetAllCellsOnBoard(n);
            var taken = new bool[n, n];
            return board.Select(cell => SolveWithBacktracking(cell, taken, 1, n, false)).Sum();
        }

        private static bool SolutionExists(int n)
        {
            var taken = new bool[n, n];
            return SolveWithBacktracking((0, 0), taken, 1, n, true) > 0;
        }

        private static long SolveWithBacktracking((int i, int j) start, bool[,] taken, int cellsWalked, int n, bool onlyFirst)
        {
            if (cellsWalked == n * n)
                return 1;

            var numOptions = 0L;

            taken[start.i, start.j] = true;
            // order next cells using Warnsdorff's rule
            var nextCells = GetPossibleNextCells(start, n, taken).OrderBy(cell => GetPossibleNextCells(cell, n, null).Count());
            foreach (var cell in nextCells)
            {
                numOptions += SolveWithBacktracking(cell, taken, cellsWalked + 1, n, onlyFirst);
                if (onlyFirst && numOptions > 0)
                    break;
            }
            taken[start.i, start.j] = false;

            return numOptions;
        }

        private static IEnumerable<(int i, int j)> GetPossibleNextCells((int i, int j) start, int n, bool[,] taken)
        {
            return _moves
                .Select(move => new { i = start.i + move.di, j = start.j + move.dj })
                .Where(x => 0 <= x.i && x.i < n && 0 <= x.j && x.j < n)
                .Where(x => taken == null || !taken[x.i, x.j])
                .Select(x => (x.i, x.j));
        }

        private static IEnumerable<(int i, int j)> GetAllCellsOnBoard(int n)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    yield return (i, j);
                }
            }
        }

        private static readonly List<(int di, int dj)> _moves = new List<(int di, int dj)>()
        {
            (2, 1), (1, 2), (-1, 2), (-2, 1), (-2, -1), (-1, -2), (1, -2), (2, -1)
        };
    }
}
