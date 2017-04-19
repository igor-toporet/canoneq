using CanonEq.App;
using FakeItEasy;
using Xunit;

namespace CanonEq.Test.App
{
    public class ProgramTests
    {
        private readonly IModeDetector _modeDetector = A.Fake<IModeDetector>();
        private readonly Program _program;

        public ProgramTests()
        {
            _program = new Program(_modeDetector);
        }

        [Fact]
        public void Run_calls_ModeDetector_with_args()
        {
            var args = new[] {"one", "two"};

            _program.Run(args);

            A.CallTo(() => _modeDetector.DetectMode(args))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public void Run_invokes_detected_mode()
        {
            var mode = A.Fake<IMode>();
            string[] args = {"doesn't matter"};
            A.CallTo(() => _modeDetector.DetectMode(args)).Returns(mode);

            _program.Run(args);

            A.CallTo(() => mode.Invoke())
                .MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}