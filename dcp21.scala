/*
    This problem was asked by Snapchat.

    Given an array of time intervals (start, end) for classroom lectures (possibly overlapping), find the minimum number of rooms required.
    
    For example, given [(30, 75), (0, 50), (60, 150)], you should return 2.
*/

object Solution {
  case class Interval(from: Int, to: Int)
  
  trait PointInTime { 
    val value: Int;
    def earlierThan(other: PointInTime): Boolean = {
      if (value < other.value) true
      else if (value > other.value) false
      else {
        // if values are same, we consider that previous class is finished first and the next class starts without intersection
        this match {
          case FinishPoint(_) => true
          case _ => false
        }
      }
    }
  }
  case class StartPoint(value: Int) extends PointInTime
  case class FinishPoint(value: Int) extends PointInTime
  
  private case class TraverseState(currentClasses: Int, maxClasses: Int) {    
    def onStarted: TraverseState = {
      TraverseState(currentClasses + 1, math.max(maxClasses, currentClasses + 1))
    }
    
    def onFinished: TraverseState = {
      TraverseState(currentClasses - 1, maxClasses)
    }
  }
  
  def runSample4_finish_and_start_at_the_same_time() {
    val intervals = List(Interval(30, 50), Interval(50, 60), Interval(60, 150))
    println(solve(intervals))
  }
  
  def runSample3_all_intersect() {
    val intervals = List(Interval(30, 50), Interval(0, 60), Interval(20, 150))
    println(solve(intervals))
  }
  
  def runSample2_no_intersections() {
    val intervals = List(Interval(30, 50), Interval(0, 20), Interval(60, 150))
    println(solve(intervals))
  }
  
  def runSample1_some_intersect() {
    val intervals = List(Interval(30, 75), Interval(0, 50), Interval(60, 150))
    println(solve(intervals))
  }
  
  def solve(intervals: List[Interval]): Int = {
    val allPoints = intervals.flatMap(i => List[PointInTime](StartPoint(i.from), FinishPoint(i.to)))
    val allPointsSorted = allPoints.sorted(Ordering.fromLessThan[PointInTime]((p1, p2) => p1.earlierThan(p2)))
    
    allPointsSorted.foldLeft(TraverseState(0, 0))((state, point) => point match {
      case StartPoint(_) => state.onStarted
      case FinishPoint(_) => state.onFinished
    }).maxClasses
  }
}

Solution.runSample1_some_intersect()
Solution.runSample2_no_intersections()
Solution.runSample3_all_intersect()
Solution.runSample4_finish_and_start_at_the_same_time()