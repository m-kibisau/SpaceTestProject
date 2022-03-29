using System.Collections.Generic;
using System.Collections.Immutable;
using SpaceTestProject.Application.Models.ImdbApiService;

namespace SpaceTestProject.Application.Services.ImdbApiService
{
    public interface IImdbApiService
    {
        SearchData SearchByName(string expression);
        TitleData GetTitleById(string id, ImmutableList<string> options);
        PosterData GetPosterById(string titleId);
        WikipediaData GetWikipediaDescriptionById(string id);
    }
}
