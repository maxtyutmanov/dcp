<Query Kind="Program" />

/*
This problem was asked by Microsoft.

You have an N by N board. Write a function that, given N, returns the number of possible arrangements of the board where N queens can be placed on the board without threatening each other, 
i.e. no two queens share the same row, column, or diagonal.
*/

void Main()
{
	Solve(13).Dump();
}

long Solve(int n)
{
	// start with empty board
	var b = new int[n, n];
	return SubSolve(b, 0);
}

long SubSolve(int[,] b, int startRow)
{
	// use backtracking
	
	var numOfArrangements = 0L;
	var n = b.GetLength(0);
	
	if (startRow == n - 1)
	{
		// final row, this is where the recursion stops
		for (int j = 0; j < n; j++)
		{
			if (b[startRow, j] == 0) numOfArrangements++;
		}
		
		return numOfArrangements;
	}
	
	for (int j = 0; j < n; j++)
	{
		if (b[startRow, j] == 0)
		{
			// try placing a queen here
			PlaceQueen(b, startRow, j);
			// count possible arrangements given this particular placement of a queen in this row
			numOfArrangements += SubSolve(b, startRow + 1);
			// remove, try placing a queen elsewhere
			RemoveQueen(b, startRow, j);
		}
	}
	
	return numOfArrangements;
}

void PlaceQueen(int[,] b, int row, int col)
{
	ChangePlacement(b, row, col, 1);
}

void RemoveQueen(int[,] b, int row, int col)
{
	ChangePlacement(b, row, col, -1);
}

void ChangePlacement(int[,] b, int row, int col, int p)
{
	var n = b.GetLength(0);
	b[row, col] += p;
	if (b[row, col] > 1) throw new InvalidOperationException("this place is either taken by another queen or threatened by it");
	if (b[row, col] < 0) throw new InvalidOperationException("can't remove a queen for a place where there is none");
	
	for (int i = 0; i < n; i++)
	{
		if (i != col) b[row, i] += p;
	}
	for (int j = 0; j < n; j++)
	{
		if (j != row) b[j, col] += p;
	}
	
	for (int i = row + 1, j = col + 1; i < n && j < n; i++, j++)
	{
		b[i, j] += p;
	}

	for (int i = row - 1, j = col - 1; i >= 0 && j >= 0; i--, j--)
	{
		b[i, j] += p;
	}

	for (int i = row + 1, j = col - 1; i < n && j >= 0; i++, j--)
	{
		b[i, j] += p;
	}

	for (int i = row - 1, j = col + 1; i >= 0 && j < n; i--, j++)
	{
		b[i, j] += p;
	}
}
