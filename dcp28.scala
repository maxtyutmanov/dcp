/*
 * This problem was asked by Palantir.

Write an algorithm to justify text. Given a sequence of words and an integer line length k, return a list of strings which represents each line, fully justified.

More specifically, you should have as many words as possible in each line. There should be at least one space between each word. Pad extra spaces when necessary so that each line has exactly length k. 
Spaces should be distributed as equally as possible, with the extra spaces, if any, distributed starting from the left.

If you can only fit one word on a line, then you should pad the right-hand side with spaces.

Each word is guaranteed not to be longer than k.

For example, given the list of words ["the", "quick", "brown", "fox", "jumps", "over", "the", "lazy", "dog"] and k = 16, you should return the following:

["the  quick brown", # 1 extra space on the left
"fox  jumps  over", # 2 extra spaces distributed evenly
"the   lazy   dog"] # 4 extra spaces distributed evenly
 */

object Tests {
  def runSample1() = {
    val words = List("the", "quick", "brown", "fox", "jumps", "over", "the", "lazy", "dog")
    println(Solution.solve(words, 16))
    println()
  }
  
  def runSample2() = {
    val words = List("the", "quick", "red", "fox", "jumps", "over", "the", "lazy", "dog")
    println(Solution.solve(words, 16))
    println()
  }
  
  def runSample3() = {
    val words = List("the", "quick", "red", "fox", "jumps", "over", "the", "lazy", "dog", "verylongword", "short")
    println(Solution.solve(words, 16))
    println()
  }
}

object Solution {
  def solve(words: List[String], k: Int): String = {
    justify(words.toStream, k).mkString("\n");
  }
  
  private def justify(words: Stream[String], k: Int): List[String] = {
    if (words.isEmpty) Nil
    else {
      val builders = words.scanLeft(LineBuilder(0, 0))((acc, word) => LineBuilder(acc.wc + 1, acc.cc + word.length)).drop(1)
      val zipped = (words zip builders)
      // while words still fit
      val lineWithBuilders = zipped.takeWhile(x => x._2.requiredLineLength <= k).toList
      val lineWords = lineWithBuilders.map(x => x._1)
      val lineBuilder = lineWithBuilders.last._2
      val line = lineBuilder.arrangeSpaces(lineWords, k)
      
      val restWords = words.drop(lineBuilder.wc)
      line::justify(restWords, k)
    }
  }
  
  private case class LineBuilder(wc: Int, cc: Int) {
    def requiredLineLength: Int = cc + (wc - 1)
    
    def arrangeSpaces(words: List[String], planW: Int): String = {
      val spacesRequired = planW - cc
      
      if (wc == 1) words.head + (" " * spacesRequired)
      else {
        // arrange required spaces into available spots between words
        val spaceSpots = wc - 1
        val spacesPerSpot = spacesRequired / spaceSpots
        val spotsWithAdditionalSpace = spacesRequired % spaceSpots
        val spotsWithoutAdditionalSpace = spaceSpots - spotsWithAdditionalSpace
        
        val left = Stream.fill(spotsWithAdditionalSpace)(" " * (spacesPerSpot + 1))
        val right = Stream.fill(spotsWithoutAdditionalSpace)(" " * spacesPerSpot)
        val spaces = left.append(right)
        
        words.zipAll(spaces, "", "").map(x => x._1 + x._2).mkString
      }
    }
  }
}

Tests.runSample1()
Tests.runSample2()
Tests.runSample3()