using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SpaceTestProject.Application.Models.CommonResponse;
using SpaceTestProject.Application.WatchListItems.Specifications;
using SpaceTestProject.Domain;
using SpaceTestProject.Persistence.Abstractions;

namespace SpaceTestProject.Application.WatchListItems.Commands.MarkWatched
{
    public class MarkWatchListItemAsWatchedCommandHandler : IRequestHandler<MarkWatchListItemAsWatchedCommand, Result>
    {
        private readonly ILogger<MarkWatchListItemAsWatchedCommandHandler> _logger;
        private readonly IRepository<WatchListItem> _repository;

        public MarkWatchListItemAsWatchedCommandHandler(ILogger<MarkWatchListItemAsWatchedCommandHandler> logger,
            IRepository<WatchListItem> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<Result> Handle(MarkWatchListItemAsWatchedCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var watchListRecord = await _repository.GetAsync(new GetWatchListItemByIdSpecification(request.Id));

                if (watchListRecord == null)
                {
                    var errorMessage = $"Watchlist item with id {request.Id} doesn't exist";
                    _logger.LogError(errorMessage);
                    return Result.Fail(errorMessage);
                }

                if (watchListRecord.IsWatched)
                {
                    var errorMessage = $"Watchlist item with id {request.Id} have already been marked as watched";
                    _logger.LogError(errorMessage);
                    return Result.Fail(errorMessage);
                }

                watchListRecord.IsWatched = true;

                var result = await _repository.UpdateAsync(watchListRecord);
                return result ? Result.Success() : Result.Fail("Failed to update watch list item");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error to add new item. Details: ");
                return Result.Fail(ex.Message);
            }
        }
    }
}
