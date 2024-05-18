
using System.Text;
using RabbitMQ.Client;

//1. Bağlantı oluşturma
ConnectionFactory factory = new();
factory.Uri = new Uri("amqps://xtdxdiyy:wJSGJ8qvPSgIFVBDDiJYu5a5w4PiI8qq@rattlesnake.rmq.cloudamqp.com/xtdxdiyy");

//2. Bağlantıyı Aktifleştirme ve Kanal Açma
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel(); // IDisposible olduğu için using ile kullanma imkanına sahibiz.

//3. Queue oluşturma
channel.QueueDeclare(queue: "example-queue", exclusive: false, durable: true); // Bir kuyruk Exclusive olarak oluşturulduysa başka bir channel'la bu kuyruğa bağlanıp işlem yapılamaz. Ayrıca kuyruk Exclusive ise o bağlantıya özel oluşturulur ve daha sonra silinir.
//durable mesajın kalıcılığı Durability
//4. Queue'ya Mesaj yayınlama
// RabbitMQ kuyruğa atacağı mesajları byte türünden kabul eder. Mesajları byte'a dönüştürmemiz gerekmektedir.

// string text = "Hello RabbitMQ!";
// byte[] message = Encoding.UTF8.GetBytes(text);

// channel.BasicPublish(exchange: "", routingKey: "example-queue", body: message);
//RabbitMQ'de default exchange(direct exchange) davranışı vardır.Exchange belirtmezsek bu şekilde davranır.
//default exchange panelde exchanges kısmında amq.direct'e denk gelir.


IBasicProperties properties = channel.CreateBasicProperties();
properties.Persistent = true; //Durability için
for (int i = 0; i < 50; i++)
{
    byte[] message = Encoding.UTF8.GetBytes($"{i + 1}.Hello RabbitMQ!");
    channel.BasicPublish(exchange: "", routingKey: "example-queue", body: message, basicProperties: properties); // properties Durability için eklendi.
}
//İletileri kalıcı olarak işaretlemek iletinin kaybolmayacağını tam olarak Garanti Etmez.


Console.Read();