using AutoMapper;
using EventBusRabbitMQ;
using EventBusRabbitMQ.Common;
using EventBusRabbitMQ.Events;
using MediatR;
using Newtonsoft.Json;
using Ordering.Application.Commands;
using Ordering.Core.Repos;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Ordering.API.RabbitMQ
{
    public class EventBusRabbitMQConsumer
    {
        private readonly IRabbitMQConnection _connection;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IOrderRepo _repo;

        public EventBusRabbitMQConsumer(IRabbitMQConnection connection, IMediator mediator, IMapper mapper, IOrderRepo repo)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public void Consume()
        {
            var channel = _connection.CreateModel();
            channel.QueueDeclare(queue: EventBusConsts.BasketCheckoutQueue,
                durable: false, exclusive: false,
                autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += RecievedEvent;

            channel.BasicConsume(queue: EventBusConsts.BasketCheckoutQueue,
                autoAck: true, consumer: consumer, noLocal: false, exclusive: false, arguments: null);
        }

        private async void RecievedEvent(object sender, BasicDeliverEventArgs e)
        {
            if (e.RoutingKey == EventBusConsts.BasketCheckoutQueue)
            {
                var msg = Encoding.UTF8.GetString(e.Body.Span);
                var basketCheckoutEvent = JsonConvert.DeserializeObject<BasketChekoutEvent>(msg);
                var cmd = _mapper.Map<CheckoutOrderCommand>(basketCheckoutEvent);

                var res = await _mediator.Send(cmd);
            }
        }

        public void Disconnect()
        {
            _connection.Dispose();
        }
    }
}
