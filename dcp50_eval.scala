/*
This problem was asked by Microsoft.

Suppose an arithmetic expression is given as a binary tree. 
Each leaf is an integer and each internal node is one of '+', '−', '∗', or '/'.

Given the root to such a tree, write a function to evaluate it.

For example, given the following tree:

    *
   / \
  +    +
 / \  / \
3  2  4  5
You should return 45, as it is (3 + 2) * (4 + 5).
 */

object Tests {
  def runSample1(): Unit = {
    val left = BinaryOperation('+', Const(3), Const(2))
    val right = BinaryOperation('+', Const(4), Const(5))
    val expr = BinaryOperation('*', left, right)
    println(s"Should be 45: ${expr.eval()}")
  }
}

trait Expression {
  def eval(): Int
}

case class BinaryOperation(op: Char, left: Expression, right: Expression) extends Expression {
  def eval(): Int = op match {
    case '+' => left.eval + right.eval
    case '-' => left.eval - right.eval
    case '*' => left.eval * right.eval
    case '/' => left.eval / right.eval
  }
}

case class Const(value: Int) extends Expression {
  def eval(): Int = value
}
