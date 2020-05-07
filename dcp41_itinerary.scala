/*
This problem was asked by Facebook.

Given an unordered list of flights taken by someone, each represented as (origin, destination) pairs, 
and a starting airport, compute the person's itinerary. If no such itinerary exists, return null. If there are multiple possible itineraries, 
return the lexicographically smallest one. All flights must be used in the itinerary.

For example, given the list of flights [('SFO', 'HKO'), ('YYZ', 'SFO'), ('YUL', 'YYZ'), ('HKO', 'ORD')] and starting airport 'YUL', you should return the list ['YUL', 'YYZ', 'SFO', 'HKO', 'ORD'].

Given the list of flights [('SFO', 'COM'), ('COM', 'YYZ')] and starting airport 'COM', you should return null.

Given the list of flights [('A', 'B'), ('A', 'C'), ('B', 'C'), ('C', 'A')] and starting airport 'A', 
you should return the list ['A', 'B', 'C', 'A', 'C'] even though ['A', 'C', 'A', 'B', 'C'] is also a valid itinerary. 
However, the first one is lexicographically smaller.
 */

case class Flight(from: String, to: String)

object Tests {
  def runSample1() = {
    val flights = Set(Flight("SFO", "HKO"), Flight("YYZ", "SFO"), Flight("YUL", "YYZ"), Flight("HKO", "ORD"))
    println("Should be ['YUL', 'YYZ', 'SFO', 'HKO', 'ORD']: " + Solution.solve(flights, "YUL"))
  }
  
  def runSample2() = {
    val flights = Set(Flight("SFO", "COM"), Flight("COM", "YYZ"))
    println("Should be none: " + Solution.solve(flights, "COM"))
  }
  
  def runSample3() = {
    val flights = Set(Flight("A", "B"), Flight("A", "C"), Flight("B", "C"), Flight("C", "A"))
    println("Should be ['A', 'B', 'C', 'A', 'C']: " + Solution.solve(flights, "A"))
  }
}

object Solution {
  def solve(flights: Set[Flight], start: String): Option[List[String]] = {
    if (flights.isEmpty) Some(List(start))
    else {
      val next = flights.filter(f => f.from == start).toList.sortBy(f => f.from).toStream
      next
        .map(f => solve(flights - f, f.to))
        .filter(solution => !solution.isEmpty)
        .headOption
        .flatten
        .map(subsolution => start::subsolution)
    }
  }
}

Tests.runSample1()
Tests.runSample2()
Tests.runSample3()