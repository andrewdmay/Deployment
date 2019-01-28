var target = Argument("target", "UnitTests");

// Run Unit tests (Default target)
var unitTests = Task("UnitTests")
    .Does(() => DotNetCoreTest("Deployment.Api.Tests"));

// Release Package to NuGet
Task("Release")
    .IsDependentOn(unitTests)
    .Does(() => 
{
    var nugetApiKey = Argument<string>("nugetApiKey", null) ?? EnvironmentVariable("NUGET_API_KEY");
    if (nugetApiKey == null)
    {
        throw new Exception("nugetApiKey argument or NUGET_API_KEY environment variable must be set");
    }
    CleanDirectory("Deployment.Sdk/bin");
    DotNetCoreBuild("Deployment.Sdk", new DotNetCoreBuildSettings { Configuration = "Release" });
    // No Push for fake sdk
});

RunTarget(target);