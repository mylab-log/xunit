using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Moq;
using MyLab.Log.XUnit;
using Xunit.Abstractions;
using Xunit;

namespace UnitTests
{
    public class LoggerBehavior
    {
        [Fact]
        public void ShouldWriteLogToOutput()
        {
            //Arrange
            string? outputMessage = null;

            var outputMock = new Mock<ITestOutputHelper>();
            outputMock
                .Setup(o => o.WriteLine(It.IsAny<string>()))
                .Callback<string>(msg => outputMessage = msg);

            var srv = new ServiceCollection()
                .AddLogging(l => l.AddXUnit(outputMock.Object))
                .BuildServiceProvider();

            var logger = srv.GetRequiredService<ILogger<LoggerBehavior>>();

            //Act
            logger.LogInformation("Foo");

            //Assert
            Assert.StartsWith($"[40m[32minfo[39m[22m[49m: UnitTests.{nameof(LoggerBehavior)}[0]", outputMessage);
            Assert.Contains("Foo", outputMessage);
        }

        [Fact]
        public void ShouldUseConsoleFormatter()
        {
            //Arrange
            string? outputMessage = null;

            var outputMock = new Mock<ITestOutputHelper>();
            outputMock
                .Setup(o => o.WriteLine(It.IsAny<string>()))
                .Callback<string>(msg => outputMessage = msg);

            var srv = new ServiceCollection()
                .AddLogging(l => l.AddXUnit(outputMock.Object))
                .Configure<ConsoleLoggerOptions>(o => o.FormatterName = ConsoleFormatterNames.Systemd)
                .BuildServiceProvider();

            var logger = srv.GetRequiredService<ILogger<LoggerBehavior>>();

            //Act
            logger.LogInformation("Foo");

            //Assert
            Assert.NotNull(outputMessage);
            Assert.Equal($"<6>UnitTests.{nameof(LoggerBehavior)}[0] Foo", outputMessage!.TrimEnd());
        }
    }
}