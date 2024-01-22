using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Images;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace IntegrationTest;

public class TestBuilder :  WebApplicationFactory<Program>, IAsyncLifetime
{
    //src/Service1/Dockerfile
    //G:\Projects\INTegrateTestContainer\src\Service1\Dockerfile
    //src/Service1/Dockerfile
    //WithDockerfileDirectory(@"G:\Projects\INTegrateTestContainer\src\Service1")
    
    public  IContainer  Service1Container ;
    
    public readonly IFutureDockerImage Service1Image = new ImageFromDockerfileBuilder()
        .WithDockerfileDirectory(CommonDirectoryPath.GetSolutionDirectory(), "src/Service1")
        .WithDockerfile("Dockerfile")
        .WithBuildArgument("ASPNETCORE_ENVIRONMENT", "Test")
        .WithName("service1")
        .WithDeleteIfExists(true)
        .WithCreateParameterModifier((parameters) => Console.WriteLine($" ---> {parameters.Outputs}"))
        .Build();

    public async Task InitializeAsync()
    {
        await Service1Image.CreateAsync();
        Service1Container = new ContainerBuilder()
            .WithImage(Service1Image)
            .WithPortBinding(8000,80)
            .Build();

        await Service1Container.StartAsync();
        
    }

    public async Task DisposeAsync()
    {
        await Service1Container.DisposeAsync();
        await Service1Image.DisposeAsync();
    }


    
}