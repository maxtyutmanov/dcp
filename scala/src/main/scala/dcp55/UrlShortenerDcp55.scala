package dcp55

import scala.collection.mutable

/*
This problem was asked by Microsoft.

Implement a URL shortener with the following methods:

shorten(url), which shortens the url into a six-character alphanumeric string, such as zLg6wl.
restore(short), which expands the shortened string into the original url. If no such shortened string exists, return null.
Hint: What if we enter the same URL twice?
 */

object Tests {
  def runSample1(): Unit = {
    val sut = new UrlShortener
    val handle = sut.shorten("")

  }
}

class UrlShortener {
  private val _handleSize = 6
  private val _map = new mutable.HashMap[String, String]();
  private val _alphabet: Array[Char] = createAlphabet
  private val _maxHashcode = scala.math.pow(_alphabet.length, _handleSize).toLong

  def shorten(url: String): String = {
    val handle = getHandle(url)
    _map.put(handle, url)
    handle
  }

  def restore(handle: String): Option[String] = _map.get(handle)

  private def getHandle(url: String): String = {
    val hc = url.hashCode.toLong
    val boundedHc = if (hc < 0) (hc + Int.MaxValue) % _maxHashcode else hc % _maxHashcode
    hashCodeToHandle(boundedHc)
  }

  private def hashCodeToHandle(hc: Long): String = {
    val alphabetSize = _alphabet.length
    val digits = Stream
      .iterate((hc, 0L))(x => (x._1 / alphabetSize, x._1 % alphabetSize))
      .drop(1)
      .takeWhile(x => x._1 > 0 || x._2 > 0)
      .reverse
      .map(_._2.toInt)
      .map(_alphabet(_))
      .toList

    digits.padTo(_handleSize, '0').mkString
  }

  private def createAlphabet: Array[Char] = {
    val lowercase = Stream.from('a').takeWhile(_ <= 'z')
    val uppercase = Stream.from('A').takeWhile(_ <= 'Z')
    val digits = Stream.from('0').takeWhile(_ <= '9')
    val alphabet = Stream.concat(lowercase, uppercase, digits).map(_.toChar)
    alphabet.toArray
  }
}

class UrlShortenerDcp55 {

}
