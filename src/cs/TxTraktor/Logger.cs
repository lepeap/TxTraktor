using System.IO;
using Serilog;
using Serilog.Formatting.Json;
using Serilog.Sinks.SystemConsole.Themes;

namespace TxTraktor
{
    public class Logger : ILogger
    {
        public Logger(bool debugMode = false)
        {
            var txtFile = "log.txt";
            if (File.Exists(txtFile))
                File.Delete(txtFile);

            var jsonFile = "log.json";
            if (File.Exists(jsonFile))
                File.Delete(jsonFile);
            
            var logConfig = new LoggerConfiguration()
                .WriteTo.Console(theme: ConsoleTheme.None)
                .WriteTo.File(txtFile)
                .WriteTo.File(new JsonFormatter(), jsonFile);

            if (debugMode)
                logConfig.MinimumLevel.Debug();
            else
                logConfig.MinimumLevel.Information();
            
            Log.Logger = logConfig.CreateLogger();
        }

        public void Debug(string template, params object[] items)
        {
            Log.Debug(template, items);
        }

        public void Information(string template, params object[] items)
        {
            Log.Information(template, items);
        }

        public void Warning(string template, params object[] items)
        {
            Log.Warning(template, items);
        }

        public void Error(string template, params object[] items)
        {
            Log.Error(template, items);
        }
    }
}