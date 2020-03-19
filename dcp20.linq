<Query Kind="Program">
  <Namespace>System.Runtime.CompilerServices</Namespace>
</Query>

/*
This problem was asked by Google.

Given two singly linked lists that intersect at some point, find the intersecting node. The lists are non-cyclical.

For example, given A = 3 -> 7 -> 8 -> 10 and B = 99 -> 1 -> 8 -> 10, return the node with value 8.

In this example, assume nodes with the same value are the exact same node objects.

Do this in O(M + N) time (where M and N are the lengths of the lists) and constant space.
*/

void Main()
{
	TestCase1_CommonTail_DifferentLengths();
	TestCase2_NoCommonTail();
	TestCase3_CommonTail_SameLengths();
	TestCase4_OneIsSublistOfOther();
}

void TestCase4_OneIsSublistOfOther()
{
	PrintTestCase();
	var ll1 = LLN.Create(6, 5, 4);
	var ll2 = LLN.Create(ll1, 3, 8, 15, 22, -1);

	ll1.Print();
	ll2.Print();

	var res = Solve(ll1, ll2);
	res?.Value.Dump("6");
}

void TestCase3_CommonTail_SameLengths()
{
	PrintTestCase();
	var commonTail = LLN.Create(4, 5, 6);

	var ll1 = LLN.Create(commonTail, 1, 2, 3, 4, 5);
	var ll2 = LLN.Create(commonTail, 3, 8, 15, 22, -1);

	ll1.Print();
	ll2.Print();

	var res = Solve(ll1, ll2);
	res?.Value.Dump("4");
}

void TestCase2_NoCommonTail()
{
	PrintTestCase();
	var ll1 = LLN.Create(1, 2, 3, 4, 5);
	var ll2 = LLN.Create(3, 8, 15, 22, -1);

	ll1.Print();
	ll2.Print();

	var res = Solve(ll1, ll2);
	res?.Value.Dump("empty");
}

void TestCase1_CommonTail_DifferentLengths()
{
	PrintTestCase();
	var commonTail = LLN.Create(4, 5, 6);

	var ll1 = LLN.Create(commonTail, 1, 2, 3, 4, 5);
	var ll2 = LLN.Create(commonTail, 3, 8, 15, 22, -1, 2);

	ll1.Print();
	ll2.Print();

	var res = Solve(ll1, ll2);
	res?.Value.Dump("4");
}

LLN Solve(LLN ll1, LLN ll2)
{
	var cl1 = ll1.GetLength();
	var cl2 = ll2.GetLength();
	
	var longerL = cl1 > cl2 ? ll1 : ll2;
	var shorterL = ReferenceEquals(ll1, longerL) ? ll2 : ll1;
	longerL = longerL.Skip(Math.Abs(cl1 - cl2));
	
	// zip
	while (!ReferenceEquals(longerL, shorterL))
	{
		longerL = longerL.Next;
		shorterL = shorterL.Next;
	}
	
	return longerL;
}

void PrintTestCase([CallerMemberName] string tc = null)
{
	Console.WriteLine(tc);
}

class LLN
{
	public int Value { get; set; }
	public LLN Next { get; set; }
	
	public static LLN Create(params int[] headValues)
	{
		return Create(null, headValues);
	}
	
	public static LLN Create(LLN tail, params int[] headValues)
	{
		var prev = new LLN();
		LLN cur = null;
		LLN head = null;
		foreach (var val in headValues)
		{
			var first = cur == null;
			cur = new LLN() { Value = val };
			if (prev != null) 
				prev.Next = cur;
			if (first)
				head = cur;
				
			prev = cur;
		}
		
		if (cur != null)
			cur.Next = tail;
		
		return head;
	}
	
	public LLN Skip(int count)
	{
		var cur = this;
		for (int i = 0; i < count; i++)
		{
			cur = cur.Next;
		}
		return cur;
	}
	
	public int GetLength()
	{
		var count = 0;
		var cur = this;
		while (cur != null)
		{
			++count;
			cur = cur.Next;
		}
		return count;
	}

	public void Print()
	{
		var cur = this;
		while (cur != null)
		{
			Console.Write(" {0}", cur.Value);
			cur = cur.Next;
		}
		Console.WriteLine();
	}
}

// Define other methods and classes here