import dcp55.UrlShortener
import org.scalacheck.Gen
import org.scalatest.FunSuite
import org.scalatest.Matchers._
import org.scalatest.prop.GeneratorDrivenPropertyChecks

class UrlShortenerDcp55Test extends FunSuite with GeneratorDrivenPropertyChecks {
  test("dcp55_should_restore_original_str") {
    forAll(Gen.asciiStr) { (original) =>
      val sut = new UrlShortener
      val handle = sut.shorten(original)
      val restored = sut.restore(handle)
      restored.get shouldEqual original
    }
  }

  test("dcp55_handle_should_be_six_chars") {
    forAll(Gen.asciiStr) { (original) =>
      val sut = new UrlShortener
      val handle = sut.shorten(original)
      handle.length shouldBe 6
    }
  }

  test("dcp55_should_restore_original_str_reuse_shortener") {
    val sut = new UrlShortener
    forAll(Gen.asciiStr) { (original) =>
      val handle = sut.shorten(original)
      val restored = sut.restore(handle)
      restored.get shouldEqual original
    }
  }
}
