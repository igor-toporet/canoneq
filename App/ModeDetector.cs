namespace CanonEq.App
{
    public class ModeDetector : IModeDetector
    {
        public IMode DetectMode(string[] args)
        {
            switch (args.Length)
            {
                case 0:
                    return new InteractiveMode();

                case 1:
                    return new FileMode(args[0]);

                default:
                    return new NotSupportedMode(args.Length);
            }
        }
    }
}