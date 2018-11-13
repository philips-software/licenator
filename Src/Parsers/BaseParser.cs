namespace Licenator
{
    public interface IParser
    {
        bool SupportsFile(string filepath);

        PackageInfo[] Parse(string filepath);
    }

    public abstract class BaseParser : IParser
    {
        public abstract bool SupportsFile(string filepath);

        public abstract PackageInfo[] Parse(string filepath);
    }
}