using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory=new ConnectionFactory();
factory.Uri = new Uri("amqps://xtdxdiyy:wJSGJ8qvPSgIFVBDDiJYu5a5w4PiI8qq@rattlesnake.rmq.cloudamqp.com/xtdxdiyy");

using IConnection connection=factory.CreateConnection();
using IModel channel=connection.CreateModel();

channel.ExchangeDeclare(
    exchange: "fanout-exchange-example", 
    type: ExchangeType.Fanout);

for (int i = 0; i < 100; i++)
{
    await Task.Delay(200);
    string message = $"{i + 1}.Hello RabbitMQ!";
    byte[] body = Encoding.UTF8.GetBytes(message);
    channel.BasicPublish(exchange:"fanout-exchange-example",
        routingKey:string.Empty,// Exchange bind olmuş tüm kuyruklara yolladığımız için routingKey'i boş geçiyoruz.
        body:body
        ); 
}


Console.Read();
