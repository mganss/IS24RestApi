#! "netcoreapp2.2"

using System.Net;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

var urls = new[] { "https://rest.immobilienscout24.de/restapi/api/offer/v1.0/?_wadl&_schema",
     "https://rest.immobilienscout24.de/restapi/api/search/v1.0/?_wadl&_schema",
     "https://rest.immobilienscout24.de/restapi/api/gis/v1.0/schema" };

foreach (var url in urls.Select(u => new Uri(u)))
{
    var baseUrl = url.GetLeftPart(UriPartial.Authority);
    var client = new WebClient();
    var pg = client.DownloadString(url);
    var matches = Regex.Matches(pg, @"<a href=""([^""]+)"">Namespace Prefix: (\S+)");

    foreach (var match in matches.Cast<Match>())
    {
        var link = baseUrl + match.Groups[1].Value;
        var ns = match.Groups[2].Value;
        var fn = Regex.Match(link, "[^/]+$").Value;

        if (Regex.IsMatch(fn, "^savedSearch.*2.*"))
            continue;

        var path = Path.Combine(Environment.CurrentDirectory, ns);
        if (ns == "common" && fn.StartsWith("messages-"))
            path = Path.Combine(path, "includes");

        Directory.CreateDirectory(path);

        path = Path.Combine(path, fn);

        Console.Out.WriteLine($"Downloading {link} to {path}");
        client.DownloadFile(link, path);
    }
}
