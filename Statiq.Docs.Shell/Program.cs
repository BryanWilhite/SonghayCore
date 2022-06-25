using Statiq.Common;
using Statiq.App;
using Statiq.Docs;
using Statiq.Web;

await Bootstrapper
    .Factory
    .CreateDocs(args)
    .AddExcludedPath("../../../Statiq.Docs.Shell")
    .AddSettings(new Dictionary<string, object>
    {
        { DocsKeys.ApiPath, "api" },
        {
            DocsKeys.SourceFiles,
            new []
            {
                "../../../SonghayCore/**/{!.git,!bin,!obj,!packages,!*.Tests,}/**/*.cs",
                "../../../SonghayCore.xUnit/**/{!.git,!bin,!obj,!packages,!*.Tests,}/**/*.cs",
            }
        },
        { DocsKeys.OutputApiDocuments, true },
        { WebKeys.Author, "@BryanWilhite" },
        { WebKeys.Copyright, $"(c) {DateTime.Now.Year} Songhay System" },
        { WebKeys.GitHubName, "Bryan Wilhite" },
        { WebKeys.GitHubUsername, "BryanWilhite" },
        { WebKeys.OutputPath, "tmp-output" },
    })
    .RunAsync();
