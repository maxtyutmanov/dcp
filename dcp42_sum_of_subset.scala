/*
This problem was asked by Google.

Given a list of integers S and a target number k, write a function that returns a subset of S that adds up to k. If such a subset cannot be made, then return null.

Integers can appear more than once in the list. You may assume all numbers in the list are positive.

For example, given S = [12, 1, 61, 5, 9, 2] and k = 24, return [12, 9, 2, 1] since it sums up to 24.
 */

object Tests {
  def runSample1() = {
    println("Should be (12, 9, 2, 1): " + Solution.solve(List(12, 1, 61, 5, 9, 2), 24))
  }
}

object Solution {
  // use backtracking (is there a better way?)
  def solve(a: List[Int], k: Int): Option[List[Int]] = {
    if (k == 0) Some(Nil) // empty set adds up to 0
    else if (k < 0) None  // no negative numbers in the set, thus there is no such subset that adds up to a negative number
    else a match {
      case Nil => None
      // try including k, if this does not work, try not including it in the subset
      case h::t => solve(t, k - h).map(subsolution => h::subsolution).orElse(solve(t, k))
    }
  }
}

Tests.runSample1()