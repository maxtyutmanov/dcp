/*
 *  This problem was asked by Facebook.
    
    A builder is looking to build a row of N houses that can be of K different colors. He has a goal of minimizing cost while ensuring that no two neighboring houses are of the same color.
    
    Given an N by K matrix where the nth row and kth column represents the cost to build the nth house with kth color, return the minimum cost which achieves this goal.
 */

object Solution {
  trait HasCost { val cost: Int }
  type Color = Int
  type Cost = Int
  case class ColorAndCost(color: Color, cost: Cost) extends HasCost {
    def merge(other: ColorAndCost): ColorAndCost = {
      ColorAndCost(color, cost + other.cost)
    }
  }
  type CostColumn = List[ColorAndCost]
  type CostMatrix = List[CostColumn]
  
  def runSample1(): Unit = {
    val mx = List(
        List(100, 10000),
        List(10, 1000),
        List(100, 50),
        List(1, 10)).map(column => mapWithIndex(column)((cost, ix) => ColorAndCost(ix, cost)))
        
    val result = solve(mx)
    println(result)
  }
  
  def runSample2(): Unit = {
    val mx = List(
        List(100, 10000, 1),
        List(10, 1000, 1),
        List(100, 50, 1),
        List(1, 10, 1)).map(column => mapWithIndex(column)((cost, ix) => ColorAndCost(ix, cost)))
        
    val result = solve(mx)
    println(result)
  }
  
  def solve(cm: CostMatrix): Int = {
    val solutions = subSolve(cm)
    solutions.map(s => s.cost).min
  }
  
  def subSolve(cm: CostMatrix): CostColumn = cm match {
    case Nil => Nil
    case column::Nil => column
    case column::submatrix => {
      val subSolution = subSolve(submatrix).toStream
      // for each value in current column, we search for minimum cost in the sub-solution,
      // then merge those two values
      column.map(left => left merge minCostDiffColor(subSolution, left.color))
    }
  }
  
  def minCostDiffColor(s: Stream[ColorAndCost], color: Color): ColorAndCost =
    s.filter(x => x.color != color).minBy(x => x.cost)
    
  def mapWithIndex[A,B](as: List[A])(f: (A, Int) => B): List[B] = {
    as.zipWithIndex.map(x => f(x._1, x._2))
  }
}

Solution.runSample1()
Solution.runSample2()