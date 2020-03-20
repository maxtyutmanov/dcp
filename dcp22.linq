<Query Kind="Program" />

/*
This problem was asked by Microsoft.

Given a dictionary of words and a string made up of those words (no spaces), return the original sentence in a list. If there is more than one possible reconstruction, return any of them. If there is no possible reconstruction, then return null.

For example, given the set of words 'quick', 'brown', 'the', 'fox', and the string "thequickbrownfox", you should return ['the', 'quick', 'brown', 'fox'].

Given the set of words 'bed', 'bath', 'bedbath', 'and', 'beyond', and the string "bedbathandbeyond", return either ['bed', 'bath', 'and', 'beyond] or ['bedbath', 'and', 'beyond'].
*/

void Main()
{
	RunSample1_Unambiguous();
	RunSample2_Ambiguous();
	RunSample3_NotMatching();
	RunSample4_MatchingButTricky();
}

void RunSample1_Unambiguous()
{
	var dict = new HashSet<string>()
	{
	  "quick", "brown", "the", "fox"
	};

	var str = "thequickbrownfox";
	Solve(dict, str)?.ToList().Dump();
}

void RunSample2_Ambiguous()
{
	var dict = new HashSet<string>()
	{
	  "bed", "bath", "and", "bedbath", "beyond"
	};

	var str = "bedbathandbeyond";
	Solve(dict, str)?.ToList().Dump();
}

void RunSample3_NotMatching()
{
	var dict = new HashSet<string>()
	{
	  "bed", "bath", "and", "bedbath", "beyond"
	};

	var str = "bedbatandbeyond";
	Solve(dict, str)?.ToList().Dump();
}

void RunSample4_MatchingButTricky()
{
	var dict = new HashSet<string>()
	{
	  "a", "abc"
	};

	var str = "abc";
	Solve(dict, str)?.ToList().Dump();
}

IEnumerable<string> Solve(HashSet<string> dict, string str)
{
	var buffer = new StringBuilder();
	return SubSolve(dict, str, 0, buffer);
}

IEnumerable<string> SubSolve(HashSet<string> dict, string str, int ix, StringBuilder buffer)
{
	if (ix >= str.Length)
	{
		//reached the end of string
		if (buffer.Length == 0)
			return Enumerable.Empty<string>();
		else
			return null;
	}
	
	buffer.Append(str[ix]);
	
	var word = buffer.ToString();
	if (dict.Contains(word))
	{
		// partial match! let's try to match the rest of the string
		buffer.Clear();
		
		var subSolution = SubSolve(dict, str, ix + 1, buffer);
		if (subSolution != null)
		{
			// success! the rest of the string matches!
			return new[] { word }.Concat(subSolution);
		}
		else
		{
			// could not match the rest of the string, maybe we matched the current word too early
			// let's step back
			buffer.Clear();
			buffer.Append(word);
		}
	}
	
	return SubSolve(dict, str, ix + 1, buffer);
}



