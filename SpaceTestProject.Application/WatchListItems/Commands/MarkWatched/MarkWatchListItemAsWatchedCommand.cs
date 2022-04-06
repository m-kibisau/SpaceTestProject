using System;
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
