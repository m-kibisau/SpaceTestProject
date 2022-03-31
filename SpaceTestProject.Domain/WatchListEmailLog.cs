using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
