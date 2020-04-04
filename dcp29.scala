
/*
This problem was asked by Amazon.

Run-length encoding is a fast and simple method of encoding strings. The basic idea is to represent repeated successive characters as a single count and character. 
For example, the string "AAAABBBCCDAA" would be encoded as "4A3B2C1D2A".

Implement run-length encoding and decoding. You can assume the string to be encoded have no digits and consists solely of alphabetic characters. You can assume the string to be decoded is valid.
 */

object Tests {
  def runSample1_encode() = {
    println("Should be 4A3B2C1D2A: " + Solution.encode("AAAABBBCCDAA"))
  }
  
  def runSample2_decode() = {
    println("Should be AAAABBBCCDAA: " + Solution.decode("4A3B2C1D2A"))
  }
  
  def runSample3_decode_w_multiple_digits() = {
    println("Should be AAAAAAAAAABBBCCDAA: " + Solution.decode("10A3B2C1D2A"))
  }
}

object Solution {
  private case class EncoderState(curChr: Option[Char], cnt: Int, encoded: List[String]) {
    def accept(chr: Option[Char]): EncoderState = {
      if (chr == curChr) EncoderState(chr, cnt + 1, encoded)
      else EncoderState(chr, 1, encodeCurrent)
    }
    
    private def encodeCurrent: List[String] = curChr.map(c => (cnt.toString() + c)::encoded).getOrElse(encoded)
  }
  
  def encode(str: String): String = {
    str
      .foldLeft(EncoderState(None, 0, Nil))((state, chr) => state.accept(Some(chr)))
      .accept(None)  // denote the end of string
      .encoded
      .reverse
      .mkString
  }
  
  private case class DecoderState(cnt: Int, decoded: List[String]) {
    def accept(chr: Char): DecoderState = {
      if ('0' <= chr && chr <= '9') DecoderState(cnt * 10 + (chr - '0'), decoded)
      else DecoderState(0, decode(chr))
    }
    
    private def decode(chr: Char): List[String] = {
      if (cnt != 0) (chr.toString * cnt)::decoded
      else decoded
    }
  }
  
  def decode(str: String): String = {
    str
      .foldLeft(DecoderState(0, Nil))((state, chr) => state.accept(chr))
      .decoded
      .reverse
      .mkString
  } 
}

Tests.runSample1_encode()
Tests.runSample2_decode()
Tests.runSample3_decode_w_multiple_digits()