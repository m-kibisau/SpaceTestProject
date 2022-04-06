using System;

namespace SpaceTestProject.Domain
{
    public class WatchListEmailLog
    {
        public Guid Id { get; set; }
        public Guid WatchListId { get; set; }
        public DateTime SendingTime { get; set; }

        public WatchListItem WatchList { get; set; }
    }
}
