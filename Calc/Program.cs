using System;

namespace Calc
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            while (true)
            {
                Console.Write("Write your expression: ");
                string expr = Console.ReadLine();
                Calculator calc = new Calculator(expr);
                try
                {
                    int res = calc.Calculate();
                    Console.WriteLine($"{expr} = {res}");
                }
                catch (InvalidSyntaxException e)
                {
                    Console.WriteLine($"Error: {e.Message}");
                }
                Console.WriteLine("To continue press Enter. For exit press Escape");
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape)
                    break;
            }
        }
    }
}