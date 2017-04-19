using System;
using CanonEq.Lib;

namespace CanonEq.App
{
    public class InteractiveMode : IMode
    {
        public void Invoke()
        {
            Console.WriteLine("(To exit press Ctrl+C)");

            while (true)
            {
                Console.WriteLine();
                Console.Write("Enter equation: ");
                string line = Console.ReadLine();

                Equation equation;
                if (Equation.TryParse(line, out equation))
                {
                    Console.WriteLine("Canonical form: " + equation.ToCanonicalForm());
                }
                else
                {
                    Console.WriteLine("Cannot parse equation. Please make sure it has valid format.");
                }
            }
        }
    }
}