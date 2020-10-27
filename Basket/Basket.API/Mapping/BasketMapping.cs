

using AutoMapper;
using EventBusRabbitMQ.Events;

namespace Basket.API.Mapping
{
    public class BasketMapping: Profile
    {
        public BasketMapping()
        {
            CreateMap<Entities.BasketCheckout, BasketChekoutEvent>().ReverseMap();
        }
    }
}
