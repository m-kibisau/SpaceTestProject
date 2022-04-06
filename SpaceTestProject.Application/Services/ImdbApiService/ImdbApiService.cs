using System;
using System.Collections.Immutable;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SpaceTestProject.Application.Extensions;
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

                var result =
                    await _httpClientFactory.GetRequestAsync<SearchData>(
                        string.Concat(_imdbSettingsOptions.BaseImdbUrl, path));

                if (!result.IsSuccess())
                {
                    return new SearchData()
                    {
                        ErrorMessage = result.GetMessageSummary()
                    };
                }

                return result.Data;
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

                var result = await _httpClientFactory.GetRequestAsync<TitleData>(string.Concat(_imdbSettingsOptions.BaseImdbUrl, path));
                
                if (!result.IsSuccess())
                {
                    return new TitleData()
                    {
                        ErrorMessage = result.GetMessageSummary()
                    };
                }

                return result.Data;
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

                var result = await _httpClientFactory.GetRequestAsync<PosterData>(string.Concat(_imdbSettingsOptions.BaseImdbUrl, path));

                if (!result.IsSuccess())
                {
                    return new PosterData()
                    {
                        ErrorMessage = result.GetMessageSummary()
                    };
                }

                return result.Data;
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

                var result = await _httpClientFactory.GetRequestAsync<WikipediaData>(string.Concat(_imdbSettingsOptions.BaseImdbUrl, path));

                if (!result.IsSuccess())
                {
                    return new WikipediaData()
                    {
                        ErrorMessage = result.GetMessageSummary()
                    };
                }

                return result.Data;
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