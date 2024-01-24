namespace IntegrationTest;

public class UnitTest2
{

    [Fact]
    public async Task TestMe()
    {
        TestBuilder2 _builder = new TestBuilder2();
        await _builder.InitializeAsync();
        
        Assert.True(true);

    }
}