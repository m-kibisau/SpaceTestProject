using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SpaceTestProject.Application.Infrastructure;
using SpaceTestProject.Application.Models.CommonResponse;
using SpaceTestProject.Application.Models.WatchListItems;
using SpaceTestProject.Application.Services.ImdbApiService;
using SpaceTestProject.Application.WatchListItems.Specifications;
using SpaceTestProject.Domain;
using SpaceTestProject.Persistence.Abstractions;

namespace SpaceTestProject.Application.WatchListItems.Commands.Add
{
    public class AddWatchListItemCommandHandler : IRequestHandler<AddWatchListItemCommand, Result<WatchListItemDto>>
    {
        private readonly ILogger<AddWatchListItemCommandHandler> _logger;
        private readonly IRepository<WatchListItem> _repository;
        private readonly IImdbApiService _imdbApiService;

        public AddWatchListItemCommandHandler(IRepository<WatchListItem> repository, 
            IImdbApiService imdbApiService, 
            ILogger<AddWatchListItemCommandHandler> logger)
        {
            _repository = repository;
            _imdbApiService = imdbApiService;
            _logger = logger;
        }

        public async Task<Result<WatchListItemDto>> Handle(AddWatchListItemCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (await _repository.AnyAsync(
                        new GetWatchListItemByUserIdAndTitleIdSpecification(request.UserId, request.TitleId)))
                {
                    return Result<WatchListItemDto>.Fail("This title have already been added to user's watch list");
                }

                var checkTitle = await _imdbApiService.GetTitleById(request.TitleId);

                if (!string.IsNullOrEmpty(checkTitle.ErrorMessage))
                {
                    var errorMessage = $"Error to get Title from Imdb. Error: {checkTitle.ErrorMessage}";
                    _logger.LogError(errorMessage);
                    return Result<WatchListItemDto>.Fail(errorMessage);
                }

                var model = request.MapToDomain();
                model.CreationTime = DateTime.UtcNow;
                model.IsWatched = false;

                var saveResult = await _repository.CreateAsync(model);

                if (!saveResult)
                {
                    _logger.LogError("Error to save new record in db");
                    return Result<WatchListItemDto>.Fail("Error to save new record in db");
                }

                return Result<WatchListItemDto>.Success(model.MapToDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error to add new item. Details: ");
                return Result<WatchListItemDto>.Fail(ex.Message);
            }
        }
    }
}
