using System.Text.Json.Serialization.Metadata;
using Microsoft.Extensions.Configuration;
using Songhay.Models;

namespace Songhay.Tests.Extensions;

public class ProgramMetadataExtensionsTests
{
    [Theory]
    [InlineData("""
                {
                    "DbmsSet": {
                        "MySQLite": {
                            "ConnectionString": "Data Source=./my.db",
                            "ProviderName": "System.Data.Sqlite"
                        }
                    },
                    "RestApiMetadataSet": {
                        "SocialTwitter": {
                            "ClaimsSet": {
                                "TwitterConsumerKey": "[key]",
                                "TwitterConsumerSecret": "[secret]",
                                "TwitterToken": "[token]",
                                "TwitterTokenSecret": "[secret]",
                                "TwitterTokenBearer": "[token]"
                            }
                        }
                    }
                }
                """, false )]
    [InlineData("""
                {
                    "DbmsSet": null,
                    "RestApiMetadataSet": {
                        "SocialTwitter": {
                            "ClaimsSet": {
                                "TwitterConsumerKey": "[key]",
                                "TwitterConsumerSecret": "[secret]",
                                "TwitterToken": "[token]",
                                "TwitterTokenSecret": "[secret]",
                                "TwitterTokenBearer": "[token]"
                            }
                        }
                    }
                }
                """, true )]
    [InlineData("""
                {
                    "DbmsSet": {
                        "MySQLite": {
                            "ConnectionString": "Data Source=./my.db",
                            "ProviderName": "System.Data.Sqlite"
                        }
                    },
                    "RestApiMetadataSet": null
                }
                """, true )]
    public void EnsureProgramMetadata_Test(string input, bool exceptionExpected)
    {
        ProgramMetadata? meta = JsonSerializer.Deserialize<ProgramMetadata>(input);

        if (exceptionExpected) Assert.Throws<NullReferenceException>(() => meta.EnsureProgramMetadata());
        else meta.EnsureProgramMetadata();
    }
}