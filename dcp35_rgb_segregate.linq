<Query Kind="Program" />

/*
This problem was asked by Google.

Given an array of strictly the characters 'R', 'G', and 'B', segregate the values of the array so that all the Rs come first, the Gs come second, and the Bs come last. You can only swap elements of the array.

Do this in linear time and in-place.

For example, given the array ['G', 'B', 'R', 'R', 'B', 'R', 'G'], it should become ['R', 'R', 'R', 'G', 'G', 'B', 'B'].
*/

void Main()
{
	Solve(new[] { 'G', 'B', 'R', 'R', 'B', 'R', 'G' }).Dump("should be RRRGGBB");
	Solve(new char[] { }).Dump("should be empty");
	Solve(new[] { 'B', 'G' }).Dump("should be GB");
	Solve(new[] { 'R', 'R', 'R' }).Dump("should be RRR");
}

string Solve(char[] a)
{
	var i = Compact(a, 'R', 0);
	i = Compact(a, 'G', i);
	i = Compact(a, 'B', i);
	return new string(a);
}

int Compact(char[] a, char c, int start)
{
	var i = start;
	for (int j = i; j < a.Length; j++)
	{
		if (a[j] == c)
		{
			Swap(a, i, j);
			i++;
		}
	}
	return i;
}

void Swap(char[] a, int i, int j)
{
	var tmp = a[i];
	a[i] = a[j];
	a[j] = tmp;
}
