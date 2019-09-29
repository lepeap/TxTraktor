using System;
using System.Collections.Generic;
using System.IO;

namespace TxTraktor.Extension
{
    public class TxtFileExtension : IExtension
    {
        private string _rootPath;
        public TxtFileExtension(string rootPath)
        {
            _rootPath = rootPath;
        }
        public string Name => "txt";
        public IEnumerable<Dictionary<string, string>> Process(string filePath)
        {
            if (!string.IsNullOrWhiteSpace(_rootPath) && !Path.IsPathRooted(filePath))
            {
                filePath = Path.Combine(_rootPath, filePath);
            }

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Extension text file '{filePath}' not found.");
            }

            foreach (var value in File.ReadAllLines(filePath))
            {
                yield return new Dictionary<string, string>()
                {
                    {"value", value},
                    {"value_id", value}
                };

            }
        }
    }
}