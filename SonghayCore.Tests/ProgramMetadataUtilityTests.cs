using Songhay.Models;

namespace Songhay.Tests;

public class ProgramMetadataUtilityTests
{
    [SkippableFact]
    public void ShouldLoadConfigFromConventionalPath()
    {
        Skip.If(string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(ProgramMetadataUtility.EnvVarSettingsPath)));

        // act:
        ProgramMetadata? actual = ProgramMetadataUtility.GetProgramMetadataFromEnvironment();

        //assert:
        Assert.NotNull(actual);
    }

    [Theory]
    [InlineData(
        """
        {
            "DbmsSet": {},
            "RestApiMetadataSet": {
                "SonghayFeedsApi": {
                    "ApiKey": "sk_live_123",
                    "ClaimsSet": {
                        "feed-flickr.rss.xml": "http://api.flickr.com/services/feeds/photoset.gne?set=72157625087343217&nsid=7160940@N02&format=rss2&lang=en-us",
                        "feed-github.atom.xml": "https://github.com/BryanWilhite.atom",
                        "feed-studio.rss.xml": "https://songhayblog.azurewebsites.net/entry/feed.xml",
                        "quartz-schedule-mode": "not-testing",
                        "s3-set-key": "Wasabi1",
                        "s3-bucket-meta-key": "studio-public-region",
                        "s3-bucket-key": "songhay"
                    }
                },
                "Wasabi1": {
                    "ClaimsSet": {
                        "aws-credentials-profile-name": "rx-wasabi",
                        "bucket-region-suffix": "-region",
                        "public-key": "456",
                        "private-key": "789",
                        "bucket-location-template": "https://s3.{Region}.wasabisys.com/",
                        "bucket-location-template-placeholder": "{Region}",
                        "b-roll-player-video-region": "us-central-1",
                        "studio-public-region": "us-central-1"
                    }
                }
            }
        }
        """)]
    public void ShouldLoadConfigFromProcess(string json)
    {
        // arrange:
        Environment.SetEnvironmentVariable(
                ProgramMetadataUtility.EnvVarSettings,
                json,
                EnvironmentVariableTarget.Process //FUNKYKB: Process is by default
            );

        // act:
        ProgramMetadata? actual = ProgramMetadataUtility.GetProgramMetadataFromEnvironment();

        //assert:
        Assert.NotNull(actual);
    }
}
