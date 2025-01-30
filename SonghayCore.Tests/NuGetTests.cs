using System.Xml.Linq;

namespace Songhay.Tests;

public class NuGetTests(ITestOutputHelper helper)
{
    [Trait(TestScalars.XunitCategory, TestScalars.XunitCategoryIntegrationManualTest)]
    [SkippableTheory]
    [ProjectFileData(typeof(NuGetTests), "../../../../SonghayCore/SonghayCore.nuspec")]
    [ProjectFileData(typeof(NuGetTests), "../../../../SonghayCore.xUnit/SonghayCore.xUnit.nuspec")]
    public void ShouldEditNuSpecFiles(FileSystemInfo nuspecInfo)
    {
        Skip.If(TestScalars.IsNotDebugging, TestScalars.ReasonForSkippingWhenNotDebugging);

        var nuspecDoc = XDocument.Load(nuspecInfo.FullName);
        Assert.NotNull(nuspecDoc);

        var nuspecMetaElement = nuspecDoc.Root?.Element("metadata");

        var csprojDoc = XDocument.Load(nuspecInfo.FullName.Replace(".nuspec", ".csproj"));
        Assert.NotNull(csprojDoc);

        var csprojPgElement = csprojDoc.Root?.Element("PropertyGroup");
        Assert.NotNull(csprojPgElement);

        helper.WriteLine($"reading {nameof(csprojDoc)}...");

        var version = csprojPgElement.Element("AssemblyVersion")?.Value;
        var versionElement = nuspecMetaElement?.Element(nameof(version));
        versionElement?.SetValue(version.ToReferenceTypeValueOrThrow());
        helper.WriteLine($"{nameof(version)}: {version}");

        var description = csprojPgElement.Element("Description")?.Value;
        var descriptionElement = nuspecMetaElement?.Element(nameof(description));
        descriptionElement?.SetValue(description.ToReferenceTypeValueOrThrow());
        helper.WriteLine($"{nameof(description)}: {description}");

        var authors = csprojPgElement.Element("Authors")?.Value;
        var authorsElement = nuspecMetaElement?.Element(nameof(authors));
        authorsElement?.SetValue(authors.ToReferenceTypeValueOrThrow());
        helper.WriteLine($"{nameof(authors)}: {authors}");

        var title = csprojPgElement.Element("Title")?.Value;
        var titleElement = nuspecMetaElement?.Element(nameof(title));
        titleElement?.SetValue(title.ToReferenceTypeValueOrThrow());
        helper.WriteLine($"{nameof(title)}: {title}");

        var ownersElement = nuspecMetaElement?.Element("owners");
        ownersElement?.SetValue(authors.ToReferenceTypeValueOrThrow());

        var projectUrl = csprojPgElement.Element("PackageProjectUrl")?.Value;
        var projectUrlElement = nuspecMetaElement?.Element(nameof(projectUrl));
        projectUrlElement?.SetValue(projectUrl.ToReferenceTypeValueOrThrow());
        helper.WriteLine($"{nameof(projectUrl)}: {projectUrl}");

        var license = csprojPgElement.Element("PackageLicenseFile")?.Value;
        var licenseElement = nuspecMetaElement?.Element(nameof(license));
        licenseElement?.SetValue(license.ToReferenceTypeValueOrThrow());
        helper.WriteLine($"{nameof(license)}: {license}");

        var iconUrl = csprojPgElement.Element("PackageIconUrl")?.Value;
        var iconUrlElement = nuspecMetaElement?.Element(nameof(iconUrl));
        iconUrlElement?.SetValue(iconUrl.ToReferenceTypeValueOrThrow());
        helper.WriteLine($"{nameof(iconUrl)}: {iconUrl}");

        var requireLicenseAcceptance = csprojPgElement.Element("PackageRequireLicenseAcceptance")?.Value;
        var requireLicenseAcceptanceElement = nuspecMetaElement?.Element(nameof(requireLicenseAcceptance));
        requireLicenseAcceptanceElement?.SetValue(requireLicenseAcceptance.ToReferenceTypeValueOrThrow());
        helper.WriteLine($"{nameof(requireLicenseAcceptance)}: {requireLicenseAcceptance}");

        var summaryElement = nuspecMetaElement?.Element("summary");
        summaryElement?.SetValue(description.ToReferenceTypeValueOrThrow());

        var releaseNotes = csprojPgElement.Element("PackageReleaseNotes")?.Value;
        var releaseNotesElement = nuspecMetaElement?.Element(nameof(releaseNotes));
        releaseNotesElement?.SetValue(releaseNotes.ToReferenceTypeValueOrThrow());
        helper.WriteLine($"{nameof(releaseNotes)}: {releaseNotes}");

        var copyright = csprojPgElement.Element("Copyright")?.Value;
        var copyrightElement = nuspecMetaElement?.Element(nameof(copyright));
        copyrightElement?.SetValue(copyright.ToReferenceTypeValueOrThrow());
        helper.WriteLine($"{nameof(copyright)}: {copyright}");

        var tags = csprojPgElement.Element("PackageTags")?.Value.Replace(';', ' ');
        var tagsElement = nuspecMetaElement?.Element(nameof(tags));
        tagsElement?.SetValue(tags.ToReferenceTypeValueOrThrow());
        helper.WriteLine($"{nameof(tags)}: {tags}");

        var repositoryType = csprojPgElement.Element("RepositoryType")?.Value.Replace(';', ' ');
        helper.WriteLine($"{nameof(repositoryType)}: {repositoryType}");

        var repositoryUrl = csprojPgElement.Element("RepositoryUrl")?.Value.Replace(';', ' ');
        helper.WriteLine($"{nameof(repositoryUrl)}: {repositoryUrl}");

        var repositoryElement = nuspecMetaElement?.Element("repository");
        Assert.NotNull(repositoryElement);
        var typeAttribute = repositoryElement.Attribute("type");
        Assert.NotNull(typeAttribute);
        var urlAttribute = repositoryElement.Attribute("url");
        Assert.NotNull(urlAttribute);
        typeAttribute.SetValue(repositoryType.ToReferenceTypeValueOrThrow());
        urlAttribute.SetValue(repositoryUrl.ToReferenceTypeValueOrThrow());

        nuspecDoc.Save(nuspecInfo.FullName);
    }
}
