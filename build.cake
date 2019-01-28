#addin nuget:?package=Cake.Docker&version=0.9.7

var target = Argument("target", "UnitTests");

var testApi = Task("TestApi")
    .Does(() => DotNetCoreTest("Deployment.Api.Tests"));

// All Unit Tests
var unitTests = Task("UnitTests")
    .IsDependentOn(testApi);

// Package API in Docker image
var packageApi = Task("PackageApi")
    .IsDependentOn(testApi)
    .Does(() => 
{
    DockerBuild(new DockerImageBuildSettings {
        Pull = true,
        Rm = true,
        Tag = new string[]{"microservice/deployment:latest"}
    }, ".");
});

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