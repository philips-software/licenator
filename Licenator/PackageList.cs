using System.Collections.Generic;
using System.Linq;

namespace Licenator
{
    public class PackageList
    {
        private readonly List<PackageInfo> _infos = new List<PackageInfo>();

        public void Add(IEnumerable<PackageInfo> infos)
        {
            // Add new packages if they don't exist.
            // If they do, add the UsedIn information if it doesn't exist.
            foreach (var i in infos)
            {
                var existing = GetPackageIfExists(i.Name, i.Version);
                if (existing != null)
                {
                    existing.UsedIn = existing.UsedIn.CombineUnique(i.UsedIn).ToList();
                }
                else
                {
                    _infos.Add(i);
                }
            }
        }

        public void IgnorePackages(List<string> packagesToIgnore)
        {
            _infos.RemoveAll(i => packagesToIgnore.Contains(i.Name));
        }

        public IEnumerable<PackageInfo> GetInfos()
        {
            return _infos;
        }

        public IEnumerable<LicenseSummary> GetSummary()
        {
            // Gather all unique licenses.
            // For each license, list the packages that have this license and where it is used.

            var result = new List<LicenseSummary>();

            foreach (var i in _infos)
            {
                if (string.IsNullOrEmpty(i.LicenseUrl)) continue;

                AddOrUpdateSummary(result, i);
            }

            result = result.OrderBy(r => r.LicenseUrl).ToList();

            return result;
        }

        public IEnumerable<PackageInfo> GetFailedPackages()
        {
            return _infos.Where(i => string.IsNullOrEmpty(i.LicenseUrl)).OrderBy(i => i.Name + i.Version);
        }

        private void AddOrUpdateSummary(List<LicenseSummary> result, PackageInfo i)
        {
            var existingSummary = result.SingleOrDefault(r => r.LicenseUrl == i.LicenseUrl);
            if (existingSummary == null)
            {
                AddNewSummaryEntry(result, i);
            }
            else
            {
                UpdateExistingSummaryEntry(existingSummary, i);
            }
        }

        private void AddNewSummaryEntry(List<LicenseSummary> result, PackageInfo i)
        {
            result.Add(new LicenseSummary
            {
                LicenseUrl = i.LicenseUrl,
                Packages = new List<PackageSummary>
                {
                    new PackageSummary
                    {
                        Name = i.Name,
                        Version = i.Version
                    }
                },
                UsedIn = i.UsedIn
            });
        }

        private void UpdateExistingSummaryEntry(LicenseSummary summary, PackageInfo i)
        {
            if (!summary.Packages.Any(p => p.Name == i.Name))
            {
                summary.Packages.Add(new PackageSummary
                {
                    Name = i.Name,
                    Version = i.Version
                });
            }
            
            summary.UsedIn = summary.UsedIn.CombineUnique(i.UsedIn).ToList();
        }

        private PackageInfo GetPackageIfExists(string name, string version)
        {
            return _infos.SingleOrDefault(i => i.Name == name && i.Version == version);
        }
    }
}