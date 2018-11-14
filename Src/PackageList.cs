using System.Collections.Generic;
using System.Linq;

namespace Licenator
{
    public class PackageList
    {
        private readonly List<PackageInfo> _infos = new List<PackageInfo>();

        public void Add(IEnumerable<PackageInfo> infos)
        {
            _infos.AddRange(infos);
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

                var existingSummary = result.SingleOrDefault(r => r.LicenseUrl == i.LicenseUrl);
                if (existingSummary == null)
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
                        UsedIn = new List<string>{i.UsedIn}
                    });
                }
                else
                {
                    if (!existingSummary.Packages.Any(p => p.Name == i.Name)) existingSummary.Packages.Add(new PackageSummary
                    {
                        Name = i.Name,
                        Version = i.Version
                    });
                    if (!existingSummary.UsedIn.Contains(i.UsedIn)) existingSummary.UsedIn.Add(i.UsedIn);
                }
            }

            return result;
        }

        public IEnumerable<PackageInfo> GetFailedPackages()
        {
            return _infos.Where(i => string.IsNullOrEmpty(i.LicenseUrl));
        }
    }
}