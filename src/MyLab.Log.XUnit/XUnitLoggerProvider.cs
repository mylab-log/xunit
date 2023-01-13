using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using Xunit.Abstractions;

namespace MyLab.Log.XUnit
{
    class XUnitLoggerProvider : ILoggerProvider, ISupportExternalScope
    {
        private readonly ITestOutputHelper _xUnitOutput;
        private IExternalScopeProvider _externalScopeProvider;
        private readonly ConsoleFormatter _formatter;
        private readonly string _configFormatterName;

        public XUnitLoggerProvider(ITestOutputHelper xUnitOutput, 
            IOptions<ConsoleLoggerOptions> opts, 
            IEnumerable<ConsoleFormatter> consoleFormatters)
        {
            _xUnitOutput = xUnitOutput;
            _configFormatterName = opts.Value.FormatterName ?? ConsoleFormatterNames.Simple;
            _formatter = consoleFormatters.FirstOrDefault(f => f.Name == _configFormatterName);
        }

        public ILogger CreateLogger(string categoryName)
        {
            if (_formatter == null)
                throw new InvalidOperationException($"Formatter '{_configFormatterName}' not found");

            return new XUnitLogger(categoryName, _xUnitOutput, _formatter)
            {
                ExternalScopeProvider = _externalScopeProvider
            };
        }

        public void SetScopeProvider(IExternalScopeProvider scopeProvider)
        {
            _externalScopeProvider = scopeProvider;
        }

        public void Dispose()
        {

        }
    }
}