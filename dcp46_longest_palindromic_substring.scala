object Tests {
  def runSample1() = {
    println("Should be anana: " + Solution.solve("bananas"))
    println("Should be bcdcb: " + Solution.solve("aabcdcb"))
    println("Should be mananam: " + Solution.solve("bananas_sdr_mananam"))
    println("Should be mananam: " + Solution.solve("mananam_sdr_bananas"))
    println(Solution.solve("babaddtattarrattatddetartrateedredividerb"))
    println(Solution.solve("anugnxshgonmqydttcvmtsoaprxnhpmpovdolbidqiyqubirkvhwppcdyeouvgedccipsvnobrccbndzjdbgxkzdbcjsjjovnhpnbkurxqfupiprpbiwqdnwaqvjbqoaqzkqgdxkfczdkznqxvupdmnyiidqpnbvgjraszbvvztpapxmomnghfaywkzlrupvjpcvascgvstqmvuveiiixjmdofdwyvhgkydrnfuojhzulhobyhtsxmcovwmamjwljioevhafdlpjpmqstguqhrhvsdvinphejfbdvrvabthpyyphyqharjvzriosrdnwmaxtgriivdqlmugtagvsoylqfwhjpmjxcysfujdvcqovxabjdbvyvembfpahvyoybdhweikcgnzrdqlzusgoobysfmlzifwjzlazuepimhbgkrfimmemhayxeqxynewcnynmgyjcwrpqnayvxoebgyjusppfpsfeonfwnbsdonucaipoafavmlrrlplnnbsaghbawooabsjndqnvruuwvllpvvhuepmqtprgktnwxmflmmbifbbsfthbeafseqrgwnwjxkkcqgbucwusjdipxuekanzwimuizqynaxrvicyzjhulqjshtsqswehnozehmbsdmacciflcgsrlyhjukpvosptmsjfteoimtewkrivdllqiotvtrubgkfcacvgqzxjmhmmqlikrtfrurltgtcreafcgisjpvasiwmhcofqkcteudgjoqqmtucnwcocsoiqtfuoazxdayricnmwcg"))
  }
}

object Solution {
  private case class SubString(s: String, from: Int, to: Int) {
    def first = s(from)
    def last = s(to - 1)
    def length = to - from
    
    def canGrowBothWaysAndStayPalindromic = from > 0 && to < s.length && s(from - 1) == s(to)
    def growBothWays = SubString(s, from - 1, to + 1)
    
    override def toString = s.substring(from, to)
  }
  
  def solve(s: String): String = {
    if (s.isEmpty) s
    else {
      val ixs = for {
        left <- 0 to s.length - 1
        right <- left + 1 to math.min(left + 2, s.length)
      } yield (left, right)
      
      val pals = ixs
        .map(x => x match { case (left, right) => SubString(s, left, right)})
        .filter(ss => ss.first == ss.last)
      
      subSolve(pals).maxBy(ss => ss.length).toString
    }
  }
  
  @annotation.tailrec
  private def subSolve(pals: Seq[SubString]): Seq[SubString] = {
    val next = pals
      .filter(p => p.canGrowBothWaysAndStayPalindromic)
      .map(p => p.growBothWays)
    if (next.isEmpty) pals else subSolve(next)
  }
}

Tests.runSample1()
