﻿using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;

namespace Licenator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var traveler = new DirectoryTraveler();

            traveler.Begin(@"C:\Data\MaskDesign\Source\VisualStudioSolution", file =>
            {
                Console.WriteLine("found file: " + file);
            });

            //var projectName = "nuget.server.core";
            //var projectName = "castle.core";
            var projectName = "Newtonsoft.Json";

            var licenses = GetUniqueLicenseUrlForProject(projectName);
        }

        private static string[] GetUniqueLicenseUrlForProject(string projectName)
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

                foreach (var item in data.Items)
                {
                    foreach (var e in item.Items)
                    {
                        var entry = e.CatalogEntry;
                        Console.WriteLine(entry.Id + " - " + entry.Title + " - " + entry.Version + " - " + entry.LicenseUrl);
                    }
                }

                var licenseUrls = data.Items
                    .SelectMany(i => i.Items.Select(ii => ii.CatalogEntry.LicenseUrl))
                    .Distinct()
                    .Where(s => !string.IsNullOrEmpty(s))
                    .ToArray();

                return licenseUrls;
            }
        }
    }
}
