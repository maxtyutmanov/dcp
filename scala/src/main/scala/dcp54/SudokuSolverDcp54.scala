package dcp54

import gigahorse.support.okhttp.Gigahorse
import play.api.libs.json._

import scala.collection.mutable.HashSet
import scala.concurrent.Await
import scala.concurrent.duration.DurationInt

/*
This problem was asked by Dropbox.

Sudoku is a puzzle where you're given a partially-filled 9 by 9 grid with digits. 
The objective is to fill the grid with the constraint that every row, column, and box (3 by 3 subgrid) must contain all of the digits from 1 to 9.

Implement an efficient sudoku solver.
 */

object Tests {
  def runSample1(): Unit = {
    run(
      "000072000600030000027509610105060420902015300000900061406100830700080190018096045",
      "531672984649831257827549613185763429962415378374928561496157832753284196218396745")
  }

  def runSample2(): Unit = {
    run(
      "000000090090700210004090000010008000700420005008000074801000040000000000009613000",
      "157832496396745218284196753415378962763429185928561374831257649672984531549613827")
  }

  def runRandomSample(): Unit = {
    val (initial, expected)= SampleLoader.loadNew
    run(initial, expected)
  }

  private def run(init: String, expected: String): Unit = {
    val puzzle = SudokuSerializer.deserialize(init)
    val solution = SudokuSolver.solve(puzzle)
    val solutionStr = solution.map(x => SudokuSerializer.serialize(x)).getOrElse("")

    if (solutionStr == expected) println("OK!")
    else println(s"Expected ${expected}, but got ${solutionStr}")
  }
}

object SampleLoader {
  private val url = "https://sudoku.com/api/getLevel/expert"

  def loadNew(): (String, String) = {
    Gigahorse.withHttp { http =>
      val req = Gigahorse.url(url).get
      // TODO: rewrite with async/await
      val resp = Await.result(http.run(req, Gigahorse.asString), 10.seconds)
      val parsed = Json.parse(resp)

      val initial = (parsed \ "desc")(0).as[String]
      val expected = (parsed \ "desc")(1).as[String]
      (initial, expected)
    }
  }
}

object SudokuSerializer {
  private val size = 9

  def deserialize(s: String): Array[Array[Int]] = {
    val a = Array.ofDim[Int](size, size)
    s.map(c => c.toInt - '0'.toInt).zipWithIndex.foreach {
      case (digit, ix) => {
        val row = ix / size
        val col = ix % size
        a(row)(col) = digit
      }
    }
    a
  }

  def serialize(a: Array[Array[Int]]): String = {
    val sb = new StringBuilder
    for ( row <- (0 until size) ) {
      for ( col <- (0 until size) ) {
        sb.append(a(row)(col))
      }
    }
    sb.toString
  }
}

case class Cell(var value: Int, val subsets: List[HashSet[Int]]) {
  def isUnset(): Boolean = value == 0

  def setValue(newVal: Int): Unit = {
    value = newVal
    subsets.foreach(ss => ss += newVal)
  }

  def unsetValue(): Unit = {
    subsets.foreach(ss => ss -= value)
    value = 0
  }
}

object SudokuSolver {
  private val size = 9

  def solve(init: Array[Array[Int]]): Option[Array[Array[Int]]] = {
    val cells = Array.ofDim[Cell](size, size)
    val rowSets = Array.fill[HashSet[Int]](size)(new HashSet[Int])
    val colSets = Array.fill[HashSet[Int]](size)(new HashSet[Int])
    val sqSets = Array.fill[HashSet[Int]](size)(new HashSet[Int])

    for ( row <- 0 until size) {
      for ( col <- 0 until size) {
        val cellVal = init(row)(col)
        val relatedSets = List(rowSets(row), colSets(col), sqSets((row / 3) * 3 + col / 3))
        cells(row)(col) = Cell(cellVal, relatedSets)
        relatedSets.foreach(s => s.add(cellVal))
      }
    }

    if (subSolve(cells)) {
      val result = cells.map(_.map(_.value))
      Some(result)
    }
    else None
  }

  private def subSolve(c: Array[Array[Cell]]): Boolean = {
    // backtracking
    for (row <- 0 until size) {
      for (col <- 0 until size) {
        val cell = c(row)(col)
        if (cell.isUnset && !trySetValueAndContinue(c, cell)) return false
      }
    }
    return true
  }

  private def trySetValueAndContinue(c: Array[Array[Cell]], cell: Cell): Boolean = {
    val occupiedValues = cell.subsets.foldLeft(new HashSet[Int])((union, hs) => union ++ hs)
    val freeValues = new HashSet[Int]() ++ (1 to size) -- occupiedValues
    for ( freeVal <- freeValues ) {
      cell.setValue(freeVal)
      if (subSolve(c)) return true
      else cell.unsetValue()
    }
    // if we got here, we could not set a value for the current cell
    return false
  }
}