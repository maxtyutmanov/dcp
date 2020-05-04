<Query Kind="Program" />

/*
This problem was asked by Dropbox.
Given the root to a binary search tree, find the second largest node in the tree.
*/

void Main()
{
	var tree1 = new TreeNode() { Value = 5 };
	new[] { 4, 15, 10, 11, 40 }.ToList().ForEach(x => tree1.SortedInsert(x));
	tree1.DFS().Skip(1).First().Value.Dump("Should be 15");

	var tree2 = new TreeNode() { Value = 5 };
	tree2.SortedInsert(6);
	tree2.DFS().Skip(1).First().Value.Dump("Should be 5");
}

class TreeNode
{
	public TreeNode Left { get; set; }
	
	public TreeNode Right { get; set; }
	
	public int Value { get; set; }
	
	public IEnumerable<TreeNode> DFS()
	{
		if (Right != null)
		{
			foreach (var n in Right.DFS())
			{
				yield return n;
			}
		}
		
		yield return this;
		
		if (Left != null)
		{
			foreach (var n in Left.DFS())
			{
				yield return n;
			}
		}
	}
	
	public void SortedInsert(int val)
	{
		if (val <= Value)
		{
			if (Left == null) Left = new TreeNode() { Value = val };
			else Left.SortedInsert(val);
		}
		else
		{
			if (Right == null) Right = new TreeNode() { Value = val };
			else Right.SortedInsert(val);
		}
	}
}