
using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory=new ConnectionFactory();
factory.Uri = new Uri("amqps://xtdxdiyy:wJSGJ8qvPSgIFVBDDiJYu5a5w4PiI8qq@rattlesnake.rmq.cloudamqp.com/xtdxdiyy");

using IConnection connection= factory.CreateConnection();
using IModel channel= connection.CreateModel();

string exchangeName="topic-exchange-example";

channel.ExchangeDeclare(
    exchange: exchangeName, 
    type: ExchangeType.Topic);

for (int i = 0; i < 50; i++)
{
    await Task.Delay(300);
    byte[] message= Encoding.UTF8.GetBytes($"{i + 1}.Hello RabbitMQ!");
    Console.Write("Mesajın Gönderileceği Topic Formatını Belirtiniz : ");
    string topic = Console.ReadLine();

    channel.BasicPublish(
        exchange: exchangeName, 
        routingKey: topic, 
        basicProperties: null, 
        body: message);
    
}


Console.Read();