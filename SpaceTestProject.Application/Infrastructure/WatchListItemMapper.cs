using SpaceTestProject.Application.Models.WatchListItems;
using SpaceTestProject.Application.WatchListItems.Commands.Add;
using SpaceTestProject.Domain;

namespace SpaceTestProject.Application.Infrastructure
{
    public static class WatchListItemMapper
    {
        public static WatchListItem MapToDomain(this AddWatchListItemCommand command)
        {
            return new WatchListItem()
            {
                UserId = command.UserId,
                TitleId = command.TitleId
            };
        }

        public static WatchListItemDto MapToDto(this WatchListItem item)
        {
            return new WatchListItemDto()
            {
                Id = item.Id,
                TitleId = item.TitleId,
                CreationTime = item.CreationTime,
                IsWatched = item.IsWatched,
                UserId = item.UserId
            };
        }
    }
}
