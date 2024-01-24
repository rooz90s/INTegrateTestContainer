using Docker.DotNet.Models;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Networks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

using Testcontainers.Kafka;

namespace IntegrationTest;

public class TestBuilder2 :  WebApplicationFactory<Program>, IAsyncLifetime
{

    private INetwork _network;

    
    public  IContainer  KafkaService ;
    public IContainer ZooKeeperService;

    public TestBuilder2()
    {
        _network = new NetworkBuilder().WithName(Guid.NewGuid().ToString()).Build();
    }


    public async Task InitializeAsync()
    {
        
        
        ZooKeeperService = new ContainerBuilder()
            .WithImage("repo.asax.ir/confluentinc/cp-zookeeper:7.4.3")
            .WithPortBinding(2181,2181)
            .WithEnvironment("ALLOW_ANONYMOUS_LOGIN","yes")
            .WithEnvironment("ZOOKEEPER_CLIENT_PORT","2181")
            .WithName("zookeepertest")
            .WithNetwork(_network)
            .Build();
        
        KafkaService = new KafkaBuilder()
            .WithImage("confluentinc/cp-kafka:7.4.3")
            .WithName("kafkatest")
            .WithEnvironment("KAFKA_BROKER_ID","1")
            .WithEnvironment("KAFKA_ZOOKEEPER_CONNECT","172.22.3.52:2181")
            .WithEnvironment("KAFKA_ADVERTISED_LISTENERS","PLAINTEXT://localhost:29092")
            .WithEnvironment("KAFKA_CFG_AUTO_CREATE_TOPICS_ENABLE","true")
            .WithNetwork(_network)
            .WithPortBinding(29092,29092)
            .DependsOn(ZooKeeperService)
            .Build();

        await ZooKeeperService.StartAsync();
        //await KafkaService.StartAsync();


    }

    public async Task DisposeAsync()
    {
        await ZooKeeperService.DisposeAsync();
        await KafkaService.DisposeAsync();
    }
}