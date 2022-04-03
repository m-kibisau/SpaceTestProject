using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SpaceTestProject.Application.Models.ImdbApiService;
using SpaceTestProject.Application.Options;

namespace SpaceTestProject.Application.Services.ImdbApiService
{
    public class ImdbApiService : IImdbApiService
    {
        private readonly ILogger<ImdbApiService> _logger; 
        private readonly ImdbSettingsOptions _imdbSettingsOptions;
        private readonly IHttpClientFactory _httpClientFactory;

        private const string SearchByNameUrl = "/Search/{0}/{1}";
        private const string GetTitleByIdUrl = "/Title/{0}/{1}/{2}";
        private const string GetPosterByIdUrl = "/Posters/{0}/{1}";
        private const string GetWikipediaDescriptionByIdUrl = "/Wikipedia/{0}/{1}";

        public ImdbApiService(IOptions<ImdbSettingsOptions> imdbSettingsOptions, 
            IHttpClientFactory httpClientFactory, ILogger<ImdbApiService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _imdbSettingsOptions = imdbSettingsOptions.Value;
        }

        public async Task<SearchData> SearchByName(string expression)
        {
            try
            {
                var path = string.Format(SearchByNameUrl, _imdbSettingsOptions.ApiKey, expression);

                var httpClient = _httpClientFactory.CreateClient();
                var result = await httpClient.GetAsync(string.Concat(_imdbSettingsOptions.BaseImdbUrl, path));

                if (!result.IsSuccessStatusCode)
                {
                    return new SearchData()
                    {
                        ErrorMessage = "Error with http request to external url"
                    };
                }

                var responseContent = await result.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<SearchData>(responseContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error with SearchByName.");
                return new SearchData()
                {
                    ErrorMessage = "Error with SearchByName."
                };
            }
        }

        public async Task<TitleData> GetTitleById(string id, ImmutableList<string> options = null)
        {
            try
            {
                var joinOptions = string.Join(",", options ?? ImmutableList.Create<string>());

                var path = string.Format(GetTitleByIdUrl, _imdbSettingsOptions.ApiKey, id, joinOptions);

                var httpClient = _httpClientFactory.CreateClient();
                var result = await httpClient.GetAsync(string.Concat(_imdbSettingsOptions.BaseImdbUrl, path));

                if (!result.IsSuccessStatusCode)
                {
                    return new TitleData()
                    {
                        ErrorMessage = "Error with http request to external url"
                    };
                }

                var responseContent = await result.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<TitleData>(responseContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error with GetTitleById.");
                return new TitleData()
                {
                    ErrorMessage = "Error with GetTitleById."
                };
            }
        }

        public async Task<PosterData> GetPosterById(string titleId)
        {
            try
            {
                var path = string.Format(GetPosterByIdUrl, _imdbSettingsOptions.ApiKey, titleId);

                var httpClient = _httpClientFactory.CreateClient();
                var result = await httpClient.GetAsync(string.Concat(_imdbSettingsOptions.BaseImdbUrl, path));

                if (!result.IsSuccessStatusCode)
                {
                    return new PosterData()
                    {
                        ErrorMessage = "Error with http request to external url"
                    };
                }

                var responseContent = await result.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<PosterData>(responseContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error with GetPosterById.");
                return new PosterData()
                {
                    ErrorMessage = "Error with GetPosterById."
                };
            }
        }

        public async Task<WikipediaData> GetWikipediaDescriptionById(string id)
        {
            try
            {
                var path = string.Format(GetWikipediaDescriptionByIdUrl, _imdbSettingsOptions.ApiKey, id);

                var httpClient = _httpClientFactory.CreateClient();
                var result = await httpClient.GetAsync(string.Concat(_imdbSettingsOptions.BaseImdbUrl, path));

                if (!result.IsSuccessStatusCode)
                {
                    return new WikipediaData()
                    {
                        ErrorMessage = "Error with http request to external url"
                    };
                }

                var responseContent = await result.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<WikipediaData>(responseContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error with GetPosterById.");
                return new WikipediaData()
                {
                    ErrorMessage = "Error with GetPosterById."
                };
            }
        }

    }
}