using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcp
{
    /*
     This problem was asked by Microsoft.

     Given a 2D matrix of characters and a target word, write a function that returns whether the word can be found 
     in the matrix by going left-to-right, or up-to-down.
     
     For example, given the following matrix:
     
     [['F', 'A', 'C', 'I'],
      ['O', 'B', 'Q', 'P'],
      ['A', 'N', 'O', 'B'],
      ['M', 'A', 'S', 'S']]
     
     and the target word 'FOAM', you should return true, since it's the leftmost column. Similarly, given the target 
     word 'MASS', you should return true, since it's the last row.
     */

    public static class Dcp63
    {
        public static void Test()
        {
            var matrix = new char[,]
            {
                {'F', 'A', 'C', 'I'},
                {'O', 'B', 'Q', 'P'},
                {'A', 'N', 'O', 'B'},
                {'M', 'A', 'S', 'S'}
            };
            Console.WriteLine("Find FOAM: {0}", Solve(matrix, "FOAM"));
            Console.WriteLine("Find MASS: {0}", Solve(matrix, "MASS"));
            Console.WriteLine("Find CQOS: {0}", Solve(matrix, "CQOS"));
            Console.WriteLine("Find MAX: {0}", Solve(matrix, "MAX"));
            Console.WriteLine("Find FOA: {0}", Solve(matrix, "FOA"));
            Console.WriteLine("Find FOMA: {0}", Solve(matrix, "FOMA"));
        }

        public static bool Solve(char[,] m, string word)
        {
            var resultUgly = SolveUgly(m, word);
            var resultBeautiful = SolveBeautiful(m, word);

            if (resultUgly != resultBeautiful)
                throw new Exception("OOPS");

            return resultBeautiful;
        }

        private static bool SolveBeautiful(char[,] m, string word)
        {
            var rows = m.GetLength(0);
            var columns = m.GetLength(1);

            if (word.Length == rows)
            {
                if (EnumerateRows(m).Any(row => MatchWord(row, word)))
                    return true;
            }

            if (word.Length == columns)
            {
                if (EnumerateColumns(m).Any(column => MatchWord(column, word)))
                    return true;
            }

            return false;
        }

        private static bool MatchWord(IEnumerable<char> sequence, string targetWord)
        {
            var sequenceIt = sequence.GetEnumerator();

            foreach (var targetChar in targetWord)
            {
                if (!sequenceIt.MoveNext()) // target word contains more characters than the sequence
                    return false;

                if (sequenceIt.Current != targetChar)
                    return false;
            }

            if (sequenceIt.MoveNext())  // sequence contains more characters than the target word
                return false;

            return true;
        }

        private static IEnumerable<IEnumerable<char>> EnumerateColumns(char[,] m)
        {
            var columns = m.GetLength(1);

            for (int j = 0; j < columns; j++)
            {
                yield return EnumerateSingleColumn(m, j);
            }
        }

        private static IEnumerable<char> EnumerateSingleColumn(char[,] m, int columnIx)
        {
            var rows = m.GetLength(0);

            for (int i = 0; i < rows; i++)
            {
                yield return m[i, columnIx];
            }
        }

        private static IEnumerable<IEnumerable<char>> EnumerateRows(char[,] m)
        {
            var rows = m.GetLength(0);

            for (int i = 0; i < rows; i++)
            {
                yield return EnumerateSingleRow(m, i);
            }
        }

        private static IEnumerable<char> EnumerateSingleRow(char[,] m, int rowIx)
        {
            var columns = m.GetLength(1);

            for (int j = 0; j < columns; j++)
            {
                yield return m[rowIx, j];
            }
        }

        private static bool SolveUgly(char[,] m, string word)
        {
            var rows = m.GetLength(0);
            var columns = m.GetLength(1);

            if (word.Length == rows)
            {
                for (int i = 0; i < rows; i++)
                {
                    var found = true;
                    for (int j = 0; j < columns; j++)
                    {
                        if (m[i, j] != word[j])
                        {
                            found = false;
                            break;
                        }
                    }
                    if (found)
                        return true;
                }
            }
            
            if (word.Length == columns)
            {
                for (int j = 0; j < columns; j++)
                {
                    var found = true;
                    for (int i = 0; i < rows; i++)
                    {
                        if (m[i, j] != word[i])
                        {
                            found = false;
                            break;
                        }
                    }
                    if (found)
                        return true;
                }
            }

            return false;
        }
    }
}
