using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SpaceTestProject.Application.Infrastructure;
using SpaceTestProject.Application.Models.CommonResponse;
using SpaceTestProject.Application.Models.WatchListItems;
using SpaceTestProject.Application.WatchListItems.Specifications;
using SpaceTestProject.Domain;
using SpaceTestProject.Persistence.Abstractions;
using SpaceTestProject.Persistence.Abstractions.Enums;

namespace SpaceTestProject.Application.WatchListItems.Queries.GetByUserId
{
    public class GetWatchListItemsByUserIdQueryHandler : IRequestHandler<GetWatchListItemsByUserIdQuery, Result<List<WatchListItemDto>>>
    {
        private readonly IRepository<WatchListItem> _repository;

        public GetWatchListItemsByUserIdQueryHandler(IRepository<WatchListItem> repository)
        {
            _repository = repository;
        }

        public async Task<Result<List<WatchListItemDto>>> Handle(GetWatchListItemsByUserIdQuery request, CancellationToken cancellationToken)
        {
            if (request.UserId <= 0)
            {
                return Result<List<WatchListItemDto>>.Fail("Incorrect user id. It can be less or equal 0");
            }

            var watchListItems = await _repository.GetAllAsync(new GetWatchListItemByUserIdSpecification(request.UserId),
                new Sorting<WatchListItem>(x => x.CreationTime, SortingType.Descending));

            var itemsDto = watchListItems.Select(x => x.MapToDto()).ToList();

            return Result<List<WatchListItemDto>>.Success(itemsDto);
        }
    }
}
