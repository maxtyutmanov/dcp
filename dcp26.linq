<Query Kind="Program" />

/*
This problem was asked by Google.

Given a singly linked list and an integer k, remove the kth last element from the list. k is guaranteed to be smaller than the length of the list.

The list is very long, so making more than one pass is prohibitively expensive.

Do this in constant space and in one pass.
*/

void Main()
{
	var ll1 = Solve(LLN.CreateList(new[] { 1, 2, 3, 4, 5, 6, 7 }), 1);
	var ll2 = Solve(LLN.CreateList(new[] { 1, 2, 3, 4, 5, 6, 7 }), 7);
	var ll3 = Solve(LLN.CreateList(new[] { 1, 2, 3, 4, 5, 6, 7 }), 3);
	
	string.Join(" ", ll1.GetAll()).Dump();
	string.Join(" ", ll2.GetAll()).Dump();
	string.Join(" ", ll3.GetAll()).Dump();
}

LLN Solve(LLN ll, int k)
{
	var (kthPrev, kth) = FindKthFromEnd(ll, k);
	if (kthPrev != null)
	{
		kthPrev.Next = kth.Next;
		return ll;
	}
	else
	{
		// delete head
	    return ll.Next;
	}
}

(LLN kthPrev, LLN kth) FindKthFromEnd(LLN ll, int k)
{
	// establish the following invariant for pointers
	// cur - current pointer
	// kth - points to k items back relative to the cur
	// kthPrev - points to the item that preceds kth (or null if kth points to the first item)
	
	LLN kthPrev = null;
	var kth = ll;
	var cur = ll;
	// skip to kth item in the list
	for (int i = 0; i < k; i++)
	{
		cur = cur.Next;
	}

	// run all pointers in parallel until cur hits the end of list
	while (cur != null)
	{
		kthPrev = kth;
		kth = kth.Next;
		cur = cur.Next;
	}
	
	return (kthPrev, kth);
}

class LLN
{
	public int Value;
	public LLN Next;
	
	public static LLN CreateList(IEnumerable<int> xs)
	{
		var e = xs.GetEnumerator();
		if (!e.MoveNext())
			return null;

		var head = new LLN { Value = e.Current };
		var cur = head;
		
		while (e.MoveNext())
		{
			cur.Next = new LLN { Value = e.Current };
			cur = cur.Next;
		}
		
		return head;
	}
	
	public IEnumerable<int> GetAll()
	{
		var cur = this;
		while (cur != null)
		{
			yield return cur.Value;
			cur = cur.Next;
		}
	}
}
