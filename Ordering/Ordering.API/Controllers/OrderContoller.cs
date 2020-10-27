using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Commands;
using Ordering.Application.Queries;
using Ordering.Application.Responses;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Ordering.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<OrderReponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<OrderReponse>>> GetOrdersByUsername(string userName)
        {
            var q = new GetOrderByUserNameQuery(userName);
            var orders = await _mediator.Send(q);
            return Ok(orders);
        }

        [HttpPost]
        [ProducesResponseType(typeof(OrderReponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<OrderReponse>> CheckoutOrder([FromBody] CheckoutOrderCommand cmd)
        {
            var res = await _mediator.Send(cmd);
            return Ok(res);
        }
    }
}
