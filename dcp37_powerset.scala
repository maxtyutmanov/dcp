/*
This problem was asked by Google.
The power set of a set is the set of all its subsets. Write a function that, given a set, generates its power set.
For example, given the set {1, 2, 3}, it should return {{}, {1}, {2}, {3}, {1, 2}, {1, 3}, {2, 3}, {1, 2, 3}}.
You may also use a list or array to represent a set.
*/

object Tests {
  def runSample1(): Unit = {
    println(Solution.solve(List(1, 2, 3)))
    println(Solution.solve(List(1, 2, 3, 4)))
  }
}

object Solution {
  def solve(a: List[Int]): List[List[Int]] = a match {
    // power set of an empty set has only 1 item - the empty set
    case Nil => List(Nil)
    // generating power set for all items except the current (h), then 
    // creating two versions of each set by either appending or not appending the current item to them
    case h::t => solve(t).flatMap(subset => List(subset, h::subset))
  }
}

Tests.runSample1()