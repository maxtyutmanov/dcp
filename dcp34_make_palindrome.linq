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
	Solve("rac").Dump("should be carac, NOT racar");
	Solve("rabdar").Dump("should be rabdbar");

	Solve("ab").Dump();
	Solve("ba").Dump();
	Solve("alabcala").Dump();
	Solve("test").Dump();
	Solve("already").Dump();
	Solve("somebigbigword").Dump();
	Solve("somebigbigwordqweqwqwe").Dump();
	Solve("somebigbigwordfortesting").Dump();
	Solve("somebigbigwordfortestingsomebigbigwordfortestingsomebigbigwordfortestingsomebigbigwordfortesting").Dump();
}

string Solve(string word, Dictionary<string, string> cache = null)
{
	if (cache == null) cache = new Dictionary<string, string>();
	else if (cache.ContainsKey(word)) return cache[word];
	
	if (word.Length <= 1)
	{
		return word;
	}
	
	if (word[0] == word[word.Length - 1])
	{
		// if first and last char in the word match, then we can ignore them and only need to make the "inner" part of the word a palindrome
		var res = word[0] + Solve(word.Substring(1, word.Length - 2), cache) + word[word.Length - 1];
		cache[word] = res;
		return res;
	}
	else
	{
		// if first and last char do not match, then we can try two options:
		// 1) insert the same char as the last one in the beginning of the word
		// 2) insert the same char as the first one in the end of the word
		
		// then we need to select the shortest word out of two. If they have the same length, choose the first alphabetically
		
		// if we insert the last char first
		var leftWord = word[word.Length - 1] + Solve(word.Substring(0, word.Length - 1), cache) + word[word.Length - 1];
		// if we insert the first char last
		var rightWord = word[0] + Solve(word.Substring(1, word.Length - 1), cache) + word[0];
		
		string res;
		
		if (leftWord.Length < rightWord.Length)
			res = leftWord;
		else if (leftWord.Length > rightWord.Length)
			res = rightWord;
		else if (leftWord.CompareTo(rightWord) < 0)
			res = leftWord;
		else
			res = rightWord;
			
		cache[word] = res;
		return res;
	}
}