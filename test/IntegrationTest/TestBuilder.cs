using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Images;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace IntegrationTest;

public class TestBuilder :  WebApplicationFactory<Program>, IAsyncLifetime
{
    //src/Service1/Dockerfile
    //G:\Projects\INTegrateTestContainer\src\Service1\Dockerfile
    //WithDockerfileDirectory(@"G:\Projects\INTegrateTestContainer\src\Service1")
    private readonly IFutureDockerImage _service1Container = new ImageFromDockerfileBuilder()
        .WithDockerfileDirectory(CommonDirectoryPath.GetGitDirectory(),"src")
        .WithDockerfile(Path.Combine("Service1","Dockerfile"))
        .WithBuildArgument("ASPNETCORE_ENVIRONMENT", "Test")
        .WithName("service1")
        .WithDeleteIfExists(true)
        .WithCreateParameterModifier((parameters) => Console.WriteLine($" ---> {parameters.Outputs}"))
        .Build();

    public async Task InitializeAsync()
    {
        Console.WriteLine($" ===> {CommonDirectoryPath.GetSolutionDirectory().DirectoryPath}\\src\\Service1");
        await _service1Container.CreateAsync().ConfigureAwait(false);
    }

    public async Task DisposeAsync()
    {
        await _service1Container.DisposeAsync();
    }
    
    
}