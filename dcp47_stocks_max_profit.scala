
/*
This problem was asked by Facebook.

Given a array of numbers representing the stock prices of a company in chronological order, 
write a function that calculates the maximum profit you could have made from buying and selling that stock once. 
You must buy before you can sell it.

For example, given [9, 11, 8, 5, 7, 10], you should return 5, since you could buy the stock at 5 dollars and 
sell it at 10 dollars.
*/

object Tests {
  def runSample1(): Unit = {
    println("Should be 5: " + Solution.solve(List(9, 11, 8, 5, 7, 10)))
    println("Should be 0: " + Solution.solve(List(5, 5, 5, 5)))
    println("Should be -1: " + Solution.solve(List(5, 4, 3, 2)))
    println("Should be 0: " + Solution.solve(List()))
    println("Should be 5: " + Solution.solve(List(1, 2, 3, 4, 5, 6)))
  }
}

object Solution {
  private case class ProfitStats(lastMin: Option[Int], maxProfit: Option[Int]) {
    def addPrice(price: Int) = {
      lastMin.map(lastMinVal => {
        val nextMin = math.min(lastMinVal, price)
        val nextMaxProfit = maxProfit
          .map(maxProfitVal => math.max(maxProfitVal, price - lastMinVal))
          .orElse(Some(price - lastMinVal))
        
        ProfitStats(Some(nextMin), nextMaxProfit)
      }).getOrElse(ProfitStats(Some(price), None))
    }
  }
  
  def solve(prices: List[Int]): Int = {
    prices
      .foldLeft(ProfitStats(None, None))((s, price) => s.addPrice(price))
      .maxProfit
      .getOrElse(0)
  }
}

Tests.runSample1()
