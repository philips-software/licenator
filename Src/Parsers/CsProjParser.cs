namespace Licenator
{
    public class CsProjParser : BaseParser
    {
        public override bool SupportsFile(string filepath)
        {
            return filepath.ToLowerInvariant().EndsWith("csproj");
        }

        public override PackageInfo[] Parse(string filepath)
        {
            throw new System.NotImplementedException();
        }
    }
}
