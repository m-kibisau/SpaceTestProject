using System.Collections.Immutable;
using SpaceTestProject.Application.Models.ImdbApiService;

namespace SpaceTestProject.Application.Services.ImdbApiService
{
    public class ImdbApiService : IImdbApiService
    {
        public SearchData SearchByName(string expression)
        {
            throw new System.NotImplementedException();
        }

        public TitleData GetTitleById(string id, ImmutableList<string> options)
        {
            throw new System.NotImplementedException();
        }

        public PosterData GetPosterById(string titleId)
        {
            throw new System.NotImplementedException();
        }

        public WikipediaData GetWikipediaDescriptionById(string id)
        {
            throw new System.NotImplementedException();
        }
    }
}