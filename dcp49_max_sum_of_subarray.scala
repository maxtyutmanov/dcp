/*
This problem was asked by Amazon.

Given an array of numbers, find the maximum sum of any contiguous subarray of the array.

For example, given the array [34, -50, 42, 14, -5, 86], the maximum sum would be 137, 
since we would take elements 42, 14, -5, and 86.

Given the array [-5, -1, -8, -9], the maximum sum would be 0, since we would not take any elements.

Do this in O(N) time.
*/

object Tests {
  def runSample1(): Unit = {
    println(s"Should be 137: ${Solution.solve(Array(34, -50, 42, 14, -5, 86))}")
    println(s"Should be 0: ${Solution.solve(Array(-5, -1, -8, -9))}")
  }
}

object Solution {
  def solve(xs: Array[Int]): Int = {
    subSolve(xs, 0, 0, 0)    
  }
  
  @annotation.tailrec
  def subSolve(xs: Array[Int], startIx: Int, curSum: Int, maxSum: Int): Int = {
    if (startIx >= xs.length) maxSum
    else {
      val nextSum = curSum + xs(startIx)
      if (nextSum > 0) subSolve(xs, startIx + 1, nextSum, math.max(maxSum, nextSum))
      else subSolve(xs, startIx + 1, 0, maxSum)
    }
  }
}

Tests.runSample1()
