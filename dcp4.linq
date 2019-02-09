<Query Kind="Program" />

void Main()
{
	var a = Enumerable.Range(1, 10000).Select(x => x % 5 == 0 ? -x : x).Where(x => x != 1000).Reverse().ToArray();
	//var a = GenerateInput(10000).ToArray();
	
	//var a = new[] { 15, 20, 23 };
	
	var result = Solve(a);
	
	//a.Dump();
	result.Dump();
	IterationsCount.Dump();
}

Random Generator = new Random();

IEnumerable<int> GenerateInput(int n)
{
	for (int i = 0; i < n; i++)
	{
		yield return Generator.Next(0, n);
	}
}

int IterationsCount = 0;

int Solve(int[] a)
{
	for (int i = 0; i < a.Length; i++)
	{
		// on each iteration of the internal cycle 1 positive integer is put in the right place,
		// others (non-positive or too large) are thrown away (zeroed)
		while (true)
		{
			IterationsCount++;
			
			var isInRightPlace = (a[i] == i + 1);

			if (isInRightPlace)
				break;

			if (a[i] > a.Length || a[i] < 1)
			{
				// not interested in negative or too large numbers
				a[i] = 0;
				break;
			}
				
			var rightPlace = a[i] - 1;
			
			if (a[rightPlace] == a[i])
			{
				// not interested in duplicates
				a[i] = 0;
				break;
			}
				
			Swap(a, i, rightPlace);
		}
	}
	
	return a.TakeWhile((x, i) => x == i + 1).LastOrDefault() + 1;
}

void Swap(int[] a, int i, int j)
{	
	var tmp = a[i];
	a[i] = a[j];
	a[j] = tmp;
}

// Define other methods and classes here
