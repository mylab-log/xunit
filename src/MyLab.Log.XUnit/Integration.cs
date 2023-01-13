using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace MyLab.Log.XUnit
{
    /// <summary>
    /// Contains extensions method for integration
    /// </summary>
    public static class Integration
    {
        /// <summary>
        /// Adds XUnit logger
        /// </summary>
        public static ILoggingBuilder AddXUnit(this ILoggingBuilder loggingBuilder, ITestOutputHelper xUnitOutput)
        {
            loggingBuilder.AddConsole();
            loggingBuilder.Services.AddSingleton<ILoggerProvider, XUnitLoggerProvider>();
            loggingBuilder.Services.AddSingleton(xUnitOutput);

            return loggingBuilder;
        }
    }
}
