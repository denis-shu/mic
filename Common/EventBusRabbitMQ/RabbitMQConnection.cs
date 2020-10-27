using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace EventBusRabbitMQ
{
    public class RabbitMQConnection : IRabbitMQConnection
    {
        private readonly IConnectionFactory _conFac;
        private IConnection _connection;
        private bool _disposed;

        public RabbitMQConnection(IConnectionFactory conFac)
        {
            _conFac = conFac ?? throw new ArgumentNullException(nameof(conFac));
            if (!IsConnected)
            {
                TryConnect();
            }
        }



        public bool IsConnected
        {
            get
            {
                return _connection != null && _connection.IsOpen && !_disposed;
            }
        }

        public bool TryConnect()
        {
            try
            {
                _connection = _conFac.CreateConnection();
            }
            catch (BrokerUnreachableException d)
            {
                Thread.Sleep(2000);
                _connection = _conFac.CreateConnection();
            }
            if (IsConnected)
                return true;
            else
                return false;
        }

        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("NO krolik connection");
            }
            return _connection.CreateModel();
        }

        public void Dispose()
        {
            if (_disposed)
                return;
            try
            {
                _connection.Dispose();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
