using System;
using System.IO;
using System.Reflection;

namespace CanonEq.App
{
    public class NotSupportedMode : IMode
    {
        public NotSupportedMode(int numberOfArgs)
        {
            NumberOfArgs = numberOfArgs;
        }

        public int NumberOfArgs { get; }

        public void Invoke()
        {
            PrintUsage();

            string message = $"Supplied {NumberOfArgs} arguments, expected one or zero.";
            throw new Exception(message);
        }

        private static void PrintUsage()
        {
            var executable = Assembly.GetEntryAssembly();
            var exeFileName = Path.GetFileName(executable.CodeBase);

            string usageInfo = $@"
Usage:    

    {exeFileName} <input-file.ext>  -   batch file mode, outputs to <input-file.ext>.out
    {exeFileName}                   -   interactive mode
";

            Console.WriteLine(usageInfo);
        }
    }
}