<Query Kind="Program">
  <Namespace>System.Runtime.CompilerServices</Namespace>
</Query>

/*
This problem was asked by Google.

Implement locking in a binary tree. A binary tree node can be locked or unlocked only if all of its descendants or ancestors are not locked.

Design a binary tree node class with the following methods:

is_locked, which returns whether the node is locked
lock, which attempts to lock the node. If it cannot be locked, then it should return false. Otherwise, it should lock it and return true.
unlock, which unlocks the node. If it cannot be unlocked, then it should return false. Otherwise, it should unlock it and return true.
You may augment the node to add parent pointers or any other property you would like. You may assume the class is used in a single-threaded program, so there is no need for actual locks or mutexes. 
Each method should run in O(h), where h is the height of the tree.
*/

void Main()
{
	Sample1_LockInRightSubtree_CanLockLeftSubtree();
	Sample2_CantLockDescendantIfAsdendantIsLocked();
	Sample3_CantLockAscendantIfDescendantIsLocked();
}

void Sample3_CantLockAscendantIfDescendantIsLocked()
{
	var leftLeaf = new TN();
	var rightLeaf = new TN();
	var leftSubtree = new TN();
	var rightSubtree = new TN(leftLeaf, rightLeaf);
	var tree = new TN(leftSubtree, rightSubtree);

	leftLeaf.Lock().Assert(true);
	rightLeaf.Lock().Assert(true);

	tree.Lock().Assert(false);
	rightSubtree.Lock().Assert(false);
	
	leftLeaf.Unlock().Assert(true);
	// still can't unlock the root, since right leaf is still locked
	tree.Lock().Assert(false);
	rightLeaf.Unlock().Assert(true);
	tree.Lock().Assert(true);
}

void Sample2_CantLockDescendantIfAsdendantIsLocked()
{
	var leftLeaf = new TN();
	var rightLeaf = new TN();
	var leftSubtree = new TN();
	var rightSubtree = new TN(leftLeaf, rightLeaf);
	var tree = new TN(leftSubtree, rightSubtree);
	
	tree.Lock().Assert(true);
	leftSubtree.Lock().Assert(false);
	leftLeaf.Lock().Assert(false);

	tree.Unlock().Assert(true);
	leftSubtree.Lock().Assert(true);
	leftLeaf.Lock().Assert(true);
}

void Sample1_LockInRightSubtree_CanLockLeftSubtree()
{
	var leftLeaf = new TN();
	var rightLeaf = new TN();
	var leftSubtree = new TN();
	var rightSubtree = new TN(leftLeaf, rightLeaf);
	var tree = new TN(leftSubtree, rightSubtree);
	
	leftLeaf.Lock().Assert(true);
	rightLeaf.Lock().Assert(true);
	leftSubtree.Lock().Assert(true);
}

class TN 
{
	public TN Left { get; private set; }
	
	public TN Right { get; private set; }
	
	public TN Parent { get; private set; }
	
	public TN(TN left = null, TN right = null)
	{
		if (left != null)
			left.Parent = this;
		
		if (right != null)
			right.Parent = this;
			
		Left = left;
		Right = right;
	}

	private HashSet<TN> _lockedDescendants = new HashSet<TN>();
	
	private bool _selfLocked;
	
	public bool IsReadonly => AscLocked || DescLocked;
	
	private bool AscLocked => GetAscendants().Any(n => n._selfLocked);
	private bool DescLocked => _lockedDescendants.Any();
	
	public bool IsLocked => _selfLocked;
	
	public bool Lock()
	{
		if (IsReadonly)
			return false;
			
		_selfLocked = true;
		foreach (var asc in GetAscendants())
		{
			asc._lockedDescendants.Add(this);
		}
		return true;
	}
	
	public bool Unlock()
	{
		if (IsReadonly)
			return false;
			
		_selfLocked = false;
		foreach (var asc in GetAscendants())
		{
			asc._lockedDescendants.Remove(this);
		}
		return true;
	}

	private IEnumerable<TN> GetAscendants()
	{
		var cur = Parent;
		while (cur != null)
		{
			yield return cur;
			cur = cur.Parent;
		}
	}
}

public static class Utils
{
	public static void Assert(this bool b, bool expected, [System.Runtime.CompilerServices.CallerMemberName] string testCase = null)
	{
		if (b != expected)
			throw new Exception($"Test case {testCase} failed!");
		
		Console.WriteLine($"Passed check in test case {testCase}");
	}
}