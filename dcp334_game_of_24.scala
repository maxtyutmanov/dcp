/*
 * This problem was asked by Twitter.

	 The 24 game is played as follows. You are given a list of four integers, each between 1 and 9, in a fixed order. 
	 By placing the operators +, -, *, and / between the numbers, and grouping them with parentheses, 
	 determine whether it is possible to reach the value 24.

	 For example, given the input [5, 2, 7, 8], you should return True, since (5 * 2 - 7) * 8 = 24.

	 Write a function that plays the 24 game.
 */

object Tests {
  def runSample1() = {
    println("Should be true: " + Solution.solve(List(5, 2, 7, 8), 24))
    println("Should be false: " + Solution.solve(List(5, 2, 7, 8), 23))
    println("Should be true: " + Solution.solve(List(1, 2, 1, 7), 24))
    println("Should be true: " + Solution.solve(List(9, 2, 3, 6), 24))
  }
  
  def runSample2() = {
    println("Should be true: " + Solution.solveLeetcode(Array(3, 3, 8, 8)))
  }
}

object Solution {
  def solveLeetcode(xs: Array[Int]): Boolean = {
    val perms = Permutations.generate(xs.map(x => x.toDouble))
    perms.exists(p => solve(p, 24))
  }
  
  def solve(xs: List[Double], target: Int): Boolean = xs match {
    case Nil => false  // empty set cannot be made into a target value obviously
    case h::Nil => math.abs(h - target) < 0.0001  // reduced the input down to a single number
    case _ => getPossibleReductions(xs).exists(reduction => {
      val s = solve(reduction, target)  // solve for smaller input
      if (s) println(reduction)
      s
    })
  }
  
  def getPossibleReductions(xs: List[Double]): Stream[List[Double]] = xs match {
    // try reducing input by squashing two items into one using a binary operation
    case a::b::t => {
      val squashed = getOperations(a, b).map(f => f(a, b)::t).toStream
      t match {
        case Nil => squashed  // must squash at least one pair
        case _ => {
          val nonSquashed = getPossibleReductions(xs.tail).map(rest => a::rest)
          squashed ++ nonSquashed
        }
      }
    }
    case t => Stream(t)
  }
  
  def getOperations(a: Double, b: Double): List[(Double, Double) => Double] = {
    val ops = List[(Double, Double) => Double](_/_, _+_, _-_, _*_)
    if (b != 0) ops
    else ops.drop(1)  // avoid division by zero
  }
}

object Permutations {
  def generate(xs: Array[Double]): Stream[List[Double]] = generateInternal(xs, List.range(0, xs.length).toSet)
  
  private def generateInternal(xs: Array[Double], available: Set[Int]): Stream[List[Double]] = {
    if (available.isEmpty) Stream(Nil)
    else available.toStream.map(i => generateInternal(xs, available - i).map(sublist => xs(i)::sublist)).flatten
  }
}
