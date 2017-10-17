using System;
using Calc.Managers;

namespace Calc
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Write("Введите выражение: ");
                string expr = Console.ReadLine();
                expr = expr.Replace('.', ',');
                Calculator calculator = new Calculator(expr);
                Console.Write($"{expr} = {calculator.Run()}\nДля выхода нажмите Q, что бы продолжить, нажмите Enter:\n");
                var key = Console.ReadKey();
                if(key.KeyChar=='q'||key.KeyChar=='й')break;
            }
        }
    }
}
