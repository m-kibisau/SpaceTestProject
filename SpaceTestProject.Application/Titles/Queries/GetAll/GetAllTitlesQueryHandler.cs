using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SpaceTestProject.Application.Infrastructure;
using SpaceTestProject.Application.Models.CommonResponse;
using SpaceTestProject.Application.Models.Titles;
using SpaceTestProject.Application.Services.ImdbApiService;

namespace SpaceTestProject.Application.Titles.Queries.GetAll
{
    public class GetAllTitlesQueryHandler : IRequestHandler<GetAllTitlesQuery, Result<List<TitleView>>>
    {
        private readonly IImdbApiService _imdbApiService;

        public GetAllTitlesQueryHandler(IImdbApiService imdbApiService)
        {
            _imdbApiService = imdbApiService;
        }

        public async Task<Result<List<TitleView>>> Handle(GetAllTitlesQuery request, CancellationToken cancellationToken)
        {
            var searchResult = await _imdbApiService.SearchByName(request.Expression);

            if (!string.IsNullOrEmpty(searchResult.ErrorMessage))
            {
                return Result<List<TitleView>>.Fail(searchResult.ErrorMessage);
            }

            return Result<List<TitleView>>.Success(searchResult.Map());
        }
    }
}
