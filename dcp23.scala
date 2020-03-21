/*
 *  This problem was asked by Google.

    You are given an M by N matrix consisting of booleans that represents a board. Each True boolean represents a wall. Each False boolean represents a tile you can walk on.
    
    Given this matrix, a start coordinate, and an end coordinate, return the minimum number of steps required to reach the end coordinate from the start. If there is no possible path, then return null. 
    You can move up, left, down, and right. You cannot move through walls. You cannot wrap around the edges of the board.
    
    For example, given the following board:
    
    [[f, f, f, f],
    [t, t, f, t],
    [f, f, f, f],
    [f, f, f, f]]
    and start = (3, 0) (bottom left) and end = (0, 0) (top left), the minimum number of steps required to reach the end is 7, since we would need to go through (1, 2) because there is a wall 
    everywhere else on the second row.
 */

object Solution {
  case class Coordinates(val row: Int, val col: Int) {
    def +(other: Coordinates): Coordinates = 
      Coordinates(row + other.row, col + other.col)
  }
  
  class Board(val walls: Array[Array[Boolean]]) {
    def isAccessible(c: Coordinates): Boolean =
      areValidCoords(c) && !walls(c.row)(c.col)
    
    def areValidCoords(c: Coordinates): Boolean = 
      c.row >= 0 && c.row < rowNum && c.col >= 0 && c.col < colNum
      
    def markAsInacessible(c: Coordinates): Unit = 
      walls(c.row)(c.col) = true
    
    def rowNum: Int = walls.length
    def colNum: Int = {
      if (rowNum != 0) walls(0).length
      else 0
    }
  }
  
  def runSample1_with_wall(): Unit = {
    val f = false
    val t = true
    val walls = Array(
        Array(f, f, f, f),
        Array(t, t, f, t),
        Array(f, f, f, f),
        Array(f, f, f, f))
        
    val board = new Board(walls)
    val result = solve(board, Coordinates(3, 0), Coordinates(0, 0))
    println(result)
  }
  
  def runSample2_no_walls(): Unit = {
    val f = false
    val t = true
    val walls = Array(
        Array(f, f, f, f),
        Array(f, f, f, f),
        Array(f, f, f, f),
        Array(f, f, f, f))
        
    val board = new Board(walls)
    val result = solve(board, Coordinates(3, 0), Coordinates(0, 0))
    println(result)
  }
  
  def runSample3_no_path(): Unit = {
    val f = false
    val t = true
    val walls = Array(
        Array(f, f, f, f),
        Array(t, t, t, t),
        Array(f, f, f, f),
        Array(f, f, f, f))
        
    val board = new Board(walls)
    val result = solve(board, Coordinates(3, 0), Coordinates(0, 0))
    println(result)
  }
  
  def runSample4_segmented_but_there_is_path(): Unit = {
    val f = false
    val t = true
    val walls = Array(
        Array(f, f, f, f),
        Array(t, t, t, t),
        Array(f, f, f, f),
        Array(f, f, f, f))
        
    val board = new Board(walls)
    val result = solve(board, Coordinates(3, 0), Coordinates(2, 3))
    println(result)
  }
  
  def solve(b: Board, start: Coordinates, end: Coordinates): Option[Int] = {
    subSolve(b, end, List(start), 0)
  }
  
  @annotation.tailrec
  def subSolve(b: Board, end: Coordinates, currentSet: List[Coordinates], curLen: Int): Option[Int] = {
    // UGLY SCALA BFS
    
    // filter the current set of coords to visit so that only accessible are left 
    val accessibleSet = currentSet.filter(c => b.isAccessible(c))
    // side effect: marking coordinates as visited so we won't stuck in an endless loop
    accessibleSet.foreach(c => b.markAsInacessible(c))
    
    accessibleSet match {
      case Nil => None
      case _ => {
        if (accessibleSet.find(c => c == end).isDefined) Some(curLen)
        else {
          val moveVectors = List(Coordinates(1, 0), Coordinates(-1, 0), Coordinates(0, 1), Coordinates(0, -1))
          val nextSet = accessibleSet.flatMap(c => moveVectors.map(c + _)).filter(c => b.areValidCoords(c))
          subSolve(b, end, nextSet, curLen + 1)
        }
      }
    }
  }
}

Solution.runSample1_with_wall()
Solution.runSample2_no_walls()
Solution.runSample3_no_path()
Solution.runSample4_segmented_but_there_is_path()