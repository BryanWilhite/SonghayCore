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
        [ProjectFileData(typeof(NuGetTests), "../../../../SonghayCore.xUnit/SonghayCore.xUnit.nuspec")]
        public void ShouldEditNuSpecFile(FileInfo nuspecInfo)
        {
            var nuspecDoc = XDocument.Load(nuspecInfo.FullName);
            var csprojDoc = XDocument.Load(nuspecInfo.FullName.Replace(".nuspec", ".csproj"));

            this._testOutputHelper.WriteLine($"reading {nameof(csprojDoc)}...");

            var version = csprojDoc.Root?.Element("PropertyGroup")?.Element("AssemblyVersion")?.Value;
            this._testOutputHelper.WriteLine($"{nameof(version)}: {version}");
            Assert.False(string.IsNullOrEmpty(version), $"Value for {nameof(version)} was not found.");

            var versionElement = nuspecDoc.Root?.Element("metadata")?.Element(nameof(version));
            Assert.NotNull(versionElement);
            versionElement.Value = version;

            var description = csprojDoc.Root?.Element("PropertyGroup")?.Element("Description")?.Value;
            this._testOutputHelper.WriteLine($"{nameof(description)}: {description}");
            Assert.False(string.IsNullOrEmpty(description), $"Value for {nameof(description)} was not found.");

            var descriptionElement = nuspecDoc.Root?.Element("metadata")?.Element(nameof(description));
            Assert.NotNull(descriptionElement);
            descriptionElement.Value = description;

            var authors = csprojDoc.Root?.Element("PropertyGroup")?.Element("Authors")?.Value;
            this._testOutputHelper.WriteLine($"{nameof(authors)}: {authors}");
            Assert.False(string.IsNullOrEmpty(authors), $"Value for {nameof(authors)} was not found.");

            var authorsElement = nuspecDoc.Root?.Element("metadata")?.Element(nameof(authors));
            Assert.NotNull(authorsElement);
            authorsElement.Value = authors;

            var title = csprojDoc.Root?.Element("PropertyGroup")?.Element("Title")?.Value;
            this._testOutputHelper.WriteLine($"{nameof(title)}: {title}");
            Assert.False(string.IsNullOrEmpty(title), $"Value for {nameof(title)} was not found.");

            var titleElement = nuspecDoc.Root?.Element("metadata")?.Element(nameof(title));
            Assert.NotNull(titleElement);
            titleElement.Value = title;

            var ownersElement = nuspecDoc.Root?.Element("metadata")?.Element("owners");
            Assert.NotNull(ownersElement);
            ownersElement.Value = authors;

            var projectUrl = csprojDoc.Root?.Element("PropertyGroup")?.Element("PackageProjectUrl")?.Value;
            this._testOutputHelper.WriteLine($"{nameof(projectUrl)}: {projectUrl}");
            Assert.False(string.IsNullOrEmpty(projectUrl), $"Value for {nameof(projectUrl)} was not found.");

            var projectUrlElement = nuspecDoc.Root?.Element("metadata")?.Element(nameof(projectUrl));
            Assert.NotNull(projectUrlElement);
            projectUrlElement.Value = projectUrl;

            var license = csprojDoc.Root?.Element("PropertyGroup")?.Element("PackageLicenseFile")?.Value;
            this._testOutputHelper.WriteLine($"{nameof(license)}: {license}");
            Assert.False(string.IsNullOrEmpty(license), $"Value for {nameof(license)} was not found.");

            var licenseElement = nuspecDoc.Root?.Element("metadata")?.Element(nameof(license));
            Assert.NotNull(licenseElement);
            licenseElement.Value = license;

            var iconUrl = csprojDoc.Root?.Element("PropertyGroup")?.Element("PackageIconUrl")?.Value;
            this._testOutputHelper.WriteLine($"{nameof(iconUrl)}: {iconUrl}");
            Assert.False(string.IsNullOrEmpty(iconUrl), $"Value for {nameof(iconUrl)} was not found.");

            var iconUrlElement = nuspecDoc.Root?.Element("metadata")?.Element(nameof(iconUrl));
            Assert.NotNull(iconUrlElement);
            iconUrlElement.Value = iconUrl;

            var requireLicenseAcceptance = csprojDoc.Root?.Element("PropertyGroup")?.Element("PackageRequireLicenseAcceptance")?.Value;
            this._testOutputHelper.WriteLine($"{nameof(requireLicenseAcceptance)}: {requireLicenseAcceptance}");
            Assert.False(string.IsNullOrEmpty(requireLicenseAcceptance), $"Value for {nameof(requireLicenseAcceptance)} was not found.");

            var requireLicenseAcceptanceElement = nuspecDoc.Root?.Element("metadata")?.Element(nameof(requireLicenseAcceptance));
            Assert.NotNull(requireLicenseAcceptanceElement);
            requireLicenseAcceptanceElement.Value = requireLicenseAcceptance;

            var summaryElement = nuspecDoc.Root?.Element("metadata")?.Element("summary");
            Assert.NotNull(summaryElement);
            summaryElement.Value = description;

            var releaseNotes = csprojDoc.Root?.Element("PropertyGroup")?.Element("PackageReleaseNotes")?.Value;
            this._testOutputHelper.WriteLine($"{nameof(releaseNotes)}: {releaseNotes}");
            Assert.False(string.IsNullOrEmpty(releaseNotes), $"Value for {nameof(releaseNotes)} was not found.");

            var releaseNotesElement = nuspecDoc.Root?.Element("metadata")?.Element(nameof(releaseNotes));
            Assert.NotNull(releaseNotesElement);
            releaseNotesElement.Value = releaseNotes;

            var copyright = csprojDoc.Root?.Element("PropertyGroup")?.Element("Copyright")?.Value;
            this._testOutputHelper.WriteLine($"{nameof(copyright)}: {copyright}");
            Assert.False(string.IsNullOrEmpty(copyright), $"Value for {nameof(copyright)} was not found.");

            var copyrightElement = nuspecDoc.Root?.Element("metadata")?.Element(nameof(copyright));
            Assert.NotNull(copyrightElement);
            copyrightElement.Value = copyright;

            var tags = csprojDoc.Root?.Element("PropertyGroup")?.Element("PackageTags")?.Value.Replace(';', ' ');
            this._testOutputHelper.WriteLine($"{nameof(tags)}: {tags}");
            Assert.False(string.IsNullOrEmpty(tags), $"Value for {nameof(tags)} was not found.");

            var tagsElement = nuspecDoc.Root?.Element("metadata")?.Element(nameof(tags));
            Assert.NotNull(tagsElement);
            tagsElement.Value = tags;

            var repositoryType = csprojDoc.Root?.Element("PropertyGroup")?.Element("RepositoryType")?.Value.Replace(';', ' ');
            this._testOutputHelper.WriteLine($"{nameof(repositoryType)}: {repositoryType}");
            Assert.False(string.IsNullOrEmpty(repositoryType), $"Value for {nameof(repositoryType)} was not found.");

            var repositoryUrl = csprojDoc.Root?.Element("PropertyGroup")?.Element("RepositoryUrl")?.Value.Replace(';', ' ');
            this._testOutputHelper.WriteLine($"{nameof(repositoryUrl)}: {repositoryUrl}");
            Assert.False(string.IsNullOrEmpty(repositoryUrl), $"Value for {nameof(repositoryUrl)} was not found.");

            var repositoryElement = nuspecDoc.Root?.Element("metadata")?.Element("repository");
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
