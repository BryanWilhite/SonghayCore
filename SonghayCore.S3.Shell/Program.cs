using Microsoft.Extensions.Hosting;

using Songhay.S3.Extensions;
using Songhay.S3.Hosting;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddS3HostedService<AmazonS3Service>();

var host = builder.Build();
host.Run();
