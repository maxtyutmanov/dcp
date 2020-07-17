/*
This problem was asked by Google.

We can determine how "out of order" an array A is by counting the number of inversions it has. 
Two elements A[i] and A[j] form an inversion if A[i] > A[j] but i < j. That is, a smaller element appears after a larger element.

Given an array, count the number of inversions it has. Do this faster than O(N^2) time.

You may assume each element in the array is distinct.

For example, a sorted list has zero inversions. 
The array [2, 4, 1, 3, 5] has three inversions: (2, 1), (4, 1), and (4, 3). 
The array [5, 4, 3, 2, 1] has ten inversions: every distinct pair forms an inversion.
*/

object Tests {
  def runSample1() = {
    println("Should be 3: " + Solution.solve(Array(2, 4, 1, 3, 5)))
    println("Should be 10: " + Solution.solve(Array(5, 4, 3, 2, 1)))
    println("Should be 15: " + Solution.solve(Array(6, 5, 4, 3, 2, 1)))
    println("Should be 0: " + Solution.solve(Array(1, 2, 3, 4, 5)))
  }
}

object Solution {
  case class SubArray(xs: Array[Int], from: Int, to: Int) {
    def apply(i: Int): Int = xs.apply(from + i)
    def update(i: Int, x: Int): Unit = xs.update(from + i, x)
    
    def length: Int = to - from
    
    def divide: (SubArray, SubArray) = {
      val middle = from + (to - from) / 2
      (SubArray(xs, from, middle), SubArray(xs, middle, to))  
    }
  }
  
  def solve(xs: Array[Int]): (Int, Seq[Int]) = {
    val outXsArr = Array.fill(xs.length)(0)
    xs.copyToArray(outXsArr)
    val inXs = SubArray(xs, 0, xs.length)
    val outXs = SubArray(outXsArr, 0, xs.length)
    
    val inv = mergeSort(inXs, outXs)
    (inv, outXsArr.toList)
  }
  
  def mergeSort(inXs: SubArray, outXs: SubArray): Int = {
    var inv = 0
    if (inXs.length > 1) {
      val (in1, in2) = inXs.divide
      val (out1, out2) = outXs.divide
      val (inv1, inv2) = (mergeSort(out1, in1), mergeSort(out2, in2))
      var i1, i2, i = 0;
      inv = inv1 + inv2;
      
      while (i1 < in1.length && i2 < in2.length) {
        if (in1(i1) < in2(i2)) {
          outXs(i) = in1(i1);
          i1 += 1;
        }
        else {
          // item from the right part is greater than items from the left part => invertion
          outXs(i) = in2(i2)
          i2 += 1;
          inv += in1.length - i1;
        }
        
        i += 1;
      }
      
      while (i1 < in1.length) {
        outXs(i) = in1(i1);
        i1 += 1;
        i += 1;
      }
      
      while (i2 < in2.length) {
        outXs(i) = in2(i2);
        i2 += 1;
        i += 1;
      }
    }
    
    inv
  }
}
