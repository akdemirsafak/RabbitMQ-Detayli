
using System.Text;
using RabbitMQ.Client;

//1. Bağlantı oluşturma
ConnectionFactory factory = new();
factory.Uri = new Uri("");

//2. Bağlantıyı Aktifleştirme ve Kanal Açma
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel(); // IDisposible olduğu için using ile kullanma imkanına sahibiz.

//3. Queue oluşturma
channel.QueueDeclare(queue: "example-queue", exclusive: false); // Bir kuyruk Exclusive olarak oluşturulduysa başka bir channel'la bu kuyruğa bağlanıp işlem yapılamaz. Ayrıca kuyruk Exclusive ise o bağlantıya özel oluşturulur ve daha sonra silinir.

//4. Queue'ya Mesaj yayınlama
// RabbitMQ kuyruğa atacağı mesajları byte türünden kabul eder. Mesajları byte'a dönüştürmemiz gerekmektedir.

// string text = "Hello RabbitMQ!";
// byte[] message = Encoding.UTF8.GetBytes(text);

// channel.BasicPublish(exchange: "", routingKey: "example-queue", body: message);
//RabbitMQ'de default exchange(direct exchange) davranışı vardır.Exchange belirtmezsek bu şekilde davranır.
//default exchange panelde exchanges kısmında amq.direct'e denk gelir.



for (int i = 0; i < 10; i++)
{
    byte[] message = Encoding.UTF8.GetBytes("Hello RabbitMQ!");
    channel.BasicPublish(exchange: "", routingKey: "example-queue", body: message);
}


Console.Read();