foreach ($url in ((New-Object uri('http://rest.immobilienscout24.de/restapi/api/offer/v1.0/?_wadl&_schema')),
    (New-Object uri('http://rest.immobilienscout24.de/restapi/api/search/v1.0/?_wadl&_schema')),
    (New-Object uri('http://rest.immobilienscout24.de/restapi/api/gis/v1.0/schema'))))
{
    $base = $url.GetLeftPart([UriPartial]::Authority)
    $client = (New-Object System.Net.WebClient)
    $pg = $client.DownloadString($url)
    $matches = $pg | Select-String -AllMatches '<a href="([^"]+)">Namespace Prefix: (\S+)' | %{ $_.Matches }

    $scriptpath = $MyInvocation.MyCommand.Path
    $dir = Split-Path $scriptpath

    foreach ($match in $matches)
    {
	    $link = $base + $match.Groups[1].Value
	    $ns = $match.Groups[2].Value
        $fn = $link | Select-String '[^/]+$' | %{ $_.Matches } | %{ $_.Value }

        if ($fn -like "savedSearch*2*")
        {
            continue;
        }

        $path = $dir + '\'
        $path += $ns + '\'

        if ($ns -eq "common" -and $fn -like "messages-*")
        {
            $path += 'includes\'
        }

        md $path -Force

        $path += $fn
        echo "downloading $link to $path"
        $client.DownloadFile($link, $path)
    }
}
