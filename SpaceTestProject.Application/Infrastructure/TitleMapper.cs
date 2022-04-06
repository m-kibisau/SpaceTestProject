using System.Collections.Generic;
using System.Linq;
using SpaceTestProject.Application.Models.ImdbApiService;
using SpaceTestProject.Application.Models.Titles;

namespace SpaceTestProject.Application.Infrastructure
{
    public static class TitleMapper
    {
        public static List<TitleView> Map(this SearchData searchData)
        {
            return searchData.Results
                .Select(x => new TitleView
                {
                    Id = x.Id,
                    Description = x.Description,
                    Image = x.Image,
                    Title = x.Title
                })
                .ToList();
        }
    }
}
