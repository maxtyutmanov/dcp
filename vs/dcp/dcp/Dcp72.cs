using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

/*
This problem was asked by Google.

In a directed graph, each node is assigned an uppercase letter. We define a path's value as the number of most frequently-occurring letter along that path. 
For example, if a path in the graph goes through "ABACA", the value of the path is 3, since there are 3 occurrences of 'A' on the path.

Given a graph with n nodes and m directed edges, return the largest value path of the graph. If the largest value is infinite, then return null.

The graph is represented with a string and an edge list. The i-th character represents the uppercase letter of the i-th node. 
Each tuple in the edge list (i, j) means there is a directed edge from the i-th node to the j-th node. Self-edges are possible, as well as multi-edges.

For example, the following input graph:

ABACA
[(0, 1),
 (0, 2),
 (2, 3),
 (3, 4)]
Would have maximum value 3 using the path of vertices [0, 2, 3, 4], (A, A, C, A).

The following input graph:

A
[(0, 0)]
Should return null, since we have an infinite loop.
 */

namespace dcp
{
    public static class Dcp72
    {
        public static void Test()
        {
            var pathLabels = "ABACA".ToArray();
            var edges = new []
            {
                (0, 1),
                (0, 2),
                (2, 3),
                (3, 4)
            };

            Console.WriteLine("Max value for graph 1: {0}", Solve(edges, pathLabels));

            edges = new[]
            {
                (0, 1),
                (0, 2),
                (2, 3),
                (3, 4),
                (4, 0)
            };

            Console.WriteLine("Max value for graph 2: {0}", Solve(edges, pathLabels));

            pathLabels = "A".ToArray();
            edges = new[]
            {
                (0, 0)
            };

            Console.WriteLine("Max value for graph 3: {0}", Solve(edges, pathLabels));
        }

        public static int? Solve(IReadOnlyList<(int, int)> edgesList, char[] labels)
        {
            var paths = edgesList
                .Select(x => EdgeToPathDescriptor(x.Item1, x.Item2, labels))
                .ToList();

            paths = MergePaths(paths, labels);

            if (paths.Any(x => x.IsCycle))
                return null;

            return paths.Max(x => x.GetValue());
        }

        private static List<PathDescriptor> MergePaths(List<PathDescriptor> paths, char[] labels)
        {
            // try to merge each pair of paths

            while (true)
            {
                if (paths.Any(x => x.IsCycle))
                    return paths; // no need to merge anything anymore, a cycle is found

                var countBefore = paths.Count;

                for (int i = 0; i < paths.Count; i++)
                {
                    for (int j = i + 1; j < paths.Count; j++)
                    {
                        var mergedPath = PathDescriptor.TryMergeTwoPaths(paths[i], paths[j], labels);
                        if (mergedPath != null)
                        {
                            paths[i] = mergedPath;
                            paths.RemoveAt(j);
                        }
                    }
                }

                var countAfter = paths.Count;

                if (countAfter == countBefore)
                    return paths;
            }
        }

        private static PathDescriptor EdgeToPathDescriptor(int fromIx, int toIx, char[] pathLabels)
        {
            var fromLabel = pathLabels[fromIx];
            var toLabel = pathLabels[toIx];

            Dictionary<char, int> freqs;

            if (fromLabel != toLabel || fromIx == toIx)
            {
                freqs = new Dictionary<char, int>
                {
                    [fromLabel] = 1,
                    [toLabel] = 1
                };
            }
            else
            {
                freqs = new Dictionary<char, int>
                {
                    [fromLabel] = 2,
                };
            }

            return new PathDescriptor(freqs, fromIx, toIx);
        }

        [DebuggerDisplay("{FromIx} - {ToIx}")]
        private class PathDescriptor
        {
            public Dictionary<char, int> LabelFreqs { get; }

            public int FromIx { get; }

            public int ToIx { get; }

            public bool IsCycle => FromIx == ToIx;

            public int GetValue()
            {
                if (LabelFreqs.Count != 0)
                    return LabelFreqs.Values.Max();

                return 0;
            }

            public PathDescriptor(Dictionary<char, int> labelFreqs, int fromIx, int toIx)
            {
                LabelFreqs = labelFreqs;
                FromIx = fromIx;
                ToIx = toIx;
            }

            public static PathDescriptor TryMergeTwoPaths(PathDescriptor a, PathDescriptor b, char[] labels)
            {
                if (a.ToIx == b.FromIx)
                    return new PathDescriptor(MergeFreqDicts(a.LabelFreqs, b.LabelFreqs, labels[a.ToIx]), a.FromIx, b.ToIx);

                return null;
            }

            private static Dictionary<char, int> MergeFreqDicts(Dictionary<char, int> a, Dictionary<char, int> b, char jointChar)
            {
                var result = new Dictionary<char, int>(a);

                foreach (var (c, freq) in b)
                {
                    result.TryGetValue(c, out var existingFreq);
                    result[c] = existingFreq + freq;
                }

                result[jointChar]--;

                return result;
            }
        }
    }
}
