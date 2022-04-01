using MediatR;
using SpaceTestProject.Application.Models.CommonResponse;
using SpaceTestProject.Application.Models.WatchListItems;

namespace SpaceTestProject.Application.WatchListItems.Commands.Add
{
    public class AddWatchListItemCommand : IRequest<Result<WatchListItemDto>>
    {
        public int UserId { get; set; }
        public string TitleId { get; set; }
    }
}
