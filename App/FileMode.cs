using System;
using System.IO;
using System.Linq;
using CanonEq.Lib;

namespace CanonEq.App
{
    public class FileMode : IMode
    {
        public FileMode(string inputFile)
        {
            InputFile = inputFile;
        }

        public string InputFile { get; }

        private string OutputFile => InputFile + ".out";


        public void Invoke()
        {
            var totalLinesProcessed = 0;

            var inputLines = File.ReadAllLines(InputFile);

            var outputLines = inputLines.Select((input, i) =>
            {
                if (Equation.TryParse(input, out Equation equation))
                {
                    totalLinesProcessed++;
                    return equation.ToCanonicalForm().ToString();
                }
                string message = $"Failed to parse line {i}: {input}";
                throw new Exception(message);
            });

            File.WriteAllLines(OutputFile, outputLines);

            Console.WriteLine($"Processed {totalLinesProcessed} lines.");
        }
    }
}