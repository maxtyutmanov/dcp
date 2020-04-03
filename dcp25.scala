/*
 *  This problem was asked by Facebook.

    Implement regular expression matching with the following special characters:
    
    . (period) which matches any single character
    * (asterisk) which matches zero or more of the preceding element
    That is, implement a function that takes in a string and a valid regular expression and returns whether or not the string matches the regular expression.
    
    For example, given the regular expression "ra." and the string "ray", your function should return true. The same regular expression on the string "raymond" should return false.
    
    Given the regular expression ".*at" and the string "chat", your function should return true. The same regular expression on the string "chats" should return false.
 */

object Tests {
  def runSample1_with_any_char() = {
    println("Should be true: " + Solution.solve("ray", "ra."))
  }
  
  def runSample2_with_quantifier() = {
    println("Should be true: " + Solution.solve("chat", ".*at"))
    println("Should be false: " + Solution.solve("chats", ".*at"))
    println("Should be true: " + Solution.solve("chats", ".*ats.*"))
    println("Should be true: " + Solution.solve("chats", ".*ats.*.*"))
    println("Should be false: " + Solution.solve("chats", ".*ats.*.*c"))
    
    println("Should be true: " + Solution.solve("chatchat", ".*at"))
  }
  
  def runSample3_anychars() = {
    println("Should be true: " + Solution.solve("cha", "..."))
    println("Should be false: " + Solution.solve("chat", "..."))
    println("Should be false: " + Solution.solve("chat", "....."))
  }
}

object Solution {
  
  def solve(str: String, pattern: String): Boolean = {
    // get the chain of matchers
    val matcher = parsePattern(pattern)
    matchEntireString(matcher, str.toList) match {
      case (MatchSucceeded, _) => true
      case _ => false
    }
  }
  
  // MATCHING AGAINST (PARSED) REGEX PATTERN
  
  private def matchEntireString(m: Matcher, str: List[Char]): (Matcher, List[Char]) = (m, str) match {
    case (MatchFailed(_), _) => (m, str)  // shortcut: we already know it's failed 
    case (_, chr::tail) => {
      m.accept(str) match {
        case (next, nextStr) => matchEntireString(next, nextStr) 
      }
    }
    case (_, Nil) => m.accept(Nil)  // this denotes the end of input string
  }
  
  private trait Matcher {
    def accept(str: List[Char]): (Matcher, List[Char])
  }
  
  private case class MatchFailed(failedAt: List[Char]) extends Matcher {
    def accept(str: List[Char]): (Matcher, List[Char]) = (MatchFailed(failedAt), safeTail(str))
  }
  
  private object MatchSucceeded extends Matcher {
    def accept(str: List[Char]): (Matcher, List[Char]) = {
      if (str.isEmpty) (MatchSucceeded, safeTail(str))
      else (MatchFailed(str), safeTail(str))
    }
  }
  
  private case class SingleGivenChar(expected: Char, next: Matcher) extends Matcher {
    def accept(str: List[Char]): (Matcher, List[Char]) = {
      if (str.headOption == Some(expected)) (next, safeTail(str))
      else (MatchFailed(str), safeTail(str))
    }
  }
  
  private case class AnyChar(next: Matcher) extends Matcher {
    def accept(str: List[Char]): (Matcher, List[Char]) = {
      if (!str.isEmpty) (next, safeTail(str))
      else (MatchFailed(str), safeTail(str))
    }
  }
  
  private case class ZeroOrMoreQuantifier(inner: Matcher, next: Matcher) extends Matcher {
    def accept(str: List[Char]): (Matcher, List[Char]) = {
      // first try omitting the entire inner matcher
      val whatIfOmit = matchEntireString(next, str)
      whatIfOmit match {
        case (MatchSucceeded, _) => whatIfOmit
        case _ => {
          // omitting didn't work
          val innerResult = inner.accept(str)
          innerResult match {
            case (MatchFailed(failedAt), _) => innerResult
            case (_, innerStr) => (this, innerStr)
          }
        }
      }
    }
  }
  
  private def safeTail[A](as: List[A]): List[A] = as match {
    case h::t => t
    case Nil => Nil
  }
  
  // PARSING OF REGEX PATTERN
  
  private def parsePattern(pattern: String): Matcher = {
    pattern.foldRight(PatternParseState(MatchSucceeded, false))((chr, state) => state.accept(chr)).next
  }
  
  private case class PatternParseState(next: Matcher, zeroOrMore: Boolean) {
    def accept(chr: Char): PatternParseState = {
      if (chr == '*') PatternParseState(next, true)
      else if (chr == '.') PatternParseState(applyQuantifier(AnyChar(next)), false)
      else PatternParseState(applyQuantifier(SingleGivenChar(chr, next)), false)
    }
    
    private def applyQuantifier(regexState: Matcher): Matcher = {
      if (zeroOrMore) ZeroOrMoreQuantifier(regexState, next)
      else regexState
    }
  }
}

Tests.runSample1_with_any_char()
Tests.runSample2_with_quantifier()
Tests.runSample3_anychars()