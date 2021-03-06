using System.Collections.Generic;
using MediatR;
using SpaceTestProject.Application.Models.CommonResponse;
using SpaceTestProject.Application.Models.Titles;

namespace SpaceTestProject.Application.Titles.Queries.GetAll
{
    public class GetAllTitlesQuery : IRequest<Result<List<TitleView>>>
    {
        public string Expression { get; set; }

        public GetAllTitlesQuery(string expression)
        {
            Expression = expression;
        }
    }
}
