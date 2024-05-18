using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

// 1. Bağlantı oluşturma

ConnectionFactory connectionFactory = new ConnectionFactory();
connectionFactory.Uri = new Uri("amqps://xtdxdiyy:wJSGJ8qvPSgIFVBDDiJYu5a5w4PiI8qq@rattlesnake.rmq.cloudamqp.com/xtdxdiyy");

// 2. Bağlantıyı aktifleştirme ve kanal açma

using IConnection connection = connectionFactory.CreateConnection();
using IModel channel = connection.CreateModel();

// 3. Queue oluşturma

channel.QueueDeclare(queue: "example-queue", exclusive: false, durable: true);// Consumer'da da kuyruk Publisher'daki ile birebir aynı yapılandırmalıdır.
// Publisher'da durable true ise burada da aynı olmalı QueueDeclare'ler birebir aynı olmalı
// 4. Queue'dan mesaj okuma

// Consumer'ın mesajları alabilmesi için bir event oluşturulmalıdır.
EventingBasicConsumer consumer = new EventingBasicConsumer(channel);

channel.BasicConsume("example-queue", autoAck: false, consumer: consumer); // ack kuyruktan alınan mesajın fiziksel olarak kuyruktan silinip silinmemesi ayarıdır.
// false ise mesaj fiziksel olarak silinir. true ise mesaj fiziksel olarak silinmez. Ack'yi false  yaptık ve mesaj başarıyla işlenirse manuel olarak sileceğiz.
channel.BasicQos(prefetchSize: 0, prefetchCount: 1, false);
consumer.Received += (sender, eventArgs) =>
{
    // kuyruğa gelen mesajın işlendiği yer burası
    byte[] body = eventArgs.Body.ToArray(); // eventArgs.Body.Span'de kullanılabilir.
    string message = Encoding.UTF8.GetString(body); // byte olarak elde ettiğimiz veriyi string'e çevirdik.

    Console.WriteLine("Gelen Mesaj: " + message);

    // Mesajın işlendiğini belirtmek için mesajın silinmesi gerekmektedir.
    channel.BasicAck(eventArgs.DeliveryTag, multiple: false); // mesaj işlendi kuyruktan silebilirsin.DeliveryTag mesaja ait unique değeri temsil eder.
    //multiple : Birden fazla mesaja dair onay bildirisi gönderir.DeliveryTag değerine sahip olan bu mesajla birlikte bundan önceki mesajların da işlendiğini onaylar.False verilirse sadece bu mesajın onay bildiriminde bulunur.
    //Manuel olarak mesajın işlendiğini bildirmeseydik 30 dk sonra mesajlar tekrar işlenmeye başlayacaktı.



};

Console.Read();