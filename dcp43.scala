/*
This problem was asked by Amazon.

Implement a stack that has the following methods:

- push(val), which pushes an element onto the stack
- pop(), which pops off and returns the topmost element of the stack. 
  If there are no elements in the stack, then it should throw an error or return null.
- max(), which returns the maximum value in the stack currently. 
  If there are no elements in the stack, then it should throw an error or return null.
 */

trait MaxStack {
  def max: Option[Integer]
  def push(value: Integer): MaxStack
  def pop: (Option[Integer], MaxStack)
}

case object EmptyMaxStack extends MaxStack {
  def max = None
  def push(value: Integer): MaxStack = NonEmptyMaxStack(value, this, value)
  def pop = (None, this)
}

case class NonEmptyMaxStack(val head: Integer, val tail: MaxStack, val maxValue: Integer) extends MaxStack {
  def max = Some(maxValue)
  def push(value: Integer): MaxStack = NonEmptyMaxStack(value, this, math.max(value, maxValue))
  def pop = (Some(head), tail)
}

object Tests {
  def runSample1() = {
    val s1 = EmptyMaxStack
    val s2 = s1.push(1)
    val s3 = s2.push(10)
    val s4 = s3.push(5)
    val s5 = s4.push(11)
    val s6 = s5.push(6)
    
    println("Should be 11: " + s6.max)
    println("Should be 6: " + s6.pop)
    println("Should be 10: " + s6.pop._2.pop._2.max)
    println("Should be None: " + s1.pop._1)
    println("Should be None: " + s1.pop._2.pop._1)
  }
}

Tests.runSample1()
