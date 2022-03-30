using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using SpaceTestProject.Application.Models.ImdbApiService;

namespace SpaceTestProject.Application.Services.ImdbApiService
{
    public interface IImdbApiService
    {
        Task<SearchData> SearchByName(string expression);
        Task<TitleData> GetTitleById(string id, ImmutableList<string> options);
        Task<PosterData> GetPosterById(string titleId);
        Task<WikipediaData> GetWikipediaDescriptionById(string id);
    }
}
