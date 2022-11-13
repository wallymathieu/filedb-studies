using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Web;
using Xunit;

namespace SomeBasicFileStoreApp.Tests;

public class ContractTests:IClassFixture<ApiFixture>
{
    private readonly ApiFixture _fixture;
    public ContractTests(ApiFixture fixture) => this._fixture = fixture;

    [Fact]
    public async Task Can_save_and_get_customer()
    {
        using var client = _fixture.Server.CreateClient();
        var createdCustomer = await client.PostAsync("/api/v1/customers",
            new StringContent(@"{
                        ""firstname"": ""Test"",
                        ""lastname"": ""TRest""
                    }", Encoding.UTF8, "application/json"));
        createdCustomer.EnsureSuccessStatusCode();
        var customersResponse = await client.GetAsync("/api/v1/customers/"); 
        var obj = JArray.Parse(await customersResponse.Content.ReadAsStringAsync());
        var id = obj[0]["id"].Value<string>();
        Assert.NotNull(id);
        var customerResponse = await client.GetAsync("/api/v1/customers/"+id);
        customerResponse.EnsureSuccessStatusCode();
        var customerId = JObject.Parse(await customerResponse.Content.ReadAsStringAsync())["id"].Value<string>();
        Assert.Equal(id,customerId);
    }
    [Fact]
    public async Task Can_save_and_get_product()
    {
        using var client = _fixture.Server.CreateClient();
        var createProductResponse = await client.PostAsync("/api/v1/products",
            new StringContent(@"{
                        ""name"": ""Test"",
                        ""cost"": 10
                    }", Encoding.UTF8, "application/json"));
        createProductResponse.EnsureSuccessStatusCode();
        var productsResponse = await client.GetAsync("/api/v1/products/");
        var resp = await productsResponse.Content.ReadAsStringAsync();
        var obj = JArray.Parse(resp);
        var id = obj[0]["id"].Value<string>();
        Assert.NotNull(id);
        var productResponse = await client.GetAsync("/api/v1/products/"+id);
        productResponse.EnsureSuccessStatusCode();
        var productId = JObject.Parse(await productResponse.Content.ReadAsStringAsync())["id"].Value<string>();
        Assert.Equal(id,productId);
    }
}

public class ApiFixture:IDisposable
{
    static TestServer Create(string file)
    {
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("JSON_FILE",file)
        }).Build();
        return new TestServer(new WebHostBuilder()
            .UseKestrel()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseConfiguration(configuration)
            .UseStartup<TestStartup>());
    }
    private readonly TestServer _testServer;
    private readonly string _tempFileName;

    public ApiFixture()
    {
        _tempFileName = Path.GetTempFileName();
        _testServer = Create(_tempFileName);
    }

    public void Dispose()
    {
        _testServer.Dispose();
        try
        {
            File.Delete(_tempFileName);
        }
        catch (Exception)
        {
            // ignored
        }
    }
    public TestServer Server=>_testServer;

    class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
