package dcp58

/*
This problem was asked by Amazon.

An sorted array of integers was rotated an unknown number of times.

Given such an array, find the index of the element in the array in faster than linear time. If the element doesn't exist in the array, return null.

For example, given the array [13, 18, 25, 2, 8, 10] and the element 8, return 4 (the index of 8 in the array).

You can assume all the integers in the array are unique.
 */

object Solution {
  def solve(xs: Array[Int], target: Int): Option[Int] = {
    if (xs.isEmpty) None
    else {
      // find an inversion: there has to be one unless the array was rotated exactly xs.length times
      val arrayStart = findArrayStart(xs, 0, xs.length - 1)
      val translateIx = (vix: Int) => (arrayStart + vix) % xs.length
      // index translation func
      val item = (vix: Int) => xs(translateIx(vix))
      val virtualIx = binSearch(item, target, 0, xs.length - 1)
      virtualIx.map(translateIx)
    }
  }

  private def binSearch(item: Int => Int, target: Int, l: Int, r: Int): Option[Int] = {
    if (r < l) None
    else {
      val middleIx = (l + r) / 2
      val middle = item(middleIx)

      if (target == middle) Some(middleIx)
      else if (target < middle) binSearch(item, target, l, middleIx - 1)
      else binSearch(item, target, middleIx + 1, r)
    }
  }

  def findInversion(xs: Array[Int], l: Int, r: Int): Option[Int] = {
    if (xs(l) <= xs(r)) None // this subarray is in order
    else if (r - l == 1) Some(r)
    else {
      val middle = (l + r) / 2
      findInversion(xs, l, middle).orElse(findInversion(xs, middle, r))
    }
  }

  def findArrayStart(xs: Array[Int], l: Int, r: Int): Int = findInversion(xs, l, r).getOrElse(0)
}
