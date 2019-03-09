<Query Kind="Program" />

/*
Implement an autocomplete system. That is, given a query string s and a set of all possible query strings, return all strings in the set that have s as a prefix.

For example, given the query string de and the set of strings [dog, deer, deal], return [deer, deal].

Hint: Try preprocessing the dictionary into a more efficient data structure to speed up queries.
*/

void Main()
{
	var ix = new AutocompleteIndex();
	ix.Add("dog");
	ix.Add("deer");
	ix.Add("deal");
	ix.GetCandidates("de").Dump();
}

class AutocompleteIndex
{
	private readonly IndexNode _root = new IndexNode();
	
	public void Add(string word)
	{
		var ctx = new InsertTraversal(_root);
		ctx.Traverse(word);
	}
	
	public IEnumerable<string> GetCandidates(string query)
	{
		var ctx = new SearchTraversal(_root);
		
		// navigate to subtree
		if (!ctx.Traverse(query))
		{
			yield break;
		}
		
		// enumerate words in subtree
		foreach (var word in ctx.EnumerateWordsInSubtree())
		{
			yield return word;
		}
	}
}

class IndexNode
{
	public Dictionary<char, IndexNode> Children { get; } = new Dictionary<char, IndexNode>();

	public bool IsTerminal => Children.Count == 0;
}

class SearchTraversal : IndexTraversal
{
	private StringBuilder CurrentPrefix { get; } = new StringBuilder();
	
	public SearchTraversal(IndexNode startNode)
		: base(startNode)
	{
	}
	
	protected override bool WalkDown(char c)
	{
		if (CurrentNode.Children.TryGetValue(c, out var nextNode))
		{
			CurrentPrefix.Append(c);
			CurrentNode = nextNode;
			return true;
		}
		
		return false;
	}

	public IEnumerable<string> EnumerateWordsInSubtree()
	{
		if (CurrentNode.IsTerminal)
		{
			yield return CurrentPrefix.ToString();
		}

		var prevNode = CurrentNode;
		foreach (var child in CurrentNode.Children)
		{
			WalkDown(child.Key);

			foreach (var word in EnumerateWordsInSubtree())
			{
				yield return word;
			}

			// walk up, reusing the same buffer to save space
			CurrentPrefix.Remove(CurrentPrefix.Length - 1, 1);
			CurrentNode = prevNode;
		}
	}
}

class InsertTraversal : IndexTraversal
{
	public InsertTraversal(IndexNode startNode)
		: base(startNode)
	{
	}

	protected override bool WalkDown(char c)
	{
		if (!CurrentNode.Children.TryGetValue(c, out var nextNode))
		{
			nextNode = new IndexNode();
			CurrentNode.Children.Add(c, nextNode);
		}
		
		CurrentNode = nextNode;
		return true;
	}
}

abstract class IndexTraversal
{
	protected IndexNode CurrentNode { get; set; }
	
	protected IndexTraversal(IndexNode startNode)
	{
		CurrentNode = startNode;
	}
	
	public bool Traverse(string sequence)
	{
		foreach (var c in sequence)
		{
			if (!WalkDown(c))
			{
				return false;
			}
		}
		
		return true;
	}
	
	protected abstract bool WalkDown(char c);
}