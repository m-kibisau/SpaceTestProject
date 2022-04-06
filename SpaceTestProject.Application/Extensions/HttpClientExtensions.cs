using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SpaceTestProject.Application.Models.CommonResponse;

namespace SpaceTestProject.Application.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<Result<T>> GetRequestAsync<T>(this IHttpClientFactory httpClientFactory, string path) 
            where T : class
        {
            var httpClient = httpClientFactory.CreateClient();
            var result = await httpClient.GetAsync(path);

            if (!result.IsSuccessStatusCode)
            {
                return Result<T>.Fail("Error with http request to external url");
            }

            var responseContent = await result.Content.ReadAsStringAsync();

            return Result<T>.Success(JsonConvert.DeserializeObject<T>(responseContent));
        }
    }
}
