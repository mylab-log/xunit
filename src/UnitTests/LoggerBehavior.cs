using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyLab.Log.XUnit;
using Xunit.Abstractions;

namespace UnitTests
{
    public class LoggerBehavior
    {
        private readonly ITestOutputHelper _output;

        public LoggerBehavior(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void ShouldWriteLogToOutput()
        {
            //Arrange
            var srv = new ServiceCollection()
                .AddLogging(l => l.AddXUnit(_output))
                .BuildServiceProvider();

            var logger = srv.GetRequiredService<ILogger<LoggerBehavior>>();

            //Act
            logger.LogInformation("Test log");

            //Assert

        }
    }
}