namespace Licenator
{
    public class ConfigParser : BaseParser
    {
        public override bool SupportsFile(string filepath)
        {
            return filepath.ToLowerInvariant().EndsWith("config");
        }

        public override PackageInfo[] Parse(string filepath)
        {
            throw new System.NotImplementedException();
        }
    }
}
