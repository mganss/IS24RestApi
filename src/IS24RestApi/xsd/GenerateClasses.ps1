$xsds = ls *.xsd | %{ $_.Name }

if ($xsds.Count -eq 0) { exit }

# Add '.\' to last file name, see http://stackoverflow.com/questions/906093/xsd-exe-output-filename
$last = $xsds | select -last 1
$last = '.\' + $last
$xsds[$xsds.Count - 1] = $last

& xsd $xsds /c /n:IS24

# Generated .cs file is the last one written to
$cs = ls *.cs | Sort-Object -Descending -Property LastWriteTime | select -first 1

if (Test-Path IS24.cs) { rm IS24.cs }
ren $cs.Name "IS24.cs"

# Get-Content reads lines, we want a single string
$code = [System.IO.File]::ReadAllText("IS24.cs")
$code = $code.Replace('Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Namespace="http://rest.immobilienscout24.de/schema/common/1.0"', 'Form=System.Xml.Schema.XmlSchemaForm.Unqualified')
$code = $code.Replace("public abstract partial class RealEstate", "public partial class RealEstate")
$code = $code.Replace("public abstract partial class Attachment", "public partial class Attachment")
$code = "#pragma warning disable 1591`n" + $code
$code = $code + "`n#pragma warning restore 1591`n"

Set-Content "IS24.cs" $code
