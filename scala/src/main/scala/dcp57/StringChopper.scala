package dcp57

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
