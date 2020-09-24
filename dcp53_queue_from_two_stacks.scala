/*
This problem was asked by Apple.

Implement a queue using two stacks. Recall that a queue is a FIFO (first-in, first-out) data structure 
with the following methods: enqueue, which inserts an element into the queue, and dequeue, which removes it.
 */

object Tests {
  def runSample1(): Unit = {
    var q = new SQueue[Int]
    var a1: Int = 0
    q = q.enqueue(1)
    q = q.enqueue(2)
    q = q.enqueue(3)
    val d1 = q.dequeue()
    q = d1._2
    println(s"Should be 1: ${d1._1}")
    q = q.enqueue(4)
    val d2 = q.dequeue()
    q = d2._2
    println(s"Should be 2: ${d2._1}")
  }
}

case class ImStack[A](val l: List[A]) {
  def this() = {
    this(Nil)
  }
  
  def push(a: A): ImStack[A] = ImStack(a::l)
  
  def pop(): (Option[A], ImStack[A]) = l match {
    case Nil => (None, this)
    case a::rest => (Some(a), ImStack(rest))
  }
}

case class SQueue[A](private val readStack: ImStack[A], private val writeStack: ImStack[A]) {
  def this() = {
    this(new ImStack[A], new ImStack[A])
  }
  
  def enqueue(a: A): SQueue[A] = {
    SQueue(readStack, writeStack.push(a))
  }
  
  def dequeue(): (Option[A], SQueue[A]) = {
    val (a, newReadStack) = readStack.pop()
    a match {
      case Some(_) => (a, SQueue(newReadStack, writeStack))
      case None => copy().dequeue()
    }
  }
  
  private def copy(): SQueue[A] = {
    val (a, newWriteStack) = writeStack.pop()
    a match {
      case Some(aVal) => SQueue(readStack.push(aVal), newWriteStack).copy()
      case None => this
    }
  }
}