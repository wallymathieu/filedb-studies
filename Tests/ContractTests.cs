using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using SomeBasicFileStoreApp.Core;
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
        var array = JArray.Parse(resp).Select(ParseProduct);
        var withName = array.FirstOrDefault(a => a.Name == "Test");
        Assert.NotNull(withName.Id);
        var productResponse = await client.GetAsync("/api/v1/products/"+withName.Id);
        productResponse.EnsureSuccessStatusCode();
        var product = ParseProduct(JObject.Parse(await productResponse.Content.ReadAsStringAsync()));
        Assert.Equal(withName.Id,product.Id);
        Assert.Null(product.Properties);
    }
    [Fact]
    public async Task Can_save_product2_and_get_product()
    {
        using var client = _fixture.Server.CreateClient();
        var createProductResponse = await client.PostAsync("/api/v1/products",
            new StringContent(@"{
                        ""$type"": ""product2"",
                        ""name"": ""TestProduct2"",
                        ""cost"": 10,
                        ""Properties"": {
                            ""weight"": ""0.92kg"",
                            ""length"": ""250cm"",
                            ""width"": ""150cm""
                        }
                    }", Encoding.UTF8, "application/json"));
        createProductResponse.EnsureSuccessStatusCode();
        var productsResponse = await client.GetAsync("/api/v1/products/");
        var resp = await productsResponse.Content.ReadAsStringAsync();
        var array = JArray.Parse(resp).Select(ParseProduct);
        var withName = array.FirstOrDefault(a => a.Name == "TestProduct2");
        Assert.NotNull(withName.Id);
        var productResponse = await client.GetAsync("/api/v1/products/"+withName.Id);
        productResponse.EnsureSuccessStatusCode();
        var product = ParseProduct(JObject.Parse(await productResponse.Content.ReadAsStringAsync()));
        Assert.Equal(withName.Id,product.Id);
        Assert.Equivalent(new Dictionary<ProductProperty,string>
        {
            {ProductProperty.Weight,"0.92kg"},
            {ProductProperty.Length,"250cm"},
            {ProductProperty.Width,"150cm"}
        }.ToArray(), product.Properties.ToArray());
    }

    private static (string Id, string Name, IDictionary<ProductProperty,string> Properties) ParseProduct(JToken obj) => (
        Id: obj["id"].Value<string>(), 
        Name: obj["name"].Value<string>(),
        Properties: obj["properties"]?.ToObject<IDictionary<ProductProperty,string>>()
    );
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
