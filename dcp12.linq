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
	var x = new[] { 1, 2 };
	
	(CountOptions(40, x) == g(40)).Dump();
	(CountOptions(50, x) == g(50)).Dump();
	(CountOptions(1000, x) == g(1000)).Dump();
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

public long g(int n, Dictionary<long, long> cache = null)
{
	// dynamic programming based, do not solve solved task over and over again, cache the results!

	if (cache == null)
		cache = new Dictionary<long, long>();

	if (cache.ContainsKey(n))
		return cache[n];

	int[] possible = new int[] { 1, 2, };

	long ways = 0;

	foreach (var step in possible)
	{
		if (step > n)
			continue;

		if (step == n)
		{
			ways += 1;
			continue;
		}

		// recoursion dive
		ways += g(n - step, cache);
	}

	cache[n] = ways;

	return ways;
}

// Define other methods and classes here
