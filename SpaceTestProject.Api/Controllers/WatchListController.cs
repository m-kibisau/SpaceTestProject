using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SpaceTestProject.Application.WatchListItems.Commands.Add;
using SpaceTestProject.Application.WatchListItems.Commands.MarkWatched;
using SpaceTestProject.Application.WatchListItems.Queries.GetByUserId;

namespace SpaceTestProject.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WatchListController : ControllerBase
    {

        private readonly IMediator _mediator;

        public WatchListController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Add new item to watch list
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("add")]
        public async Task<ActionResult> AddWatchListItem([FromBody] AddWatchListItemCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess())
            {
                return BadRequest(result.GetMessageSummary());
            }

            return Ok(result.Data);
        }

        /// <summary>
        /// Mark record as watched
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}/MarkAsWatched")]
        public async Task<ActionResult> MarkWatchListItemAsWatched(Guid id)
        {
            var request = new MarkWatchListItemAsWatchedCommand(id);
            var result = await _mediator.Send(request);

            if (!result.IsSuccess())
            {
                return BadRequest(result.GetMessageSummary());
            }

            return Ok();
        }

        /// <summary>
        /// Get Watch list by user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("user/{userId}")]
        public async Task<ActionResult> GetWatchListByUserId(int userId)
        {
            var result = await _mediator.Send(new GetWatchListItemsByUserIdQuery(userId));

            if (!result.IsSuccess())
            {
                return BadRequest(result.GetMessageSummary());
            }

            return Ok(result.Data);
        }
    }
}
