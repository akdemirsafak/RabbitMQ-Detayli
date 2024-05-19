using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory= new();
factory.Uri = new Uri("amqps://xtdxdiyy:wJSGJ8qvPSgIFVBDDiJYu5a5w4PiI8qq@rattlesnake.rmq.cloudamqp.com/xtdxdiyy");

using IConnection connection= factory.CreateConnection();
using IModel channel= connection.CreateModel();

string exchangeName="topic-exchange-example";

channel.ExchangeDeclare(
    exchange: exchangeName, 
    type: ExchangeType.Topic);

Console.Write("Dinlenecek topic formatını belirtiniz : ");
string topic = Console.ReadLine();
string queueName=channel.QueueDeclare().QueueName; // Burada kuyruk adı belirtilmediği için rastgele bir kuyruk adı oluşturulur.

channel.QueueBind(
    queue: queueName, 
    exchange: exchangeName, 
    routingKey: topic);

EventingBasicConsumer consumer= new(channel);

channel.BasicConsume(
    queue: queueName, 
    autoAck: true, 
    consumer: consumer);
consumer.Received += (object sender, BasicDeliverEventArgs e) =>
{
    byte[] message = e.Body.ToArray();
    string messageText = Encoding.UTF8.GetString(message);
    Console.WriteLine($"Gelen Mesaj : {messageText}");
};
Console.Read();
