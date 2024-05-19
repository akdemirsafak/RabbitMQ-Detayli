using System.Text;
using RabbitMQ.Client;

ConnectionFactory factory = new();
factory.Uri = new Uri("amqps://xtdxdiyy:wJSGJ8qvPSgIFVBDDiJYu5a5w4PiI8qq@rattlesnake.rmq.cloudamqp.com/xtdxdiyy");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel(); // IDisposible olduğu için using ile kullanma imkanına sahibiz.

//Exchange oluşturalım.
channel.ExchangeDeclare(exchange: "direct-exchange-example", type: ExchangeType.Direct);

while (true)
{
    System.Console.WriteLine("Mesaj : ");
    string message = Console.ReadLine();
    byte[] body = Encoding.UTF8.GetBytes(message);
    channel.BasicPublish(
        exchange: "direct-exchange-example",
        routingKey: "direct-queue-example", // Direct exchange'in mantığı gereği routingKey belirttik.
        basicProperties: null, body: body);
}
//Kuyruk oluşturma davranışı sergilenmez.


Console.Read();
