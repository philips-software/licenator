using System.Collections.Generic;

namespace Licenator
{
    public class PackageInfo
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public string UsedIn { get; set; }
    }
    public class NuGetMetadata
    {
        public string Id { get; set; }
        public string[] Type { get; set; }
        public string CommitId { get; set; }
        public string CommitTimeStamp { get; set; }
        public int Count { get; set; }
        public List<NuGetMetadataItem> Items { get; set; }
        public NuGetMetadataContext Content { get; set; }
    }

    public class NuGetMetadataContext
    {
        public string Vocab { get; set; }
        public string Catalog { get; set; }
        public string Xsd { get; set; }
        public NuGetMetadataTag Items { get; set; }
        public NuGetMetadataTag CommitTimeStamp { get; set; }
        public NuGetMetadataTag CommitId { get; set; }
        public NuGetMetadataTag Count { get; set; }
        public NuGetMetadataTag Parent { get; set; }
        public NuGetMetadataTag Tags { get; set; }
        public NuGetMetadataTag PackageTargetFrameworks { get; set; }
        public NuGetMetadataTag DependencyGroups { get; set; }
        public NuGetMetadataTag Dependencies { get; set; }
        public NuGetMetadataTag PackageContent { get; set; }
        public NuGetMetadataTag Published { get; set; }
        public NuGetMetadataTag Registration { get; set; }
    }

    public class NuGetMetadataTag
    {
        public string Id { get; set; }
        public string Container { get; set; }
        public string Type { get; set; }
    }

    public class NuGetMetadataItem
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string CommitId { get; set; }
        public string CommitTimeStamp { get; set; }
        public int Count { get; set; }
        public List<MetaDataEntry> Items { get; set; }
        public string Parent { get; set; }
        public string Lower { get; set; }
        public string Upper { get; set; }
    }

    public class MetaDataEntry
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string CommitId { get; set; }
        public string CommitTimeStamp { get; set; }
        public MetaDataCatalogEntry CatalogEntry { get; set; }
        public string PackageContent { get; set; }
        public string Registration { get; set; }
    }
    
    public class MetaDataCatalogEntry
    {
        //public string Id { get; set; }
        public string Type { get; set; }
        public string Authors { get; set; }
        public string Description { get; set; }
        public string IronUrl { get; set; }
        public string Id { get; set; }
        public string Language { get; set; }
        public string LicenseUrl { get; set; }
        public bool Listed { get; set; }
        public string MinClientVersion { get; set; }
        public string PackageContent { get; set; }
        public string ProjectUrl { get; set; }
        public string Published { get; set; }
        public bool RequireLicenseAcceptance { get; set; }
        public string Summary { get; set; }
        public string[] Tags { get; set; }
        public string Title { get; set; }
        public string Version { get; set; }
    }
}
