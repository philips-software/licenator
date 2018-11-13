namespace Licenator
{
    public class ConfigParser : LineBasedParser
    {
        public override bool SupportsFile(string filepath)
        {
            return filepath.ToLowerInvariant().EndsWith("config");
        }

        protected override PackageInfo ParseLine(string line)
        {
            return null;
        }
    }
}
