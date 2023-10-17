using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyLab.Log.XUnit;
using Xunit.Abstractions;
using Xunit;
using Microsoft.Extensions.Logging.Console;

namespace UnitTests
{

    public class Demo
    {
        private ITestOutputHelper _output;

        public Demo(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void DemoOutput()
        {
            var appServices = new ServiceCollection()
                .AddLogging(l => 
                        l.AddXUnit(_output)
                    )
                .BuildServiceProvider();

            var logger = appServices.GetRequiredService<ILogger<Demo>>();

            logger.LogInformation("Foo");

            Assert.True(false); //to see a Log
        }

        [Fact]
        public void DemoFormatting()
        {
            var appServices = new ServiceCollection()
                .AddLogging(l => l.AddXUnit(_output))
                .Configure<ConsoleLoggerOptions>(o => o.FormatterName = ConsoleFormatterNames.Systemd)
                .BuildServiceProvider();

            var logger = appServices.GetRequiredService<ILogger<Demo>>();

            logger.LogInformation("Foo");

            Assert.True(false); //to see a Log
        }
    }
}
