using System;

namespace CanonEq.App
{
    public class Program
    {
        private readonly IModeDetector _modeDetector;

        public Program(IModeDetector modeDetector)
        {
            _modeDetector = modeDetector;
        }

        public void Run(string[] args)
        {
            var mode = _modeDetector.DetectMode(args);
            mode.Invoke();
        }

        private static int Main(string[] args)
        {
            try
            {
                new Program(new ModeDetector()).Run(args);

                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return -1;
            }
        }
    }
}