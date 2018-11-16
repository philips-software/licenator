
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;

namespace Licenator
{
    public class LicenseResolver
    {
        public void ResolveAll(PackageList packageList)
        {
            var packages = packageList.GetInfos();

            foreach (var package in packages)
            {
                SetLicenseUrlOnPackage(package);
            }
        }

        private void SetLicenseUrlOnPackage(PackageInfo package)
        {
            try
            {
                package.LicenseUrl = GetLicenseUrlForProject(package.Name, package.Version, package.UsedIn);
            }
            catch (Exception exception)
            {
                Console.WriteLine("[!] Failed to fetch package metadata for '" + 
                    package.Name + "' with version '" + package.Version + "'. (Used in: " +  package.UsedIn + ")");
                Console.WriteLine("Exception details: " + exception);

                package.LicenseUrl = null;
            }
        }

        private string GetLicenseUrlForProject(string projectName, string version, string usedIn)
        {
            using (var httpClient = new HttpClient())
            {
                var uri = new Uri("https://api.nuget.org/v3/registration3/" + projectName.ToLowerInvariant() + "/index.json");
                var task = httpClient.GetAsync(uri);
                task.Wait();

                var result = task.Result;
                if (result.StatusCode != HttpStatusCode.OK) throw new Exception("StatusCode: " + result.StatusCode);
                var str = result.Content.ReadAsStringAsync().Result;

                var data = JsonConvert.DeserializeObject<NuGetMetadata>(str);

                var entries = data.Items.SelectMany(i => i.Items).Select(ii => ii.CatalogEntry).ToArray();

                var entriesOfCorrectVersion = entries.Where(i => i.Version.ToLowerInvariant() == version.ToLowerInvariant()).ToArray();

                if (!entriesOfCorrectVersion.Any())
                {
                    throw new Exception("Unable to find NuGet catalog entry for '" + projectName + "' with version '" + version + "'. (Used in: " + usedIn + ")");
                }

                var licenses = entriesOfCorrectVersion.Select(i => i.LicenseUrl).Distinct();

                if (licenses.Count() > 1)
                {
                    throw new Exception("Unable to handle package: Found multiple licenses for '" + projectName + "' with version '" + version + "'. (Used in: " + usedIn + ")");
                }

                return licenses.Single();
            }
        }
    }
}
