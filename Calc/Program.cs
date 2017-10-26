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
                expr = expr.Replace(',', '.');
                Calculator calculator = new Calculator(expr);
                Console.WriteLine($"{expr} = {calculator.Calculate()}");
                Console.WriteLine("To continue press Enter. For exit press Q");
                var key = Console.ReadKey();
                if (key.KeyChar == 'q' || key.KeyChar == 'й')
                    break;
            }
        }
    }
}