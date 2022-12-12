using Hw2;

namespace Hw2_Console
{
    public class Program
    {
        private static double result;
        public static void Main(string[] args)
        {
            Hw2.Parser.ParseCalcArguments(args, out var arg1, out var operation, out var arg2);
            result = Hw2.Calculator.Calculate(arg1, operation, arg2);
            Console.WriteLine(result);
        }

        public static double GetResult()
        {
            return result;
        }
    } 
}


