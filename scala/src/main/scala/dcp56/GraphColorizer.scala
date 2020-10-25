package dcp56

/*
This problem was asked by Google.

Given an undirected graph represented as an adjacency matrix and an integer k,
write a function to determine whether each vertex in the graph can be colored such that
no two adjacent vertices share the same color using at most k colors.
 */

object GraphColorizer {
  def canColorize(m: Array[Array[Boolean]], k: Int) = {
    // NP hard problem, solving by backtracking
    // also could have used Brooks' or Vizing's theorem

    if (k >= m.length) true // slight optimization
    else {
      val colors = new Array[Int](m.length)
      val result = tryColorize(m, k, colors, 0)
      if (result) println(s"Colors ${colors.mkString(",")}")
      result
    }
  }

  private def tryColorize(m: Array[Array[Boolean]], k: Int, colors: Array[Int], vertexIx: Int): Boolean = {
    if (vertexIx >= m.length) true
    else {
      // colorize vertex with index vertexIx
      val neighbourColors = m(vertexIx).toStream.zipWithIndex.filter(_._1).map(x => colors(x._2))
      val availableColors = getAvailableColors(neighbourColors, k)

      // backtracking: try all available colors one by one, get first working solution
      val workingSolutions = availableColors.toStream.filter(c => {
        colors(vertexIx) = c
        tryColorize(m, k, colors, vertexIx + 1)
      }).take(1).toList

      if (workingSolutions.isEmpty)
        colors(vertexIx) = 0

      !workingSolutions.isEmpty
    }
  }

  private def getAvailableColors(neighbourColors: Stream[Int], k: Int): List[Int] = {
    val occupiedColors = neighbourColors.filter(_ != 0).toSet
    (1 to k).filter(c => !occupiedColors.contains(c)).toList
  }
}
