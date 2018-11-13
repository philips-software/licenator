namespace Licenator
{
    public class CsProjParser : LineBasedParser
    {
        public override bool SupportsFile(string filepath)
        {
            return filepath.ToLowerInvariant().EndsWith("csproj");
        }

        protected override PackageInfo ParseLine(string line)
        {
            return null;
        }
    }
}
