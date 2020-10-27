import dcp57.StringChopper
import org.scalacheck.{Gen, Prop}
import org.scalatest.FunSuite
import org.scalatest.Matchers._
import org.scalatest.prop.GeneratorDrivenPropertyChecks

class RotatedArrayBinSearchTestDcp58 extends FunSuite with GeneratorDrivenPropertyChecks {
  test("dcp58_should_find_the_index_if_no_rotations") {
    forAll(Gen.choose(1, 10000)) { (size) =>
      val xs = getOrderedListOfUniqueIntegers(size).toArray
      val ix = Gen.choose(0, xs.length - 1).sample.get
      val target = xs(ix)
      dcp58.Solution.solve(xs, target).shouldBe(Some(ix))
    }
  }

  test("dcp58_should_find_array_start") {
    forAll(Gen.choose(1, 10000), Gen.choose(1, 100000)) { (size, rotateTimes) =>
      val xsList = getOrderedListOfUniqueIntegers(size)
      val xs = rotateRight(xsList, rotateTimes).toArray
      dcp58.Solution.findArrayStart(xs, 0, xs.length - 1).shouldBe(rotateTimes % xs.length)
    }
  }

  test("dcp58_should_find_array_start_for_3_elements_array") {
    val xs = Array(13, 41, 0);
    dcp58.Solution.findArrayStart(xs, 0, 2).shouldBe(2)
  }

  test("dcp58_should_find_the_index_if_rotated") {
    val int = Gen.choose(1, 10000)

    forAll(int, int, int) { (size: Int, rotateTimes: Int, targetIx: Int) =>
      whenever(size > 0 && rotateTimes > 0 && targetIx < size) {
        val xsList = getOrderedListOfUniqueIntegers(size)
        val xs = rotateRight(xsList, rotateTimes).toArray
        val target = xs(targetIx)
        dcp58.Solution.solve(xs, target).shouldBe(Some(targetIx))
      }
    }
  }

  private def getOrderedListOfUniqueIntegers(size: Int): List[Int] = (1 to size).toList

  private def rotateRight(xs: List[Int], times: Int): List[Int] = {
    val t = times % xs.length
    val (begin, end) = xs.splitAt(xs.length - t)
    end ++ begin
  }
}
