using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SpaceTestProject.Application.Titles.Queries.GetAll;

namespace SpaceTestProject.Api.Controllers
{
    /// <summary>
    /// Search titles in IMDB
    /// </summary>
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
        /// Search films by name
        /// </summary>
        [HttpGet]
        [Route("searchByName")]
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
