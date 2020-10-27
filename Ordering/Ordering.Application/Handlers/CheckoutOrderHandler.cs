using MediatR;
using Ordering.Application.Commands;
using Ordering.Application.Mapper;
using Ordering.Application.Responses;
using Ordering.Core.Entities;
using Ordering.Core.Repos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Handlers
{
    public class CheckoutOrderHandler : IRequestHandler<CheckoutOrderCommand, OrderReponse>
    {
        private readonly IOrderRepo _orderRepo;

        public CheckoutOrderHandler(IOrderRepo orderRepo)
        {
            _orderRepo = orderRepo ?? throw new ArgumentNullException(nameof(orderRepo));
        }

        public async Task<OrderReponse> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = OrderMapper.Mapper.Map<Order>(request);

            if (orderEntity == null)
                throw new ApplicationException("cannot map");

            var newOrder = await _orderRepo.AddAsync(orderEntity);
            var orderReponse = OrderMapper.Mapper.Map<OrderReponse>(newOrder);
            return orderReponse;
        }
    }
}
