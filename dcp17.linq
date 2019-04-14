<Query Kind="Program" />

/*
Suppose we represent our file system by a string in the following manner:

The string "dir\n\tsubdir1\n\tsubdir2\n\t\tfile.ext" represents:

dir
    subdir1
    subdir2
        file.ext
The directory dir contains an empty sub-directory subdir1 and a sub-directory subdir2 containing a file file.ext.

The string "dir\n\tsubdir1\n\t\tfile1.ext\n\t\tsubsubdir1\n\tsubdir2\n\t\tsubsubdir2\n\t\t\tfile2.ext" represents:

dir
    subdir1
        file1.ext
        subsubdir1
    subdir2
        subsubdir2
            file2.ext
The directory dir contains two sub-directories subdir1 and subdir2. subdir1 contains a file file1.ext and an empty second-level sub-directory subsubdir1. subdir2 contains a second-level sub-directory subsubdir2 containing a file file2.ext.

We are interested in finding the longest (number of characters) absolute path to a file within our file system. For example, in the second example above, the longest absolute path is "dir/subdir2/subsubdir2/file2.ext", and its length is 32 (not including the double quotes).

Given a string representing the file system in the above format, return the length of the longest absolute path to a file in the abstracted file system. If there is no file in the system, return 0.

Note:

The name of a file contains at least a period and an extension.

The name of a directory or sub-directory will not contain a period.
*/

void Main()
{
	Solve("dir\n\tsubdir1\n\t\tfile1.ext\n\t\tsubsubdir1\n\tsubdir2\n\t\tsubsubdir2\n\t\t\tfile2.ext").Dump();
	Solve("dir\n\tsubdir1\n\t\tfile1.ext\n\t\tsubsubdir1\n\tsubdir2\n\t\tsubsubdir2\n\t\t\tfile2.ext\nsuperlongdirectorybutnobodycareshowlongitisbecauseitisjustadirectorynotafile").Dump();
	Solve("dir\n\tsubdir1\n\t\tfile1.ext\n\t\tsubsubdir1\n\tsubdir2\n\t\tsubsubdir2\n\t\t\tfile2.ext\nsuperlongdirectorybutthistimeithasfilesinit\n\tfileinsuperlongdirectory.txt").Dump();
	"superlongdirectorybutthistimeithasfilesinit/fileinsuperlongdirectory.txt".Length.Dump();
}

enum State
{
	ReadingDirOrFile,
	ReadingTabs,
	ReadingFile
}

int Solve(string fsString)
{
	var root = new FsNode("", null);
	var lastNodesByLevels = new List<FsNode>() { root };
	
	using (var reader = new StringReader(fsString))
	{
		string line;
		while ((line = reader.ReadLine()) != null)
		{
			var nodeName = line.TrimStart('\t');
			var level = line.Length - nodeName.Length + 1;
			var childNode = lastNodesByLevels[level - 1].AddChildNode(nodeName);
			
			// TODO: validation
			if (lastNodesByLevels.Count == level)
				lastNodesByLevels.Add(childNode);
			else
				lastNodesByLevels[level] = childNode;
		}
	}
	
	return root.GetMaxFilePathLengthForSubtree();
}

class FsNode
{
	public string Name { get; }

	public List<FsNode> Children { get; } = new List<FsNode>();

	public FsNode Parent { get; }

	public bool IsFile => Name.Contains('.');
	
	public FsNode(string name, FsNode parent)
	{
		Parent = parent;
		Name = name;
	}
	
	public FsNode AddChildNode(string name)
	{
		var child = new FsNode(name, this);
		Children.Add(child);
		return child;
	}
	
	public int GetMaxFilePathLengthForSubtree()
	{
		if (IsFile)
			return Name.Length;
			
		// this is a directory with child nodes
		
		var childMax = Children.Any() ? Children.Max(c => c.GetMaxFilePathLengthForSubtree()) : 0;
		if (childMax > 0)
		{
			var rootNode = (Parent == null);
			
			if (rootNode)
				return Name.Length + childMax;
			else
				// +1 for forward slash
				return Name.Length + 1 + childMax;
		}
		
		// no child files in the directory => should be no contribution to overall calculation
		return 0;
	}
}