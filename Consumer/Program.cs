using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Consumer {
    class Program {
        static void Main (string[] args) {
            var factory = new ConnectionFactory () { HostName = "localhost" };
            using (var connection = factory.CreateConnection ())
            using (var channel = connection.CreateModel ()) {
                channel.QueueDeclare (queue: "qwebapp",
                    durable : false,
                    exclusive : false,
                    autoDelete : false,
                    arguments : null);

                var consumer = new EventingBasicConsumer (channel);
                consumer.Received += (model, ea) => {

                    {
                        try

                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString (body);
                        Console.WriteLine (" [x] Received {0}", message);
                    } catch (Exception ex) {
                        //em caso de ex retorno pra fila

                        channel.BasicAck (ea.DeliveryTag, false);
                    }
                };
                channel.BasicConsume (queue: "qwebapp",
                    autoAck : false,
                    consumer : consumer);

                Console.WriteLine (" Press [enter] to exit.");
                Console.ReadLine ();
            };
        }
    }
}
}