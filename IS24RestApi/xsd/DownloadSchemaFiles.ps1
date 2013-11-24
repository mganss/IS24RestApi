$url = New-Object uri('http://rest.immobilienscout24.de/restapi/api/offer/v1.0/?_wadl&_schema')
$base = $url.GetLeftPart([UriPartial]::Authority)
$client = (New-Object System.Net.WebClient)
$pg = $client.DownloadString($url)
$links = $pg | Select-String -AllMatches '<a href="([^"]+)' | %{ $_.Matches } | %{ $base + $_.Groups[1].Value }

foreach ($link in $links) {
    $fn = $link | Select-String '[^/]+$' | %{ $_.Matches } | %{ $_.Value }
    echo "downloading $link to $fn"
    $client.DownloadFile($link, $fn)
}

mv -Force .\messages* includes
