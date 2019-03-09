<Query Kind="Program" />

/*
There exists a staircase with N steps, and you can climb up either 1 or 2 steps at a time. Given N, write a function that returns the number of unique ways you can climb the staircase. The order of the steps matters.

For example, if N is 4, then there are 5 unique ways:

1, 1, 1, 1
2, 1, 1
1, 2, 1
1, 1, 2
2, 2
What if, instead of being able to climb 1 or 2 steps at a time, you could climb any number from a set of positive integers X? For example, if X = {1, 3, 5}, you could climb 1, 3, or 5 steps at a time.
*/

void Main()
{
	CountOptions(100, new[] { 1, 3, 5 }).Dump();
}

long CountOptions(int n, int[] x)
{
	return CountOptions(n, x, new Dictionary<int, long>());
}

long CountOptions(int n, int[] x, Dictionary<int, long> cache)
{
	if (cache.TryGetValue(n, out var cachedCnt))
	{
		return cachedCnt;
	}
	
	if (n < 0)
	{
		return 0;
	}
	
	if (n == 0)
	{
		return 1;
	}
	
	var cnt = 0L;
	
	foreach (var stepSize in x)
	{
		cnt += CountOptions(n - stepSize, x, cache);
	}
	
	cache[n] = cnt;
	
	return cnt;
}

// Define other methods and classes here
