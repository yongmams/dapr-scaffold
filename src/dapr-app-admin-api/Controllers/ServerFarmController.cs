﻿using DapApp.Admin.API.Models;
using DaprApp.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Json;

namespace DapApp.Admin.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ServerFarmController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory = null!;
        private readonly ILogger<ServerFarmController> _logger = null!;
        private readonly IConfiguration _configuration = null!;

        public ServerFarmController(
            IHttpClientFactory httpClientFactory,
            ILogger<ServerFarmController> logger,
            IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<PagingQuery<ServerFarm>> List()
        {
            using HttpClient client = _httpClientFactory.CreateClient("AuthHttpClient");
            client.BaseAddress = new Uri(_configuration.GetValue<string>("Services:K8sServiceBase"));

            var request = new HttpRequestMessage();
            request.Method = HttpMethod.Get;
            request.RequestUri = new Uri("api/Namespace/List", uriKind: UriKind.Relative);

            var res = await client.SendAsync(request);
            if (res.IsSuccessStatusCode)
            {
                string responseContent = await res.Content.ReadAsStringAsync();
                var list = JsonSerializer.Deserialize<IList<string>>(responseContent)
                    ?.Select(r => new ServerFarm() { Name = r });

                if (list != null)
                {
                    return new PagingQuery<ServerFarm>(list, 1, 10);
                }
            }

            throw new NotImplementedException();
        }
    }
}
