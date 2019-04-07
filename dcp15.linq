<Query Kind="Program" />

/*
Given a stream of elements too large to store in memory, pick a random element from the stream with uniform probability.
*/

void Main()
{
	var n = 1000;
	var results = new int[n];
	var numOfRuns = 1000000;
	
	for (var i = 0; i < numOfRuns; ++i)
	{
		var res = GetRandomFromInfiniteStream(n);
		results[res]++;
	}

	results.Select((count, index) => new { index, count }).Dump(true);
}

int GetRandomFromInfiniteStream(int n)
{
	int currentlySelected = 0;

	var stream = GetStream(n);
	var index = 0;
	foreach (var item in stream)
	{
		currentlySelected = ProcessItem(index, item, currentlySelected);
		++index;
	}
	
	return currentlySelected;	
}

Random _rand = new Random();

int ProcessItem(int index, int item, int currentlySelected)
{
	// the correctness of the algorigthm can be proven by induction
	
	// adding 1 to index, since Next requires the _exclusive_ upper bound
	if (_rand.Next(0, index + 1) == 0)
	{
		// with probability 1/index we replace the currently selected item
		return item;
	}
	return currentlySelected;
}

IEnumerable<int> GetStream(int n)
{
	return Enumerable.Range(0, n);
}
