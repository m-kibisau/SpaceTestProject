using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SpaceTestProject.Application.Models.CommonResponse;
using SpaceTestProject.Application.Models.WatchListItems;

namespace SpaceTestProject.Application.WatchListItems.Queries.GetByUserId
{
    public class GetWatchListItemsByUserIdQuery : IRequest<Result<List<WatchListItemDto>>>
    {
        public int UserId { get; set; }

        public GetWatchListItemsByUserIdQuery(int userId)
        {
            UserId = userId;
        }
    }
}
