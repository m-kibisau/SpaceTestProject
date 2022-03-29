using System.Collections.Generic;

namespace SpaceTestProject.Application.Models.ImdbApiService
{
    public class TvSeriesInfo
    {
        public string YearEnd { set; get; }
        public string Creators { set; get; }
        public List<StarShort> CreatorList { get; set; }
        public List<string> Seasons { get; set; }
    }
}