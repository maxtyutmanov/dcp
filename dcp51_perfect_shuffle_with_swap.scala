/*
This problem was asked by Facebook.

Given a function that generates perfectly random numbers between 1 and k (inclusive), 
where k is an input, write a function that shuffles a deck of cards represented as an 
array using only swaps.

It should run in O(N) time.

Hint: Make sure each one of the 52! permutations of the deck is equally likely.
 */

object Tests {
  def runSample1(): Unit = {
    val d = new Deck(52)
    println(d)
    d.shuffle()
    println(d)
  }
  
  def gatherStats(): Unit = {
    val iterations = 1000000
    val deckSize = 52
    val d = new Deck(deckSize)
    val stats = Array.fill(deckSize, deckSize + 1)(0)
    
    (1 to iterations).foreach(_ => {
      d.shuffle
      (0 to deckSize - 1).foreach(ix => {        
        val cardAtIx = d(ix)
        val counter = stats(ix)
        counter(cardAtIx) = counter(cardAtIx) + 1
      })
    })
    
    val printedStats = stats.zipWithIndex.map(x => {
      val ix = x._2
      val posStats = x._1.zipWithIndex.drop(1).map(y => "%02d".format(y._2) + " => " + "%02d".format(y._1)).mkString("|")
      
      s"Stats for position ${ix}\r\n:" + posStats
    }).mkString("\r\n\r\n")
    
    println(printedStats)
  }
}

class Deck(val size: Int) {
  private val a: Array[Int] = (1 to size).toArray
  private val rand = new scala.util.Random
  
  def apply(ix: Int) = a(ix)
  
  def shuffle(): Unit = shuffleInternal(size)
  
  private def shuffleInternal(k: Int): Unit = {
    if (k > 1) {
      shuffleInternal(k - 1)
      val pos = nextRand(k)
      swap(pos, k - 1)
    }
  }
  
  private def swap(ix1: Int, ix2: Int): Unit = {
    val tmp = a(ix1)
    a(ix1) = a(ix2)
    a(ix2) = tmp
  }
  
  private def nextRand(k: Int) = rand.nextInt(k)
  
  override def toString(): String = a.map(x => "%02d".format(x)).mkString("|")
}
