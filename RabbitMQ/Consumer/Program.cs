using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

// 1. Bağlantı oluşturma

ConnectionFactory connectionFactory = new ConnectionFactory();
connectionFactory.Uri = new Uri("");

// 2. Bağlantıyı aktifleştirme ve kanal açma

using IConnection connection = connectionFactory.CreateConnection();
using IModel channel = connection.CreateModel();

// 3. Queue oluşturma

channel.QueueDeclare(queue: "example-queue", exclusive: false);// Consumer'da da kuyruk Publisher'daki ile birebir aynı yapılandırmalıdır.

// 4. Queue'dan mesaj okuma

// Consumer'ın mesajları alabilmesi için bir event oluşturulmalıdır.
EventingBasicConsumer consumer = new EventingBasicConsumer(channel);

channel.BasicConsume("example-queue", false, consumer: consumer); // ack kuyruktan alınan mesajın fiziksel olarak kuyruktan silinip silinmemesi ayarıdır.

consumer.Received += (sender, eventArgs) =>
{
    byte[] body = eventArgs.Body.ToArray(); // eventArgs.Body.Span'de kullanılabilir.
    string message = Encoding.UTF8.GetString(body); // byte olarak elde ettiğimiz veriyi string'e çevirdik.

    Console.WriteLine("Gelen Mesaj: " + message);

    // Mesajın işlendiğini belirtmek için mesajın silinmesi gerekmektedir.
    channel.BasicAck(eventArgs.DeliveryTag, multiple: false);
};

Console.Read();