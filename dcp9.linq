<Query Kind="Program" />

/*
Given a list of integers, write a function that returns the largest sum of non-adjacent numbers. Numbers can be 0 or negative.

For example, [2, 4, 6, 2, 5] should return 13, since we pick 2, 6, and 5. [5, 1, 1, 5] should return 10, since we pick 5 and 5.

Follow-up: Can you do this in O(N) time and constant space?
*/

void Main()
{
	GetMaxSum(new[] { -1, -1, 7, -1, -1 }).Dump();
}

int GetMaxSum(int[] a)
{
	return GetMaxSum(a, 0).currentSum;
}

(int currentSum, int oneStepAheadSum) GetMaxSum(int[] a, int i)
{	
	if (i == a.Length - 1) 
		return (Math.Max(a[i], 0), 0);
	
	var (oneStepAheadSum, twoStepsAheadSum) = GetMaxSum(a, i + 1);

	// compare two options: we either use oneStepAheadSum (and skip i-th item) 
	// or use twoStepsAheadSum but in this case we can include i-th item (because it's not adjacent)
	var maxSum = Math.Max(oneStepAheadSum, a[i] + twoStepsAheadSum);
	
	// but we're not obliged to include i-th item anyway
	
	return (maxSum, oneStepAheadSum);
}