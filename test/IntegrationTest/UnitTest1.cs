using System.Net.Sockets;
using DotNet.Testcontainers.Builders;

namespace IntegrationTest;

public class UnitTest1
{
    

    public UnitTest1()
    {
        
        
    }


    [Fact]
    public async Task Test1()
    {
         TestBuilder _builder = new TestBuilder();
         await _builder.InitializeAsync();

         var service1_Hostname = _builder.Service1Container.Hostname;
         var service1_Port = _builder.Service1Container.GetMappedPublicPort(80);

         //using var Service1Client = new TcpClient(service1_Hostname, service1_Port);

         using var httpClient = new HttpClient();
         httpClient.BaseAddress = new UriBuilder("http", service1_Hostname, service1_Port).Uri;
         
         
         var httpResponseMessage = await httpClient.GetAsync("WeatherForecast")
             .ConfigureAwait(false);
         
         
         
         
         Assert.True(true);
    }
}