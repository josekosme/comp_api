using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Framing.Impl;
using System;
using System.Diagnostics;
using System.Text;

namespace comp_api.common
{
    public class RabbitMQHelper 
    {
        private static ConnectionFactory _ConnectionFactory;
        private static IConnection _Connection;
        private static IModel _Channel;
        public static string _QueueName { get; set; }
        private static EventingBasicConsumer _Consumer;

        public static string _HostName { get; set; }
        public static string _UserName { get; set; }
        public static string _Password { get; set; }

        public delegate void DomainEventListener(string Message);
        private static IDomainEventListenerAppService _DomainEventListener;

        public RabbitMQHelper()
        {
            init();
        }

        public static void init()
        {
            _HostName = "191.252.61.113";
            _UserName = "admin";
            _Password = "123456";
            _QueueName = _QueueName;
        }

        public void connect()
        {
            _ConnectionFactory = new ConnectionFactory() { HostName = _HostName, UserName = _UserName, Password = _Password };
            _ConnectionFactory.AutomaticRecoveryEnabled = true;
            _ConnectionFactory.NetworkRecoveryInterval = TimeSpan.FromSeconds(15);

            _Connection = _ConnectionFactory.CreateConnection();
            _Channel = _Connection.CreateModel();
        }

        public void sendMessage(string message)
        {
            _Channel.QueueDeclare(queue: _QueueName,
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            var body = Encoding.UTF8.GetBytes(message);

            _Channel.BasicPublish(exchange: "",
                                 routingKey: _QueueName,
                                 basicProperties: null,
                                 body: body);
        }

        public static void StartConsumer(string exchangeName)
        {
            init();

            //var factory = new ConnectionFactory
            //{
            //    HostName = _HostName,
            //    Port = 5672,
            //    UserName = _UserName,
            //    Password = _Password,
            //    VirtualHost = "/",
            //    AutomaticRecoveryEnabled = true,
            //    NetworkRecoveryInterval = TimeSpan.FromSeconds(15)
            //};

            _ConnectionFactory = new ConnectionFactory() { HostName = _HostName, UserName = _UserName, Password = _Password };
            _Connection = _ConnectionFactory.CreateConnection();

            _Channel = _Connection.CreateModel();
            _QueueName = _Channel.QueueDeclare(durable: false,
                                             exclusive: false,
                                             autoDelete: false,
                                             arguments: null).QueueName;

            _Channel.QueueBind(queue: _QueueName,
                             exchange: exchangeName,
                             routingKey: "");


            _Consumer = new EventingBasicConsumer(_Channel);
            _Channel.BasicConsume(queue: _QueueName,
                                 noAck: true,
                                 consumer: _Consumer);

            _Consumer.Received += ConsumerOnReceived;

            _Channel.BasicConsume(queue: _QueueName,
                noAck: true,
                consumer: _Consumer);
        }

        public static void Stop()
        {
            _Channel.Close(200, "Goodbye");
            _Connection.Close();
        }

        public static void startExchange(string exchangeName)
        {
            _ConnectionFactory = new ConnectionFactory() { HostName = _HostName, UserName = _UserName, Password = _Password };
            RabbitMQHelper._Connection = _ConnectionFactory.CreateConnection();
            RabbitMQHelper._Channel = _Connection.CreateModel();
            RabbitMQHelper._Channel.ExchangeDeclare(exchange: exchangeName, type: "fanout");

            var queueName = RabbitMQHelper._Channel.QueueDeclare().QueueName;
            RabbitMQHelper._Channel.QueueBind(queue: queueName,
                              exchange: exchangeName,
                              routingKey: "");
        }

        public static void publisher(string exchangeName, string message)
        {
            startExchange(exchangeName);
            var body = Encoding.UTF8.GetBytes(message);
            RabbitMQHelper._Channel.BasicPublish(exchange: exchangeName,
                                 routingKey: "",
                                 basicProperties: null,
                                 body: body);
        }

        private static void ConsumerOnReceived(object sender, BasicDeliverEventArgs ea)
        {
            var body = ea.Body;
            var eventMessage = Encoding.UTF8.GetString(body);
            Debug.WriteLine("{0}", eventMessage);

            if (_DomainEventListener != null)
            {
                _DomainEventListener.processEvent(eventMessage);
            }
        }

        public static void setDomainEventListener(IDomainEventListenerAppService eventDrivenConsumerDelegate)
        {
            _DomainEventListener = eventDrivenConsumerDelegate;
        }

        public static void setQueueName(string queueName)
        {
            _QueueName = queueName;
        }

        public static void startPublishSubscribe(string exchangeName)
        {
            _ConnectionFactory = new ConnectionFactory() { HostName = _HostName, UserName = _UserName, Password = _Password };
            _Connection = _ConnectionFactory.CreateConnection();
            _Channel = _Connection.CreateModel();
            RabbitMQHelper._Channel.ExchangeDeclare(exchange: exchangeName, type: "fanout");

            _QueueName = _Channel.QueueDeclare(durable: false,
                                                   exclusive: false,
                                                   autoDelete: false,
                                                   arguments: null).QueueName;
            _Channel.QueueBind(queue: _QueueName,
                                exchange: exchangeName,
                                routingKey: "");


            _Consumer = new EventingBasicConsumer(_Channel);
            _Channel.BasicConsume(queue: _QueueName,
                                 noAck: true,
                                 consumer: _Consumer);

            _Consumer.Received += ConsumerOnReceived;

            _Channel.BasicConsume(queue: _QueueName,
                noAck: true,
                consumer: _Consumer);
        }

    }
}
