using AutoMapper;
using Basket.API.Entities;
using Basket.API.Repos.Interfaces;
using EventBusRabbitMQ.Common;
using EventBusRabbitMQ.Events;
using EventBusRabbitMQ.Producer;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Basket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _repo;
        private readonly IMapper _mapper;
        private readonly EventBusRabbitMQProducer _eventBus;

        public BasketController(IBasketRepository repo, IMapper mapper, EventBusRabbitMQProducer eventBus)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        [HttpGet]
        [ProducesResponseType(typeof(BasketCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<BasketCart>> GetBascket(string userName)
        {
            var b = await _repo.GetBasket(userName);
            return Ok(b ?? new BasketCart(userName));
        }

        [HttpPost]
        [ProducesResponseType(typeof(BasketCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<BasketCart>> UpdateBascket([FromBody]BasketCart basket)
        {
            var res = await _repo.UpdateBasket(basket);
            return Ok(res);
        }

        [HttpDelete("{userName}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> DeleteBascket(string userName)
        {
            var res = await _repo.Delete(userName);
            return Ok(res);
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        public async Task<ActionResult> CheckOut([FromBody] BasketCheckout basket)
        {
            var b = await _repo.GetBasket(basket.UserName);
            if (b == null)
                return BadRequest();

            var bRemoved = await _repo.Delete(b.UserName);
            if (!bRemoved)
                return BadRequest();

            var eventMessage = _mapper.Map<BasketChekoutEvent>(basket);
            eventMessage.RequestId = Guid.NewGuid();
            eventMessage.TotalPrice = b.TotalPrice;

            try
            {
                _eventBus.PublishBasketCheckout(EventBusConsts.BasketCheckoutQueue, eventMessage);
            }
            catch (Exception)
            {

                throw;
            }

            return Accepted();
        }       
    }
}
