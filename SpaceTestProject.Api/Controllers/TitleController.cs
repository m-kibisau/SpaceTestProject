using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SpaceTestProject.Application.Services.ImdbApiService;
using SpaceTestProject.Application.Titles.Queries.GetAll;

namespace SpaceTestProject.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TitleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TitleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Поиск фильмов по названию
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult> Search([FromQuery] string expression)
        {
            var result = await _mediator.Send(new GetAllTitlesQuery(expression));

            if (!result.IsSuccess())
            {
                return BadRequest(result.GetMessageSummary());
            }

            return Ok(result.Data);
        }
    }
}
