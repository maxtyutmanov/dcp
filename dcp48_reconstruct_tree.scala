
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
    val s1 = Solution.solve(List("a", "b", "d", "e", "c", "f", "g"), List("d", "b", "e", "a", "f", "c", "g")) 
    println(s1)
    println(s1.map(x => x.preOrder).getOrElse(Nil))
    println(s1.map(x => x.inOrder).getOrElse(Nil))
  }
  
  def runAutotests(): Unit = {
    val childProb = 0.9
    val rand = new scala.util.Random
    val results = (0 to 10).map(_ => autoTest(rand, childProb))
    val texts = results.map(x => {
      val (ok, text) = x
      val okStr = if (ok) "[OK]" else "[FAIL]"
      s"${okStr} ${text}"
    })
    texts.foreach(t => println(t))
  }
  
  def autoTest(rand: scala.util.Random, childProb: Double): (Boolean, String) = {
    val values = "abcdefghijklmnopqrstwyxz".map(c => c.toString).toList
    val (randTree, _) = Tree.genRandom(rand, childProb, values)
    val (ok, text) = randTree.map(tree => {
      val actual = Solution.solve(tree.preOrder, tree.inOrder).get.toString
      val expected = tree.toString
      
      if (actual == expected) (true, s"Expected=Actual\r\n${actual}")
      else (false, s"\r\nE: ${expected}\r\nA: ${actual}")
    }).getOrElse((true, "Empty test case"))
    (ok, text)
  }
}

case class Node[A](value: A, left: Option[Node[A]], right: Option[Node[A]]) {
  override def toString(): String = {
    s"(${value}, ${left.map(l => l.toString).getOrElse("")}, ${right.map(l => l.toString).getOrElse("")})"
  }
  
  def preOrder(): List[A] = traverse((v, leftRes, rightRes) => v::leftRes ++ rightRes)
  
  def inOrder(): List[A] = traverse((v, leftRes, rightRes) => leftRes ++ List(v) ++ rightRes)
  
  private def traverse(combine: (A, List[A], List[A]) => List[A]): List[A] = {
    val leftRes = left.map(l => l.traverse(combine)).getOrElse(Nil)
    val rightRes = right.map(r => r.traverse(combine)).getOrElse(Nil)
    combine(value, leftRes, rightRes)
  }
}

object Tree {
  def genRandom(
      rand: scala.util.Random, 
      childProb: Double, 
      values: List[String]): (Option[Node[String]], List[String]) = {
    
    if (rand.nextDouble() < childProb) {
      values match {
        case rootVal::rest => {
          val (left, valuesAfterLeft) = genRandom(rand, childProb, rest)
          val (right, valuesAfterRight) = genRandom(rand, childProb, valuesAfterLeft)
          
          (Some(Node(rootVal, left, right)), valuesAfterRight)
        }
        case _ => (None, values)
      }
    }
    else (None, values)
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
