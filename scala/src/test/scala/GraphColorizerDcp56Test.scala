import dcp56.GraphColorizer
import org.scalatest.FunSuite
import org.scalatest.Matchers.convertToAnyShouldWrapper
import org.scalatest.prop.GeneratorDrivenPropertyChecks

class GraphColorizerDcp56Test extends FunSuite with GeneratorDrivenPropertyChecks {
  test("dcp56_possible") {
    val m = Array(
      Array(0, 1, 0, 0, 0, 0, 0),
      Array(1, 0, 0, 1, 0, 1, 1),
      Array(0, 0, 0, 1, 0, 0, 0),
      Array(0, 1, 1, 0, 0, 1, 0),
      Array(0, 0, 0, 0, 0, 1, 1),
      Array(0, 1, 0, 1, 1, 0, 0),
      Array(0, 1, 0, 0, 1, 0, 0)
    ).map(_.map(x => x == 1))

    GraphColorizer.canColorize(m, 3).shouldBe(true)
  }

  test("dcp56_not_possible") {
    val m = Array(
      Array(0, 1, 0, 0, 0, 0, 0),
      Array(1, 0, 0, 1, 0, 1, 1),
      Array(0, 0, 0, 1, 0, 0, 0),
      Array(0, 1, 1, 0, 0, 1, 0),
      Array(0, 0, 0, 0, 0, 1, 1),
      Array(0, 1, 0, 1, 1, 0, 0),
      Array(0, 1, 0, 0, 1, 0, 0)
    ).map(_.map(x => x == 1))

    GraphColorizer.canColorize(m, 2).shouldBe(false)
  }
}
