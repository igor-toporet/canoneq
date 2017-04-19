namespace CanonEq.App
{
    public interface IModeDetector
    {
        IMode DetectMode(string[] args);
    }
}