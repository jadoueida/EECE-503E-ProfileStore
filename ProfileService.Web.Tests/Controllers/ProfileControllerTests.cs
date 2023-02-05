using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using ProfileService.Web.Dtos;
using ProfileService.Web.Storage;

namespace ProfileService.Web.Tests.Controllers;

public class ProfileControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public ProfileControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }
    
    [Fact]
    public async Task GetProfile()
    {
        var profileStoreMock = new Mock<IProfileStore>();

        var profile = new Profile("foobar", "Foo", "Bar");
        profileStoreMock.Setup(m => m.GetProfile(profile.Username))
            .ReturnsAsync(profile);

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton(profileStoreMock.Object);
            });
        }).CreateClient();

        var response = await client.GetAsync($"/Profile/{profile.Username}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.Equal(profile, JsonConvert.DeserializeObject<Profile>(responseBody));
    }
    
    // TODO: Add a test for NotFound response
    [Fact]
    public async Task TestNoProfile()
    {
        var profileStoreMock = new Mock<IProfileStore>();

        var profile = new Profile("foobar", "Foo", "Bar");
        var profile2 = new Profile("foobar2", "Foo2", "Bar2");
        var profile3 = new Profile("foobar3", "Foo3", "Bar3");
        profileStoreMock.Setup(m => m.GetProfile(profile.Username))
            .ReturnsAsync(profile);
        profileStoreMock.Setup(m => m.GetProfile(profile2.Username))
            .ReturnsAsync(profile2);
        profileStoreMock.Setup(m => m.GetProfile(profile3.Username))
            .ReturnsAsync(profile3);

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton(profileStoreMock.Object);
            });
        }).CreateClient();

        var response = await client.GetAsync($"/Profile/foobar4");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    //TODO: Add tests for remaining APIs 
    [Fact]
    public async Task TestCreateProfile()
    {
        var profileStoreMock = new Mock<IProfileStore>();

        var profile = new Profile("jadoueida", "jad", "oueida");
        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton(profileStoreMock.Object);
            });
        }).CreateClient();
        
        var profileSent = "{\"username\": \"jadoueida\",\"firstName\": \"jad\",\"lastName\":\"oueida\"}";
        HttpContent c = new StringContent(profileSent,Encoding.UTF8, "application/json");
        Uri u = new Uri("/Profile");
        HttpRequestMessage request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = u,
            Content = c
        };
        HttpResponseMessage response = await client.SendAsync(request);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.Equal(profile, JsonConvert.DeserializeObject<Profile>(responseBody));
    }
    
    [Fact]
    public async Task TestUpdateProfile()
    {
        var profileStoreMock = new Mock<IProfileStore>();

        var profile = new Profile("jadoueida", "jad", "oueida");
        profileStoreMock.Setup(m => m.GetProfile(profile.Username))
            .ReturnsAsync(profile);
        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton(profileStoreMock.Object);
            });
        }).CreateClient();
        
        var profileSent = "{\"firstName\": \"karim\",\"lastName\":\"oueida\"}";  //update from jad to karim
        HttpContent c = new StringContent(profileSent,Encoding.UTF8, "application/json");
        Uri u = new Uri($"/Profile/jadoueida");
        HttpRequestMessage request = new HttpRequestMessage
        {
            Method = HttpMethod.Put,
            RequestUri = u,
            Content = c
        };
        HttpResponseMessage response = await client.SendAsync(request);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var responseBody = await response.Content.ReadAsStringAsync();
    }
}