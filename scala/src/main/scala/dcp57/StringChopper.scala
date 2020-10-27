package dcp57

/*
This problem was asked by Amazon.

Given a string s and an integer k, break up the string into multiple texts such that each text has a length of k or less.
You must break it up so that words don't break across lines. If there's no way to break the text up, then return null.

You can assume that there are no spaces at the ends of the string and that there is exactly one space between each word.

For example, given the string "the quick brown fox jumps over the lazy dog" and k = 10, you should return: ["the quick",
"brown fox", "jumps over", "the lazy", "dog"]. No string in the list has a length of more than 10.
 */

object StringChopper {
  def chop(s: String, maxLineLen: Int): Option[List[String]] = {
    val words = s.split(' ').toStream
    chopInternal(words, maxLineLen)
  }

  private def chopInternal(words: Stream[String], maxLineLen: Int): Option[List[String]] = {
    if (words.isEmpty) Some(Nil)
    else {
      val lengths = words.scanLeft(0)(_+_.length).zip(words)
      val wordsInThisLine = lengths.takeWhile(x => x._1 <= maxLineLen).map(_._2).toArray
      if (wordsInThisLine.length == 0) None
      else {
        val followingLines = chopInternal(words.drop(wordsInThisLine.length), maxLineLen)
        followingLines.map(fl => wordsInThisLine.mkString(" ")::fl)
      }
    }
  }
}
