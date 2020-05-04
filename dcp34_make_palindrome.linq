<Query Kind="Program" />

/*
This problem was asked by Quora.

Given a string, find the palindrome that can be made by inserting the fewest number of characters as possible anywhere in the word. 
If there is more than one palindrome of minimum length that can be made, return the lexicographically earliest one (the first one alphabetically).

For example, given the string "race", you should return "ecarace", since we can add three letters to it (which is the smallest amount to make a palindrome). 
There are seven other palindromes that can be made from "race" by adding three letters, but "ecarace" comes first alphabetically.

As another example, given the string "google", you should return "elgoogle".
*/

void Main()
{
	Solve("google").Dump("should be elgoogle");
	Solve("race").Dump("should be ecarace");
	Solve("rabdar").Dump("should be rabdbar");
}

string Solve(string word)
{
	if (word.Length <= 1)
	{
		return word;
	}
	
	if (word[0] == word[word.Length - 1])
	{
		return word[0] + Solve(word.Substring(1, word.Length - 2)) + word[word.Length - 1];
	}
	else
	{
		// if we insert the last char first
		var leftWord = word[word.Length - 1] + Solve(word.Substring(0, word.Length - 1)) + word[word.Length - 1];
		// if we insert the first char last
		var rightWord = word[0] + Solve(word.Substring(1, word.Length - 1)) + word[0];
		
		if (leftWord.Length < rightWord.Length)
			return leftWord;
		else if (leftWord.Length > rightWord.Length)
			return rightWord;
		else if (leftWord.CompareTo(rightWord) < 0)
			return leftWord;
		else
			return rightWord;
	}
}