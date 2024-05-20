using RabbitMQ.Client;
using System.Reflection.PortableExecutable;
using System.Text;

ConnectionFactory factory= new ConnectionFactory();
factory.Uri = new Uri("amqps://xtdxdiyy:wJSGJ8qvPSgIFVBDDiJYu5a5w4PiI8qq@rattlesnake.rmq.cloudamqp.com/xtdxdiyy");
using IConnection conn = factory.CreateConnection();
using IModel channel = conn.CreateModel();

string exchangeName="header-exchange";

channel.ExchangeDeclare(
    exchange:exchangeName,
    type:ExchangeType.Headers);


for (int i = 0; i < 50; i++)
{
    await Task.Delay(300);
    byte[] messageBodyBytes = Encoding.UTF8.GetBytes($"Message {i+1}");
    Console.Write("Lütfen header value'sunu girinizi : ");
    string headerValue = Console.ReadLine();



//headers:
//    new Dictionary<string, object>
//        {
//            {"format","pdf"},
//            {"shape","a4"},
//            {"x-match","any"},
//            {"headerValue",headerValue}
//        });

IBasicProperties basicProperties=channel.CreateBasicProperties();
    basicProperties.Headers = new Dictionary<string, object>
    {
        ["no"] = headerValue //Birden fazla header datası olabilir
    };

    channel.BasicPublish(
        exchange: exchangeName,
        routingKey: string.Empty,
        basicProperties: basicProperties,
        body: messageBodyBytes
        );
}


Console.Read();
