<Query Kind="Program" />

void Main()
{
	var h = new BinaryHeap((parent, child) => parent >= child);
	var a = new[] { 4, 10, 3, 5, 1 };
	foreach (var x in a)
	{
		h.Add(x);
	}
	
	h.GetTop().Dump();
}

delegate bool HeapInvariant(int parent, int child);

class BinaryHeap
{
	private readonly List<int> a;
	private readonly HeapInvariant _inv;
	
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
	
	public int GetTop()
	{
		if (a.Count == 0)
		{
			throw new InvalidOperationException("heap is empty");
		}
		
		return a[0];
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
