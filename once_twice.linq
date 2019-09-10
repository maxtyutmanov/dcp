<Query Kind="Program" />

// This problem was asked by Facebook.
// 
// Given an array of integers in which two elements appear exactly
// once and all other elements appear exactly twice, find the two
// elements that appear only once.
// 
// For example, given the array [2, 4, 6, 8, 10, 2, 6, 10], return
// 4 and 8. The order does not matter.
// 
// Follow-up: Can you do this in linear time and constant space?

static Random _generator = new Random();

void Main()
{
	for (int i = 0; i < 100; i++)
	{
		var (a, xInit, yInit) = GenerateInput(100);
		//var (xInit, yInit) = (2004950486, 1929294214);
		//var a = new[] { xInit, yInit };
		var (x, y) = Solve(a);

		if (x == xInit && y == yInit ||
			y == xInit && x == yInit)
		{
			//Console.WriteLine("Oh, nice");
		}
		else
		{
			Console.WriteLine("Not nice!");
			x.Dump("Actual X");
			y.Dump("Actual Y");
			xInit.Dump("Expected X");
			yInit.Dump("Expected Y");
			a.Dump("Input array");
			return;
		}
	}
	
	Console.WriteLine("Oh, nice");
}

(int[] a, int x, int y) GenerateInput(int size)
{
	var a = new int[size * 2 + 2];
	for (int i = 0; i < size; i++)
	{
		var num = _generator.Next();
		a[i] = num;
		a[size + i] = num; 
	}
	var x = _generator.Next();
	var y = _generator.Next();
	
	a[size * 2] = x;
	a[size * 2 + 1] = y;
	
	Shuffle(a);
	
	return (a, x, y);
}

void Shuffle(int[] a)
{
	for (int i = 0; i < a.Length; i++)
	{
		var ix1 = i;
		var ix2 = _generator.Next(a.Length);
		
		var tmp = a[ix1];
		a[ix1] = a[ix2];
		a[ix2] = tmp;
	}
}

(int i1, int i2) Solve(int[] a)
{
	var totalXor = a.Aggregate((res, cur) => res ^ cur);
	// find the first non-zero bit in the total XOR (which is the same as first non-matching bit for two different numbers)
	uint mask = 1U;
	for (var i = 0; i < sizeof(int) * 8; ++i)
	{
		mask = 1U << i;
		if ((totalXor & mask) != 0)
		{
			// i-th bit of total xor is non-zero, that's interesting
			break;
		}
	}

	var leftXor = totalXor;
	var rightXor = 0;

	// divide all numbers into two categories: numbers that have i-th bit 1 (left) and numbers that have i-th bit zero (right)
	// of course if there are two equal numbers they go to the same category

	foreach (var number in a)
	{
		if ((number & mask) == 0)
		{
			// goes to the right category ("subtracted" from left and "added" to right)
			leftXor ^= number;
			rightXor ^= number;
		}
	}
	
	return (leftXor, rightXor);
}

// Define other methods and classes here