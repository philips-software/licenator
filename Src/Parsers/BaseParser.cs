using System.Collections.Generic;
using System.IO;

namespace Licenator
{
    public interface IParser
    {
        bool SupportsFile(string filepath);

        PackageInfo[] Parse(string filepath);
    }

    public abstract class LineBasedParser : IParser
    {
        public abstract bool SupportsFile(string filepath);

        protected abstract PackageInfo ParseLine(string line);

        public PackageInfo[] Parse(string filepath)
        {
            var result = new List<PackageInfo>();
            var lines = File.ReadAllLines(filepath);

            foreach (var line in lines)
            {
                var r = ParseLine(RemoveWhitespaces(line));

                if (r != null)
                {
                    result.Add(r);
                }
            }

            return result.ToArray();
        }

        private string RemoveWhitespaces(string line)
        {
            return line.Trim();
        }
    }
}