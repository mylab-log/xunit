using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;
using Xunit.Abstractions;

namespace MyLab.Log.XUnit
{
    class XUnitLogger : ILogger
    {
        private readonly string _categoryName;
        private readonly ITestOutputHelper _xUnitOutput;
        private readonly ConsoleFormatter _formatter;

        public IExternalScopeProvider ExternalScopeProvider { get; set; }

        public XUnitLogger(
            string categoryName,
            ITestOutputHelper xUnitOutput, 
            ConsoleFormatter formatter)
        {
            _categoryName = categoryName;
            _xUnitOutput = xUnitOutput;
            _formatter = formatter;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            LogEntry<TState> log = new LogEntry<TState>(logLevel, _categoryName, eventId, state, exception, formatter);

            var stringBuilder = new StringBuilder();
            using var textWriter = new StringWriter(stringBuilder);
            {
                _formatter.Write(log, ExternalScopeProvider, textWriter);
            }

            try
            {
                _xUnitOutput.WriteLine(stringBuilder.ToString());
            }
            catch (InvalidOperationException e) when (e.Message == "There is no currently active test.")
            {
                //Out of tests
            }
            catch (AggregateException e) when (e.Message.Contains("There is no currently active test."))
            {
                //Out of tests
            }
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public IDisposable BeginScope<TState>(TState state) where TState : notnull
        {
            return null;
        }
    }
}