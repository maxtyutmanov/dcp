import scala.collection.mutable.HashMap

/*
This problem was asked by Google.

The edit distance between two strings refers to the minimum number of character insertions, deletions, and substitutions required to change one string to the other. 
For example, the edit distance between “kitten” and “sitting” is three: substitute the “k” for “s”, substitute the “e” for “i”, and append a “g”.

Given two strings, compute the edit distance between them.
*/

object Tests {
  def runSample1() = {
    println("Should be 3: " + Solution.solve("kitten", "sitting"))
    println("Should be 3: " + Solution.solve("child", "children"))
    println("Should be 3: " + Solution.solve("children", "child"))
    println("Should be 3: " + Solution.solve("chilrden", "child"))
    println("Should be 4: " + Solution.solve("bcdefaaaabcdef", "bcdefbcdef"))
    println("Should be 5: " + Solution.solve("", "abcde"))
  }
}

object Solution {
  private case class DiffState(w1: String, w2: String, i: Int, j: Int, k: Int);
  
  def solve(w1: String, w2: String): Int = {
    lazy val subsolve: DiffState => Int = memoize (s => {
      if (s.i >= s.w1.length && s.j >= s.w2.length) s.k
      else {
        val c1 = if (s.i < s.w1.length) Some(s.w1(s.i)) else None
        val c2 = if (s.j < s.w2.length) Some(s.w2(s.j)) else None
        
        if (c1 == c2) subsolve(DiffState(s.w1, s.w2, s.i + 1, s.j + 1, s.k))
        else {
          val updScore = Some(subsolve(DiffState(s.w1, s.w2, s.i + 1, s.j + 1, s.k + 1)))
          val delScore = c1.map(_ => subsolve(DiffState(s.w1, s.w2, s.i + 1, s.j, s.k + 1)))
          val insScore = c2.map(_ => subsolve(DiffState(s.w1, s.w2, s.i, s.j + 1, s.k + 1)))
          
          List(updScore, delScore, insScore).filter(x => !x.isEmpty).map(x => x.get).min
        }
      }
    })
    
    subsolve(DiffState(w1, w2, 0, 0, 0))
  }
  
  def memoize[I, O](f: I => O): I => O = new HashMap[I, O]() {
    override def apply(key: I) = getOrElseUpdate(key, f(key))
  }
}

Tests.runSample1()