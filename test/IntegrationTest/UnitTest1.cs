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
    }
}