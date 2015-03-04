using NuGet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace IS24RestApi.Pack
{
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                GenerateNuSpec(args);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Out.WriteLine("Error: {0}", ex);
                Console.ResetColor();
                return 1;
            }

            return 0;
        }

        private static void GenerateNuSpec(string[] args)
        {
            var nuspec = args[0];
            var spec = XElement.Load(nuspec);
            var replaced = false;
            var files = new XElement("files");
            spec.Add(files);
            var metadata = spec.Element("metadata");
            var dependencies = new XElement("dependencies");
            metadata.Add(dependencies);

            var repo = new SharedPackageRepository(Path.Combine(Path.GetDirectoryName(nuspec), @"..\packages"));
            var packages = repo.GetPackages();
            var csprojs = args.Skip(2).ToList();

            if (!csprojs.Any()) throw new Exception("No .csproj files given.");

            foreach (var csproj in csprojs)
            {
                var proj = XElement.Load(csproj);
                var ns = proj.Name.Namespace;

                var path = Path.GetFullPath(Path.GetDirectoryName(csproj));
                var assemblyName = (string)proj.Descendants(ns + "AssemblyName").First();
                var framework = "net" + Regex.Replace((string)proj.Descendants(ns + "TargetFrameworkVersion").First(), @"\D+", "");
                var srcs = proj.Descendants(ns + "Compile").Where(c => !c.HasElements).Select(c => (string)c.Attribute("Include")); // only non-linked files

                // Add .dll, .pdb, .xml
                var assemblyPath = Path.Combine(path, @"bin\Release", assemblyName);
                files.Add(new XElement("file", new XAttribute("src", assemblyPath + ".dll"), new XAttribute("target", @"lib\" + framework)),
                    new XElement("file", new XAttribute("src", assemblyPath + ".pdb"), new XAttribute("target", @"lib\" + framework)),
                    new XElement("file", new XAttribute("src", assemblyPath + ".xml"), new XAttribute("target", @"lib\" + framework)));

                // Add sources
                foreach (var src in srcs)
                    files.Add(new XElement("file", new XAttribute("src", Path.Combine(path, src)), new XAttribute("target", Path.Combine("src", src))));

                // Replace $xyz$ variables only from first assembly
                if (!replaced)
                {
                    var assembly = Assembly.LoadFile(Path.Combine(path, @"bin\Release", assemblyName + ".dll"));
                    var vars = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                    {
                        { "id", assemblyName },
                        { "version", assembly.GetName().Version.ToString() },
                        { "author", GetAttribute<AssemblyCompanyAttribute>(assembly).Company },
                        { "description", GetAttribute<AssemblyDescriptionAttribute>(assembly).Description },
                        { "title", GetAttribute<AssemblyTitleAttribute>(assembly).Title },
                        { "copyright", GetAttribute<AssemblyCopyrightAttribute>(assembly).Copyright }
                    };

                    foreach (var element in spec.Descendants().Where(e => !e.HasElements))
                    {
                        var val = Regex.Replace(element.Value, @"\$([^\$]+)\$", m => vars[m.Groups[1].Value]);
                        element.SetValue(val);
                    }

                    replaced = true;
                }

                // Dependencies
                var conf = PackageReferenceFile.CreateFromProject(csproj);
                if (File.Exists(conf.FullPath))
                {
                    var references = conf.GetPackageReferences().Where(r => !r.IsDevelopmentDependency).ToList();
                    var walker = new DependentsWalker(repo, VersionUtility.ParseFrameworkName(framework));
                    var deps = references.Where(r => !walker.GetDependents(repo.FindPackage(r.Id, r.Version)).Any()).ToList();
                    var group = new XElement("group", new XAttribute("targetFramework", framework));
                    dependencies.Add(group);
                    foreach (var dep in deps)
                        group.Add(new XElement("dependency", new XAttribute("id", dep.Id), new XAttribute("version", dep.Version)));
                }
            }

            Console.OutputEncoding = Encoding.UTF8;
            spec.Save(args[1]);
        }

        private static TAttr GetAttribute<TAttr>(Assembly asm) where TAttr : Attribute
        {
            var attrs = asm.GetCustomAttributes(typeof(TAttr), false);

            if (attrs.Length > 0)
            {
                return attrs[0] as TAttr;
            }

            return null;
        }
    }
}
