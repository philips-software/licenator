using System.Collections.Generic;
using System.IO;

namespace Licenator
{
    public class NuGetParser
    {
        private readonly List<IParser> _parsers = new List<IParser>();

        public NuGetParser()
        {
            _parsers.Add(new CsProjParser());
            _parsers.Add(new ConfigParser());
        }

        public bool SupportsFile(string filepath)
        {
            return (SelectParser(filepath) != null);
        }

        public PackageInfo[] ProcessFile(string filepath)
        {
            var parser = SelectParser(filepath);
            return parser.Parse(filepath);
        }

        private IParser SelectParser(string filepath)
        {
            var extension = Path.GetExtension(filepath);
            foreach (var parser in _parsers)
            {
                if (parser.SupportsFile(filepath))
                {
                    return parser;
                }
            }

            return null;
        }
    }
}