<Query Kind="Program" />

/*
This problem was asked by Microsoft.

Compute the running median of a sequence of numbers. That is, given a stream of numbers, print out the median of the list so far on each new element.

Recall that the median of an even-numbered list is the average of the two middle numbers.
*/

void Main()
{
	var source = new[] { 2, 1, 5, 7, 2, 0, 5 };
	Solve(source).ToList().Dump();
}

IEnumerable<double> Solve(IEnumerable<int> stream)
{
	// max heap for left
	var left = new BinaryHeap((parent, child) => parent >= child);
	// min heap for right
	var right = new BinaryHeap((parent, child) => parent <= child);
	
	foreach (var val in stream)
	{
		// insert the value into the right heap
		if (left.Count != 0 && val <= left.Top)
		{
			left.Add(val);
		}
		else
		{
			right.Add(val);
		}

		var (smaller, larger) = (left.Count < right.Count) ? (left, right) : (right, left);

		// balance two heaps
		while (larger.Count - smaller.Count > 1)
		{
			smaller.Add(larger.ExtractTop());
		}
		
		if (larger.Count == smaller.Count)
		{
			yield return (smaller.Top + larger.Top) / 2.0;
		}
		else
		{
			yield return larger.Top;
		}
	}
}

delegate bool HeapInvariant(int parent, int child);

class BinaryHeap
{
	private readonly List<int> a;
	private readonly HeapInvariant _inv;

	public int Top
	{
		get
		{
			if (a.Count == 0)
			{
				throw new InvalidOperationException("heap is empty");
			}

			return a[0];
		}
	}
	
	public int Count => a.Count;

	public BinaryHeap(HeapInvariant inv)
	{
		a = new List<int>();
		_inv = inv;
	}

	public void Add(int x)
	{
		a.Add(x);
		var i = a.Count - 1;
		while (i > 0 && !_inv(a[i / 2], a[i]))
		{
			Swap(i, i / 2);
			i = i / 2;
		}
	}
	
	public int ExtractTop()
	{
		if (Count == 0)
			throw new InvalidOperationException("heap is empty");
			
		var top = a[0];
		
		a[0] = a[Count - 1];
		a.RemoveAt(Count - 1);
		Heapify(0);
		return top;
	}

	private void Heapify(int i)
	{
		var left = 2 * i + 1;
		var right = 2 * i + 2;
		var largest = i;

		if (left < a.Count && !_inv(a[largest], a[left]))
			largest = left;

		if (right < a.Count && !_inv(a[largest], a[right]))
			largest = right;

		if (largest != i)
		{
			Swap(i, largest);
			Heapify(largest);
		}
	}

	private void Swap(int i, int j)
	{
		var tmp = a[i];
		a[i] = a[j];
		a[j] = tmp;
	}
}