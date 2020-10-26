import dcp57.StringChopper
import org.scalacheck.Gen
import org.scalatest.Matchers._
import org.scalatest.FunSuite
import org.scalatest.prop.GeneratorDrivenPropertyChecks

class StringChopperDcp57Test extends FunSuite with GeneratorDrivenPropertyChecks {
  test("dcp57_should_not_lose_words") {
    val nonEmptyStr = Gen.alphaNumStr.suchThat(_.length > 0)

    forAll(Gen.nonEmptyListOf(nonEmptyStr)) { (words) =>
      val maxLength = words.toStream.map(_.length).max
      val text = words.mkString(" ")
      val linesOpt = StringChopper.chop(text, maxLength)
      linesOpt.map(lines => {
        lines.mkString(" ").shouldEqual(text)
      }).getOrElse(false)
    }
  }

  test("dcp57_each_line_should_not_be_longer_than_limit") {
    val nonEmptyStr = Gen.alphaNumStr.suchThat(_.length > 0)

    forAll(Gen.nonEmptyListOf(nonEmptyStr)) { (words) =>
      val maxLength = words.toStream.map(_.length).max
      val text = words.mkString(" ")
      val linesOpt = StringChopper.chop(text, maxLength)
      linesOpt.map(lines => {
        lines.forall(_.length <= maxLength)
      }).getOrElse(false)
    }
  }

  test("dcp57_should_return_none_if_not_possible") {
    val nonEmptyStr = Gen.alphaNumStr.suchThat(_.length > 0)

    forAll(Gen.nonEmptyListOf(nonEmptyStr)) { (words) =>
      val maxLength = words.toStream.map(_.length).max
      val text = words.mkString(" ")
      val linesOpt = StringChopper.chop(text, maxLength - 1)
      linesOpt eq None
    }
  }
}
