/*
This problem was asked by Two Sigma.
Using a function rand5() that returns an integer from 1 to 5 (inclusive) with uniform probability, 
implement a function rand7() that returns an integer from 1 to 7 (inclusive).
 */

object Tests {
  def runSample1() = {
    val samplesCount = 1000000;
    
    val resultsArr = Stream.continually(Solution.rand7).take(samplesCount).foldLeft(Array.fill(7)(0))((res, x) => {
      res(x - 1) = res(x - 1) + 1
      res
    })
    
    val freqList = resultsArr.map(cnt => cnt.toDouble / samplesCount).toList
    println("All items should be about the same: " + freqList)
  }
}

object Solution {
  private val gen: scala.util.Random = new scala.util.Random
  
  def rand7: Int = {
    val (v1, v2) = (rand5, rand5)
    
    // zero based position in flat array of all possible outcomes (11, 12, ..., 15, 21, 22, ..., ..., 51, 52, ..., 55)
    val pos = (v1 - 1) * 5 + v2 - 1
    
    if (pos > 20) rand7
    else (pos / 3 + 1)
  }
  
  private def rand5: Int = gen.nextInt(5) + 1
}
