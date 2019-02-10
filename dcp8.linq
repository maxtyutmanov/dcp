<Query Kind="Program" />

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
