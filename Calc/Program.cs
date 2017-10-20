namespace Calc
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            while (true)
            {
                System.Console.Write("Write your expression: ");
                string expr = System.Console.ReadLine();
                expr = expr.Replace(',', '.');
                Validator validator = new Validator(expr);
                if (validator.IsValid())
                {
                    Calculator calculator = new Calculator(expr);
                    System.Console.WriteLine($"{expr} = {calculator.Run()}");
                    System.Console.WriteLine("To continue press Enter. For exit press Q");
                }
                else
                {
                    System.Console.WriteLine("Invalid expression");
                    System.Console.WriteLine("To continue press Enter. For exit press Q");
                }
                var key = System.Console.ReadKey();
                if (key.KeyChar == 'q' || key.KeyChar == 'й')
                    break;
            }
        }
    }
}