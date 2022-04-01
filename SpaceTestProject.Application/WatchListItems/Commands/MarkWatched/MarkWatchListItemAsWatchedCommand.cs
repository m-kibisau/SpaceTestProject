using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SpaceTestProject.Application.Models.CommonResponse;

namespace SpaceTestProject.Application.WatchListItems.Commands.MarkWatched
{
    public class MarkWatchListItemAsWatchedCommand : IRequest<Result>
    {
        public Guid Id { get; set; }

        public MarkWatchListItemAsWatchedCommand(Guid id)
        {
            Id = id;
        }
    }
}
