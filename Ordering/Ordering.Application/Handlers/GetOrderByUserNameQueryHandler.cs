using MediatR;
using Ordering.Application.Mapper;
using Ordering.Application.Queries;
using Ordering.Application.Responses;
using Ordering.Core.Repos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Handlers
{
    public class GetOrderByUserNameQueryHandler : IRequestHandler<GetOrderByUserNameQuery, IEnumerable<OrderReponse>>
    {
        private readonly IOrderRepo _orderRepo;

        public GetOrderByUserNameQueryHandler(IOrderRepo orderRepo)
        {
            _orderRepo = orderRepo ?? throw new ArgumentNullException(nameof(orderRepo));
        }

        public async Task<IEnumerable<OrderReponse>> Handle(GetOrderByUserNameQuery request, CancellationToken cancellationToken)
        {
            var orderList = await _orderRepo.GetOrdersByUserName(request.UserName);

            var orderResposeList = OrderMapper.Mapper.Map<IEnumerable<OrderReponse>>(orderList);

            return orderResposeList;
        }
    }
}
