await Bootstrapper
    .Factory
    .CreateDocs(args)
    .AddExcludedPath("../../../../Statiq.Docs.Shell")
    .AddSettings(new Dictionary<string, object>
    {
        { Keys.Title, "API" },
        { Keys.LinkRoot, "SonghayCore" },
        { DocsKeys.ApiPath, "latest" },
        {
            DocsKeys.SourceFiles,
            new []
            {
                "../../../../../SonghayCore/**/{!.git,!bin,!obj,!packages,!*.Tests,}/**/*.cs",
                "../../../../../SonghayCore.xUnit/**/{!.git,!bin,!obj,!packages,!*.Tests,}/**/*.cs",
            }
        },
        { DocsKeys.OutputApiDocuments, true },
        { WebKeys.Author, "@BryanWilhite" },
        { WebKeys.Copyright, $"(c) {DateTime.Now.Year} Songhay System" },
        { WebKeys.GitHubName, "Bryan Wilhite" },
        { WebKeys.GitHubUsername, "BryanWilhite" },
        { WebKeys.OutputPath, "../../../../docs" },
    })
    .RunAsync();
