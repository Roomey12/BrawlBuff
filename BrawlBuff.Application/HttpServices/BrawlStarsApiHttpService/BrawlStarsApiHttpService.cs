using BrawlBuff.Application.HttpServices.BrawlStarsApiHttpService.Models;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace BrawlBuff.Application.HttpServices.BrawlStarsApiHttpService;

public class BrawlStarsApiHttpService
{
    private readonly HttpClient _httpClient;
    private IConfiguration Configuration;

    public BrawlStarsApiHttpService(HttpClient httpClient, IConfiguration configuration)
    {
        Configuration = configuration;

        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://bsproxy.royaleapi.dev/v1/");

        var brawlStarsApiKey = Configuration["BrawlStarsApi-ApiKey"];
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", brawlStarsApiKey);
    }

    public async Task<Player> GetPlayerByTagAsync(string tag, bool checkTag = false)
    {
        if (checkTag)
        {
            if (tag.StartsWith("#"))
            {
                tag = tag.Replace("#", "%23");
            }
        }

        var jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
        try
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            var player = await _httpClient.GetFromJsonAsync<Player>($"players/{tag}", jsonSerializerOptions);
            return player;
        }
        catch (Exception ex)
        {

        }
        return null;
    }

    public async Task<List<BattleLog>> GetRecentBattlesByPlayersTagAsync(string tag, bool checkTag = false)
    {
        if (checkTag)
        {
            if (tag.StartsWith("#"))
            {
                tag = tag.Replace("#", "%23");
            }
        }

        var jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

        var root = await _httpClient.GetFromJsonAsync<Root>($"players/{tag}/battlelog", jsonSerializerOptions);

        return root?.Items;
    }
}