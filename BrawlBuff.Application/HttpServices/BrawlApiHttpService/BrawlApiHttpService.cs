using BrawlBuff.Application.Common;
using BrawlBuff.Application.HttpServices.BrawlApiHttpService.Models;
using BrawlBuff.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BrawlBuff.Application.HttpServices.BrawlApiHttpService
{
    public class BrawlApiHttpService
    {
        private readonly HttpClient _httpClient;

        public BrawlApiHttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://api.brawlapi.com/v1/");
        }

        public async Task<List<Map>> GetMapsAsync()
        {
            var jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var root = await _httpClient.GetFromJsonAsync<Root>("maps", jsonSerializerOptions);

            return root?.List;
        }
    }
}
