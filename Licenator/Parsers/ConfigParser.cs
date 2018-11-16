using System.Collections.Generic;
using System.Linq;

namespace Licenator
{
    public class ConfigParser : LineBasedParser
    {
        public override bool SupportsFile(string filepath)
        {
            return filepath.ToLowerInvariant().EndsWith("config");
        }

        protected override PackageInfo ParseLine(string filepath, string line)
        {
            if (line.StartsWith("<package") &&
                line.Contains("id=") &&
                line.Contains("version="))
            {
                return CreatePackageInfo(filepath, line);
            }

            return null;
        }

        private PackageInfo CreatePackageInfo(string filepath, string line)
        {
            var tags = XmlTagParser.Parse(line);

            var idTag = tags.SingleOrDefault(t => t.Name == "id");
            var versionTag = tags.SingleOrDefault(t => t.Name == "version");

            if (idTag == null || versionTag == null) return null;

            return new PackageInfo
            {
                Name = idTag.Value,
                Version = versionTag.Value,
                UsedIn = new List<string>{filepath}
            };
        }
    }
}
