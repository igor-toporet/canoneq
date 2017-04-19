using CanonEq.App;
using FluentAssertions;
using Xunit;

namespace CanonEq.Test.App
{
    public class ModeDetectorTests
    {
        private readonly ModeDetector _modeDetector = new ModeDetector();

        [Fact]
        public void DetectMode_with_empty_args_returns_InteractiveMode()
        {
            var emptyArgs = new string[0];

            _modeDetector.DetectMode(emptyArgs)
                .Should().BeOfType<InteractiveMode>();
        }

        [Fact]
        public void DetectMode_with_one_arg_returns_FileMode_with_arg_as_input_file()
        {
            const string fileName = "any";
            var args = new[] {fileName};

            _modeDetector.DetectMode(args)
                .Should().BeOfType<FileMode>()
                .Which
                .InputFile.Should().Be(fileName);
        }

        [Fact]
        public void DetectMode_with_more_than_one_arg_returns_NotSupportedMode_with_number_of_args()
        {
            var args = new[] {"one", "two"};

            _modeDetector.DetectMode(args)
                .Should().BeOfType<NotSupportedMode>()
                .Which
                .NumberOfArgs.Should().Be(args.Length);
        }
    }
}