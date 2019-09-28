using System.IO;
using Serilog;
using Serilog.Formatting.Json;
using Serilog.Sinks.SystemConsole.Themes;

namespace TxTraktor
{
    public class Logger : ILogger
    {
        private readonly Serilog.Core.Logger _logger;
        public Logger(bool debugMode = false, string txtFilePath = null, string jsonFilePath = null)
        {
            var logConfig = new LoggerConfiguration()
                .WriteTo.Console(theme: ConsoleTheme.None);
            
            if (txtFilePath != null && File.Exists(txtFilePath))
                File.Delete(txtFilePath);
                    
            if (txtFilePath != null)
                logConfig.WriteTo.File(txtFilePath);
                    
            if (jsonFilePath != null && File.Exists(jsonFilePath))
                File.Delete(jsonFilePath);
                    
            if (jsonFilePath != null)
                logConfig.WriteTo.File(new JsonFormatter(), jsonFilePath);

            if (debugMode)
                logConfig.MinimumLevel.Debug();
            else
                logConfig.MinimumLevel.Information();
            
            _logger = logConfig.CreateLogger();
        }

        public void Debug(string template, params object[] items)
        {
            _logger.Debug(template, items);
        }

        public void Information(string template, params object[] items)
        {
            _logger.Information(template, items);
        }

        public void Warning(string template, params object[] items)
        {
            _logger.Warning(template, items);
        }

        public void Error(string template, params object[] items)
        {
            _logger.Error(template, items);
        }
    }
}