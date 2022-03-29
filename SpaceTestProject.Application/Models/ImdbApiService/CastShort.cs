using System.Collections.Generic;

namespace SpaceTestProject.Application.Models.ImdbApiService
{
    public class CastShort
    {
        public string Job { get; set; }
        public List<CastShortItem> Items { get; set; }
    }
}