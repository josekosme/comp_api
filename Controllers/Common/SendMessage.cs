using System.Text;
using RabbitMQ.Client;

public class RabbitMQHelper
{

        public string _HostName { get; set; }
        public string _UserName { get; set; }
        public string _Password { get; set; }

        public RabbitMQHelper()
        {
            _HostName = "191.252.61.113";
            _UserName = "admin";
            _Password = "123456";
        }

        public void sendMessage()
        {
            var factory = new ConnectionFactory() { HostName = _HostName, UserName= _UserName, Password= _Password };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "hello",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
                string message = "Hello World!";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                 routingKey: "hello",
                                 basicProperties: null,
                                 body: body);
            }
        }
}
