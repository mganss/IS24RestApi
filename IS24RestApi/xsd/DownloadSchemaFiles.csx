#! "netcoreapp2.2"

using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Text;
using System.Linq;

string NormalizeLineEndings(byte[] content) => Encoding.UTF8.GetString(content).Replace("\r\n", "\n");

async Task DownloadFileIfChangedAsync(HttpClient client, string url, string path)
{
    var downloadedContent = await client.GetByteArrayAsync(url);
    if (File.Exists(path) && NormalizeLineEndings(File.ReadAllBytes(path)) == NormalizeLineEndings(downloadedContent))
    {
        Console.Out.WriteLine($"Unchanged {path}");
        return;
    }

    Console.Out.WriteLine($"Downloading {url} to {path}");
    File.WriteAllBytes(path, downloadedContent);
}

var schemaFolder = Path.GetFullPath(Environment.CurrentDirectory);
var projectFolder = Path.GetFullPath(Path.Combine(schemaFolder, ".."));

if (!string.Equals(Path.GetFileName(schemaFolder), "xsd", StringComparison.Ordinal)
    || !File.Exists(Path.Combine(schemaFolder, "DownloadSchemaFiles.csx"))
    || !File.Exists(Path.Combine(projectFolder, "IS24RestApi.csproj")))
    throw new InvalidOperationException("Run DownloadSchemaFiles.csx from the IS24RestApi/xsd directory.");

var schemaIndexUrls = new[] { "https://rest.immobilienscout24.de/restapi/api/offer/v1.0/?_wadl&_schema",
     "https://rest.immobilienscout24.de/restapi/api/search/v1.0/?_wadl&_schema" };
var client = new HttpClient();

var gisUrl = new Uri("https://rest.immobilienscout24.de/restapi/api/gis/v1.0/schema");
var gisPath = Path.Combine(schemaFolder, "platform", "gis-1.0.xsd");
Directory.CreateDirectory(Path.GetDirectoryName(gisPath));

await DownloadFileIfChangedAsync(client, gisUrl.ToString(), gisPath);

foreach (var url in schemaIndexUrls.Select(u => new Uri(u)))
{
    var baseUrl = url.GetLeftPart(UriPartial.Authority);
    var pg = await client.GetStringAsync(url);
    var matches = Regex.Matches(pg, @"<a href=""([^""]+)"">Namespace Prefix: (\S+)");

    foreach (var match in matches.Cast<Match>())
    {
        var link = baseUrl + match.Groups[1].Value;
        var ns = match.Groups[2].Value;
        var fn = Regex.Match(link, "[^/]+$").Value;

        if (Regex.IsMatch(fn, "^savedSearch.*2.*"))
            continue;

        var path = Path.Combine(schemaFolder, ns);
        if (ns == "common" && fn.StartsWith("messages-"))
            path = Path.Combine(path, "includes");

        Directory.CreateDirectory(path);

        path = Path.Combine(path, fn);

        await DownloadFileIfChangedAsync(client, link, path);
    }
}
