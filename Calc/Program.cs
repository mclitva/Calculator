﻿using System;

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
                Parser tok = new Parser(expr);
                Console.WriteLine($"{expr} = {tok.Result}");
                Console.WriteLine("To continue press Enter. For exit press Escape");
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape)
                    break;
            }
        }
    }
}