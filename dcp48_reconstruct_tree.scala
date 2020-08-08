
/*
This problem was asked by Google.

Given pre-order and in-order traversals of a binary tree, write a function to reconstruct the tree.

For example, given the following preorder traversal:

[a, b, d, e, c, f, g]

And the following inorder traversal:

[d, b, e, a, f, c, g]

You should return the following tree:

    a
   / \
  b   c
 / \ / \
d  e f  g

*/

object Tests {
  def runSample1(): Unit = {
    println(Solution.solve(List("a", "b", "d", "e", "c", "f", "g"), List("d", "b", "e", "a", "f", "c", "g")))
  }
}

case class Node[A](value: A, left: Option[Node[A]], right: Option[Node[A]]) {
  override def toString(): String = {
    s"(${value}, ${left.map(l => l.toString).getOrElse("")}, ${right.map(l => l.toString).getOrElse("")})"
  }
}

object Solution {
  def solve(pre: List[String], in: List[String]): Option[Node[String]] = {
    pre match {
      case root::rest => {
        val pivot = in.indexOf(root)
        val (preL, preR) = splitByPivot(rest, pivot, false)
        val (inL, inR) = splitByPivot(in, pivot, true)
        
        Some(Node(root, solve(preL, inL), solve(preR, inR)))
      }
      case Nil => None 
    }
  }
  
  def splitByPivot(l: List[String], pivotIx: Int, dropPivot: Boolean): (List[String], List[String]) = {
    val (left, right) = l.splitAt(pivotIx)
    (left, if (dropPivot) right.drop(1) else right)
  }
}

Tests.runSample1()
