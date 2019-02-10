<Query Kind="Program" />

/*
A unival tree (which stands for "universal value") is a tree where all nodes under it have the same value.

Given the root to a binary tree, count the number of unival subtrees.

For example, the following tree has 5 unival subtrees:

   0
  / \
 1   0
    / \
   1   0
  / \
 1   1
*/

void Main()
{
	var tree5 = new TreeNode(0,
		new TreeNode(1),
		new TreeNode(0,
			new TreeNode(1,
				new TreeNode(1),
				new TreeNode(1)),
			new TreeNode(0)));
			
	tree5.GetUnivalInfo().UnivalSubtreeCount.Dump();
	
	var tree7 = new TreeNode(0,
		new TreeNode(0,
			new TreeNode(0),
			new TreeNode(0)),
		new TreeNode(0,
			new TreeNode(0),
			new TreeNode(0)));
			
	tree7.GetUnivalInfo().UnivalSubtreeCount.Dump();
}

class UnivalInfo
{
	public bool IsSelfUnival;
	
	public int UnivalSubtreeCount;
}

class TreeNode
{
	public int Val;
	
	public TreeNode Left;
	
	public TreeNode Right;
	
	public TreeNode(int val, TreeNode left = null, TreeNode right = null)
	{
		Val = val;
		Left = left;
		Right = right;
	}
	
	public UnivalInfo GetUnivalInfo()
	{
		var count = 0;
		var isSelfUnival = true;
		
		if (Left != null)
		{
			var leftInfo = Left.GetUnivalInfo();
			count += leftInfo.UnivalSubtreeCount;
			if (!leftInfo.IsSelfUnival || Left.Val != Val)
			{
				isSelfUnival = false;
			}
		}
		
		if (Right != null)
		{
			var rightInfo = Right.GetUnivalInfo();
			count += rightInfo.UnivalSubtreeCount;
			if (!rightInfo.IsSelfUnival || Right.Val != Val)
			{
				isSelfUnival = false;
			}
		}
		
		if (isSelfUnival)
		{
			count++;
		}
		
		return new UnivalInfo()
		{
			IsSelfUnival = isSelfUnival,
			UnivalSubtreeCount = count
		};
	}
}

// Define other methods and classes here
