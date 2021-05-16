using System;

namespace dcp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("2^10 = {0}", Dcp61.Pow(2, 10));
            Console.WriteLine("3^15 = {0}", Dcp61.Pow(3, 15));
            Console.WriteLine("2^31 = {0}", Dcp61.Pow(2, 31));
            Console.WriteLine("2^57 = {0}", Dcp61.Pow(2, 57));
            Console.WriteLine("1^1577 = {0}", Dcp61.Pow(1, 1577));
            Console.WriteLine("1^157700 = {0}", Dcp61.Pow(1, 157700));

            Console.ReadLine();
        }
    }
}
