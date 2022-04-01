using System;

namespace SpaceTestProject.Application.Models.WatchListItems
{
    public class WatchListItemDto
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public string TitleId { get; set; }
        public DateTime CreationTime { get; set; }
        public bool IsWatched { get; set; }
    }
}
