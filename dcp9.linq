<Query Kind="Program" />

/*
Given a list of integers, write a function that returns the largest sum of non-adjacent numbers. Numbers can be 0 or negative.

For example, [2, 4, 6, 2, 5] should return 13, since we pick 2, 6, and 5. [5, 1, 1, 5] should return 10, since we pick 5 and 5.

Follow-up: Can you do this in O(N) time and constant space?
*/

void Main()
{
	GetMaxSum(new[] { 5, 1, 1, 5 }).Dump();
}

int GetMaxSum(int[] a)
{
	return GetMaxSum(a, 0).currentSum;
}

(int currentSum, int oneStepAheadSum) GetMaxSum(int[] a, int i)
{
	if (i >= a.Length) return (0, 0);
	
	if (i == a.Length - 1) return (a[i], 0);
	
	var (oneStepAheadSum, twoStepsAheadSum) = GetMaxSum(a, i + 1);

	// compare two options: we either use oneStepAheadSum (and skip i-th item) 
	// or use twoStepsAheadSum but in this case we can include i-th item (because it's not adjacent)
	var maxSum = Math.Max(oneStepAheadSum, a[i] + twoStepsAheadSum);
	
	return (maxSum, oneStepAheadSum);
}
