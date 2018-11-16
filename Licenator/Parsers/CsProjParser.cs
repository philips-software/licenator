using System.Linq;

namespace Licenator
{
    public class CsProjParser : LineBasedParser
    {
        public override bool SupportsFile(string filepath)
        {
            return filepath.ToLowerInvariant().EndsWith("csproj");
        }

        protected override PackageInfo ParseLine(string filepath, string line)
        {
            if (line.StartsWith("<PackageReference") &&
                line.Contains("Include=") &&
                line.Contains("Version="))
            {
                return CreatePackageInfo(filepath, line);
            }

            return null;
        }

        private PackageInfo CreatePackageInfo(string filepath, string line)
        {
            var tags = XmlTagParser.Parse(line);

            var idTag = tags.SingleOrDefault(t => t.Name == "Include");
            var versionTag = tags.SingleOrDefault(t => t.Name == "Version");

            if (idTag == null || versionTag == null) return null;

            return new PackageInfo
            {
                Name = idTag.Value,
                Version = versionTag.Value,
                UsedIn = filepath
            };
        }
    }
}
