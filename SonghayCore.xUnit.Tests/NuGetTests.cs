using System.IO;
using System.Xml.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Songhay.Tests
{
    public class NuGetTests
    {
        public NuGetTests(ITestOutputHelper helper)
        {
            this._testOutputHelper = helper;
        }

        [DebuggerAttachedTheory]
        [ProjectFileData(typeof(NuGetTests), "../../../../SonghayCore/SonghayCore.nuspec")]
        [ProjectFileData(typeof(NuGetTests), "../../../../SonghayCore.xUnit/SonghayCore.xUnit.nuspec")]
        public void ShouldEditNuSpecFiles(FileSystemInfo nuspecInfo)
        {
            var nuspecDoc = XDocument.Load(nuspecInfo.FullName);
            Assert.NotNull(nuspecDoc);

            var nuspecMetaElement = nuspecDoc?.Root?.Element("metadata");
            Assert.NotNull(nuspecMetaElement);

            var csprojDoc = XDocument.Load(nuspecInfo.FullName.Replace(".nuspec", ".csproj"));
            Assert.NotNull(csprojDoc);

            var csprojPGElement = csprojDoc?.Root?.Element("PropertyGroup");
            Assert.NotNull(csprojPGElement);

            this._testOutputHelper.WriteLine($"reading {nameof(csprojDoc)}...");

            var version = csprojPGElement.Element("AssemblyVersion")?.Value;
            this._testOutputHelper.WriteLine($"{nameof(version)}: {version}");
            Assert.False(string.IsNullOrEmpty(version), $"Value for {nameof(version)} was not found.");

            var versionElement = nuspecMetaElement.Element(nameof(version));
            Assert.NotNull(versionElement);
            versionElement.Value = version;

            var description = csprojPGElement.Element("Description")?.Value;
            this._testOutputHelper.WriteLine($"{nameof(description)}: {description}");
            Assert.False(string.IsNullOrEmpty(description), $"Value for {nameof(description)} was not found.");

            var descriptionElement = nuspecMetaElement.Element(nameof(description));
            Assert.NotNull(descriptionElement);
            descriptionElement.Value = description;

            var authors = csprojPGElement.Element("Authors")?.Value;
            this._testOutputHelper.WriteLine($"{nameof(authors)}: {authors}");
            Assert.False(string.IsNullOrEmpty(authors), $"Value for {nameof(authors)} was not found.");

            var authorsElement = nuspecMetaElement.Element(nameof(authors));
            Assert.NotNull(authorsElement);
            authorsElement.Value = authors;

            var title = csprojPGElement.Element("Title")?.Value;
            this._testOutputHelper.WriteLine($"{nameof(title)}: {title}");
            Assert.False(string.IsNullOrEmpty(title), $"Value for {nameof(title)} was not found.");

            var titleElement = nuspecMetaElement.Element(nameof(title));
            Assert.NotNull(titleElement);
            titleElement.Value = title;

            var ownersElement = nuspecMetaElement.Element("owners");
            Assert.NotNull(ownersElement);
            ownersElement.Value = authors;

            var projectUrl = csprojPGElement.Element("PackageProjectUrl")?.Value;
            this._testOutputHelper.WriteLine($"{nameof(projectUrl)}: {projectUrl}");
            Assert.False(string.IsNullOrEmpty(projectUrl), $"Value for {nameof(projectUrl)} was not found.");

            var projectUrlElement = nuspecMetaElement.Element(nameof(projectUrl));
            Assert.NotNull(projectUrlElement);
            projectUrlElement.Value = projectUrl;

            var license = csprojPGElement.Element("PackageLicenseFile")?.Value;
            this._testOutputHelper.WriteLine($"{nameof(license)}: {license}");
            Assert.False(string.IsNullOrEmpty(license), $"Value for {nameof(license)} was not found.");

            var licenseElement = nuspecMetaElement.Element(nameof(license));
            Assert.NotNull(licenseElement);
            licenseElement.Value = license;

            var iconUrl = csprojPGElement.Element("PackageIconUrl")?.Value;
            this._testOutputHelper.WriteLine($"{nameof(iconUrl)}: {iconUrl}");
            Assert.False(string.IsNullOrEmpty(iconUrl), $"Value for {nameof(iconUrl)} was not found.");

            var iconUrlElement = nuspecMetaElement.Element(nameof(iconUrl));
            Assert.NotNull(iconUrlElement);
            iconUrlElement.Value = iconUrl;

            var requireLicenseAcceptance = csprojPGElement.Element("PackageRequireLicenseAcceptance")?.Value;
            this._testOutputHelper.WriteLine($"{nameof(requireLicenseAcceptance)}: {requireLicenseAcceptance}");
            Assert.False(string.IsNullOrEmpty(requireLicenseAcceptance), $"Value for {nameof(requireLicenseAcceptance)} was not found.");

            var requireLicenseAcceptanceElement = nuspecMetaElement.Element(nameof(requireLicenseAcceptance));
            Assert.NotNull(requireLicenseAcceptanceElement);
            requireLicenseAcceptanceElement.Value = requireLicenseAcceptance;

            var summaryElement = nuspecMetaElement.Element("summary");
            Assert.NotNull(summaryElement);
            summaryElement.Value = description;

            var releaseNotes = csprojPGElement.Element("PackageReleaseNotes")?.Value;
            this._testOutputHelper.WriteLine($"{nameof(releaseNotes)}: {releaseNotes}");
            Assert.False(string.IsNullOrEmpty(releaseNotes), $"Value for {nameof(releaseNotes)} was not found.");

            var releaseNotesElement = nuspecMetaElement.Element(nameof(releaseNotes));
            Assert.NotNull(releaseNotesElement);
            releaseNotesElement.Value = releaseNotes;

            var copyright = csprojPGElement.Element("Copyright")?.Value;
            this._testOutputHelper.WriteLine($"{nameof(copyright)}: {copyright}");
            Assert.False(string.IsNullOrEmpty(copyright), $"Value for {nameof(copyright)} was not found.");

            var copyrightElement = nuspecMetaElement.Element(nameof(copyright));
            Assert.NotNull(copyrightElement);
            copyrightElement.Value = copyright;

            var tags = csprojPGElement.Element("PackageTags")?.Value.Replace(';', ' ');
            this._testOutputHelper.WriteLine($"{nameof(tags)}: {tags}");
            Assert.False(string.IsNullOrEmpty(tags), $"Value for {nameof(tags)} was not found.");

            var tagsElement = nuspecMetaElement.Element(nameof(tags));
            Assert.NotNull(tagsElement);
            tagsElement.Value = tags;

            var repositoryType = csprojPGElement.Element("RepositoryType")?.Value.Replace(';', ' ');
            this._testOutputHelper.WriteLine($"{nameof(repositoryType)}: {repositoryType}");
            Assert.False(string.IsNullOrEmpty(repositoryType), $"Value for {nameof(repositoryType)} was not found.");

            var repositoryUrl = csprojPGElement.Element("RepositoryUrl")?.Value.Replace(';', ' ');
            this._testOutputHelper.WriteLine($"{nameof(repositoryUrl)}: {repositoryUrl}");
            Assert.False(string.IsNullOrEmpty(repositoryUrl), $"Value for {nameof(repositoryUrl)} was not found.");

            var repositoryElement = nuspecMetaElement.Element("repository");
            Assert.NotNull(repositoryElement);
            var typeAttribute = repositoryElement.Attribute("type");
            Assert.NotNull(typeAttribute);
            var urlAttribute = repositoryElement.Attribute("url");
            Assert.NotNull(urlAttribute);
            typeAttribute.Value = repositoryType;
            urlAttribute.Value = repositoryUrl;

            nuspecDoc.Save(nuspecInfo.FullName);
        }

        ITestOutputHelper _testOutputHelper;
    }
}
