
/*
 *  This problem was asked by Facebook.

    Given a string of round, curly, and square open and closing brackets, return whether the brackets are balanced (well-formed).
    
    For example, given the string "([])[]({})", you should return true.
    
    Given the string "([)]" or "((()", you should return false.
 */

object Tests {
  def runSample1_balanced() {
    println("Should be true: " + Solution.solve("([])[]({})"))
    println("Should be true: " + Solution.solve("([])[(([]))]({{[]}})"))
  }
  
  def runSample2_unbalanced_interleaving() {
    println("Should be false: " + Solution.solve("([)]"))
  }
  
  def runSample3_unbalanced() {
    println("Should be false: " + Solution.solve("((()]]"))
    println("Should be false: " + Solution.solve("((()"))
  }
}

object Solution {
  def solve(s: String): Boolean = {
    val pl = ProductionsList(List(
        List(']', '['),
        List(')', '('),
        List('}', '{')))
    
    // put all characters of the string (one by one) to the stack and try to match
    // the head of this stack against the productions list
    s.foldLeft(Nil:List[Char])((stack, chr) => {
      pl.tryMatch(chr::stack)
    }).isEmpty
  }
  
  case class ProductionsList(productions: List[List[Char]]) {
    def tryMatch(stack: List[Char]): List[Char] = {
      productions.find(prod => stack.startsWith(prod)).map(matchingProd => {
        // apply production if it matches what we currently have on top of the stack
        stack.drop(matchingProd.length)
      }).getOrElse(stack)
    }
  }
}
             
Tests.runSample1_balanced
Tests.runSample2_unbalanced_interleaving
Tests.runSample3_unbalanced