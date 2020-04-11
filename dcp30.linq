<Query Kind="Program" />

/*
This problem was asked by Facebook.

You are given an array of non-negative integers that represents a two-dimensional elevation map where each element is unit-width wall and the integer is the height. 
Suppose it will rain and all spots between two walls get filled up.

Compute how many units of water remain trapped on the map in O(N) time and O(1) space.

For example, given the input [2, 1, 2], we can hold 1 unit of water in the middle.

Given the input [3, 0, 1, 3, 0, 5], we can hold 3 units in the first index, 2 in the second, and 3 in the fourth index (we cannot hold 5 since it would run off to the left), so we can trap 8 units of water.
*/

void Main()
{
	Solve(new[] { 5, 4, 2, 1, 1, 2, 2 }).Dump("Should be 2");
	Solve(new[] { 4, 2, 1, 1, 2, 2 }).Dump("Should be 2");
	Solve(new[] { 4, 2, 1, 1, 2, 2, 1, 5 }).Dump("Should be 15");
	Solve(new[] { 4, 2, 1, 1, 2, 2, 1, 5, 4, 2, 1, 1, 2, 2, 1, 5 }).Dump("Should be 37");
	Solve(new[] { 0, 1, 2, 3, 0 }).Dump("Should be 0");
	Solve(new[] { 4, 2, 1, 1, 2, 2, 1, 5, 6, 7, 10 }).Dump("Should be 15");
}

long Solve(int[] h)
{
	var leftIx = 0;
	var rightIx = h.Length - 1;
	var water = 0L;
	var direction = 0;
	
	while (leftIx < rightIx)
	{
		var lastSuccess = false;
		
		if (direction == 0)
		{	
			for (var i = leftIx + 1; i <= rightIx; i++)
			{
				if (h[i] >= h[leftIx])
				{
					water += GetWater(h, leftIx, i);
					leftIx = i;
					lastSuccess = true;
					break;
				}
			}
		}
		else
		{
			for (var i = rightIx - 1; i >= leftIx; i--)
			{
				if (h[i] >= h[rightIx])
				{
					water += GetWater(h, i, rightIx);
					rightIx = i;
					lastSuccess = true;
					break;
				}
			}
		}
		
		if (!lastSuccess)
		{
			// change traversal direction if the last pass did not work
			direction = 1 - direction;
		}
	}
	
	return water;
}

long GetWater(int[] h, int fromIx, int toIx)
{
	var water = 0L;
	var level = Math.Min(h[fromIx], h[toIx]);
	
	for (var i = fromIx + 1; i <= toIx - 1; i++)
	{
		water += level - h[i];
	}
	
	return water;
}