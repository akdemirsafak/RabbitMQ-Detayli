using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory=new ConnectionFactory();
factory.Uri= new Uri("amqps://xtdxdiyy:wJSGJ8qvPSgIFVBDDiJYu5a5w4PiI8qq@rattlesnake.rmq.cloudamqp.com/xtdxdiyy");

using IConnection connection=factory.CreateConnection();
using IModel channel=connection.CreateModel();

string exchangeName="fanout-exchange-example";

channel.ExchangeDeclare(
    exchange:exchangeName,
    ExchangeType.Fanout);

Console.WriteLine("Kuyruk adını giriniz : ");
string queueName=Console.ReadLine();

channel.QueueDeclare(
    queue: queueName,
    exclusive:false); // Kuyruğu tüketecek farklı uygulamaların da bu kuyruğa erişmesine izin vermek için exclusive değeri false olarak ayarlandı.

channel.QueueBind(
    queue: queueName,
    exchange: exchangeName,
    routingKey: string.Empty);
// Binding exchange ile kuyrukların kendi aralarında ilişkilendirilme işlemidir.
// Routing key değeri boş olarak belirlendi. Fanout exchange tipinde bu değerin bir önemi yoktur.

EventingBasicConsumer consumer=new EventingBasicConsumer(channel);
channel.BasicConsume(
    consumer: consumer,
    queue:queueName,
    autoAck:true);

consumer.Received += (sender, e) =>
{
    byte[] bytes=e.Body.ToArray();
    
    string message=Encoding.UTF8.GetString(bytes);

    Console.WriteLine("Gelen Mesaj : "+message);
};
// Aynı mesajları bu exchange ile ilişkili tüm consumer'lar işlemiş oldu.


Console.Read();
