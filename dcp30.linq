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
	Solve(new[] { 4, 2, 1, 1, 2, 2 }).Dump("Should be 2");
	Solve(new[] { 4, 2, 1, 1, 2, 2, 1, 5 }).Dump("Should be 15");
	Solve(new[] { 4, 2, 1, 1, 2, 2, 1, 5, 4, 2, 1, 1, 2, 2, 1, 5 }).Dump("Should be 37");
}

long Solve(int[] h)
{
	Pit prevPit = null;
	var totalWater = 0L;
	
	foreach (var pit in GetAllPits(h))
	{
		if (prevPit == null)
		{
			prevPit = pit;
		}
		else
		{
			prevPit = pit.Merge(prevPit);
		}
		
		if (prevPit.RightH >= prevPit.LeftH)
		{
			totalWater += prevPit.Water;
			prevPit = null;
		}
	}
	
	totalWater += (prevPit?.Water ?? 0);
	return totalWater;
}

IEnumerable<Pit> GetAllPits(int[] h)
{
	var i = 0;
	while (i < h.Length)
	{
		var p = new Pit(new HMap(h, i));
		yield return p;
		i += (p.Width + 1);
	}
}

class Pit
{
	public int LeftH { get; }
	public int RightH { get; }
	public int Width { get; }
	public long BlocksBetween { get; }
	public long Water { get; }
	
	public Pit(int leftH, int rightH, int width = 0, long blocksBetween = 0, long water = 0)
	{
		LeftH = leftH;
		RightH = rightH;
		Width = width;
		BlocksBetween = blocksBetween;
		Water = water;
	}
	
	public Pit(HMap hMap)
	{
		var i = 1;
		while (i < hMap.Length)
		{
			if (hMap.IsPeak(i))
			{
				Width = i - 1;
				break;
			}
			BlocksBetween += hMap[i];
			i++;
		}
		LeftH = hMap[0];
		RightH = hMap[i];

		if (Width >= 1)
		{
			Water = Math.Min(LeftH, RightH) * Width - BlocksBetween;
		}
	}

	public Pit Merge(Pit prev)
	{
		var left = prev.LeftH;
		var center = LeftH;
		var right = RightH;
		
		if (left >= center && center >= right)
		{
			/*
				|
				|xx|
				|xx|xx|
			*/
			
			return new Pit(left, right, prev.Width + Width + 1, prev.BlocksBetween + BlocksBetween + center, prev.Water + Water);
		}
		else if (left >= right && right >= center || right >= left && left >= center)
		{
			/*
				|
				|XXXXX|
				|xx|xx|
			*/

			// OR

			/*
				      |
				|XXXXX|
				|xx|xx|
			*/

			var bb = prev.BlocksBetween + BlocksBetween + center;
			var wi = prev.Width + Width + 1;
			var lev = Math.Min(left, right);
			
			return new Pit(left, right, wi, bb, wi * lev - bb);
		}
		else
		{
			prev.Dump();
			this.Dump();
			throw new Exception("Something strange!");
		}
	}
}

class HMap
{
	private readonly int[] map;
	private readonly int startIx;
	
	public HMap(int[] map, int startIx = 0)
	{
		this.map = map;
		this.startIx = startIx;
	}
	
	public int Length => map.Length - startIx;
	
	public int this[int ix]
	{
		get
		{
			var rix = startIx + ix;
			
			if (rix < 0 || rix >= map.Length) return 0;
			return map[rix];
		}
	}
	
	public HMap Submap(int startIx)
	{
		return new HMap(map, startIx);
	}
	
	public bool IsPeak(int i) => this[i - 1] <= this[i] && this[i] > this[i + 1];
}