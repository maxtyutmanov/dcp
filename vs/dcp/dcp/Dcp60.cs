using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcp
{
    /*
    Given a multiset of integers, return whether it can be partitioned into two subsets whose sums are the same.

    For example, given the multiset {15, 5, 20, 10, 35, 15, 10}, it would return true, since we can split it up into {15, 5, 10, 15, 10} and {20, 35}, which both add up to 55.

    Given the multiset {15, 5, 20, 10, 35}, it would return false, since we can't split it up into two subsets that add up to the same sum.
     */

    public static class Dcp60
    {
        public static bool Solve(int[] nums)
        {
            // assuming all items are positive

            var totalSum = nums.Sum(x => (long)x);
            if (totalSum % 2 != 0)
                return false;

            var targetSum = totalSum / 2;

            return SolveWithSieve(nums, targetSum);

            //return SolveWithDp(nums, targetSum);
            //return HasSubsetWithSum(nums, nums.Length - 1, targetSum, new Dictionary<(int ix, long subsum), bool>());
        }

        private static bool SolveWithSieve(int[] items, long targetSum)
        {
            // similar to sieve of eratosthenes

            var possibleSums = new HashSet<long>();

            foreach (var item in items)
            {
                IEnumerable<long> withItemIncluded;
                if (possibleSums.Any())
                    withItemIncluded = possibleSums.Select(x => x + item).ToList();
                else
                    withItemIncluded = new[] { (long)item };

                foreach (var newSum in withItemIncluded)
                {
                    if (newSum == targetSum)
                        return true;

                    possibleSums.Add(newSum);
                }
            }

            return false;
        }

        private static bool HasSubsetWithSum(int[] items, int itemIx, long targetSum, Dictionary<(int ix, long subsum), bool> cache)
        {
            // recursively solve with memoization

            if (cache.TryGetValue((itemIx, targetSum), out var cached))
                return cached;

            // naive approach: brute force

            if (itemIx < 0)
                return false;

            if (targetSum == 0)
                return true;

            if (targetSum < 0)
                return false;

            var result =
                HasSubsetWithSum(items, itemIx - 1, targetSum - items[itemIx], cache) ||   // try with this item included
                HasSubsetWithSum(items, itemIx - 1, targetSum, cache); // try without this item included

            cache[(itemIx, targetSum)] = result;
            return result;
        }

        private static bool SolveWithDp(int[] items, long targetSum)
        {
            // dynamic programming

            var cache = new bool[items.Length, targetSum + 1];

            for (var s = 0; s <= targetSum; s++)
            {
                cache[0, s] = items[0] == s;
            }

            for (var i = 1; i < items.Length; i++)
            {
                var startS = 0L;
                if (i == items.Length - 1)
                    startS = targetSum;

                for (var s = startS; s <= targetSum; s++)
                {
                    cache[i, s] = items[i] == s || HasSubsum(i - 1, s) || HasSubsum(i - 1, s - items[i]);
                }
            }

            return cache[items.Length - 1, targetSum];

            bool HasSubsum(int ix, long subsum)
            {
                if (subsum < 0)
                    return false;

                return cache[ix, subsum];
            }
        }
    }
}
