using System;

namespace dcp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("For 2x2 matrix: {0}", Dcp62.GetNumberOfOptions(2, 2));
            Console.WriteLine("For 5x5 matrix: {0}", Dcp62.GetNumberOfOptions(5, 5));
            Console.WriteLine("For 100x100 matrix: {0}", Dcp62.GetNumberOfOptions(100, 100));
            Console.ReadLine();
        }
    }
}
