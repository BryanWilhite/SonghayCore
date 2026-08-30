using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Songhay;
using Songhay.Extensions;
using Songhay.S3.Extensions;
using Songhay.S3.Hosting;

DisplayCredits();

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddConventionalJsonFile();

builder.Services
    .AddLogging()
    .AddProgramMetadata(builder.Configuration)
    .AddS3HostedService<AmazonS3Service>();

IHost host = builder.Build();

host.Run();

return;

static void DisplayCredits()
{
    Console.Write(ProgramAssemblyUtility.GetAssemblyInfo(Assembly.GetExecutingAssembly(), true));

    Console.WriteLine(string.Empty);

    Console.WriteLine("Activities Assembly:");
    Console.Write(ProgramAssemblyUtility.GetAssemblyInfo(typeof(AmazonS3Service).Assembly, true));

    Console.WriteLine(string.Empty);

    Console.WriteLine("IHost Assembly:");
    Console.Write(ProgramAssemblyUtility.GetAssemblyInfo(typeof(IHost).Assembly, true));
}
