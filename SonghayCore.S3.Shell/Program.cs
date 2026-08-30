using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Songhay.Extensions;
using Songhay.S3.Extensions;
using Songhay.S3.Hosting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddConventionalJsonFile();

builder.Services
    .AddLogging()
    .AddProgramMetadata(builder.Configuration)
    .AddS3HostedService<AmazonS3Service>();

IHost host = builder.Build();

host.Run();
