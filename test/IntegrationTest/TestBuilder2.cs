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
            .WithEnvironment("ZOOKEEPER_CLIENT_PORT","2181")
            .WithName("zookeepertest")
            .WithNetworkAliases("zookeepertest")
            .WithNetwork(_network)
            .Build();
        
        KafkaService = new KafkaBuilder()
            .WithImage("repo.asax.ir/confluentinc/cp-kafka:7.4.3")
            .WithName("brokertest")
            .WithNetworkAliases("brokertest")
            .WithEnvironment("KAFKA_BROKER_ID","1")
            .WithEnvironment("KAFKA_ZOOKEEPER_CONNECT","zookeepertest:2181")
            .WithEnvironment("KAFKA_LISTENER_SECURITY_PROTOCOL_MAP","PLAINTEXT:PLAINTEXT,PLAINTEXT_HOST:PLAINTEXT,BROKER:PLAINTEXT")
            .WithEnvironment("KAFKA_AUTO_CREATE_TOPICS_ENABLE","true")
            .WithEnvironment("KAFKA_ALLOW_PLAINTEXT_LISTENER","yes")
            .WithEnvironment("KAFKA_OFFSETS_TOPIC_REPLICATION_FACTOR","1")
            .WithEnvironment("KAFKA_ADVERTISED_LISTENERS","KAFKA_ADVERTISED_LISTENERS\",\"PLAINTEXT://:29092,PLAINTEXT_HOST://:9092,BROKER://:9093")
            .WithNetwork(_network)
            .WithPortBinding(29092,29092)
            .WithPortBinding(9092,9092)
            .DependsOn(ZooKeeperService)
            .Build();

        await ZooKeeperService.StartAsync();
        await KafkaService.StartAsync();
        

    }

    public async Task DisposeAsync()
    {
        await ZooKeeperService.DisposeAsync();
        await KafkaService.DisposeAsync();
    }
}