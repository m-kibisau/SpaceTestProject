using System;

namespace SpaceTestProject.Domain
{
    public class WatchListItem
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public string TitleId { get; set; }
        public DateTime CreationTime { get; set; }
        public bool IsWatched { get; set; }

    }
}
