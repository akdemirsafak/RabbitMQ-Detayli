using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
ConnectionFactory factory = new ConnectionFactory();

factory.Uri = new Uri("amqps://xtdxdiyy:wJSGJ8qvPSgIFVBDDiJYu5a5w4PiI8qq@rattlesnake.rmq.cloudamqp.com/xtdxdiyy");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

#region P2P (Point to Point) Messaging Example

//string queueName="p2p-queue";
//channel.QueueDeclare(queue:queueName,
//    durable:false,
//    exclusive:false,
//    autoDelete:false);

//EventingBasicConsumer consumer=new EventingBasicConsumer(channel);

//channel.BasicConsume(queue:queueName,
//    autoAck:true,
//    consumer:consumer);

//consumer.Received+= (sender, e) =>
//{
//    byte[] body=e.Body.ToArray();
//    string message=Encoding.UTF8.GetString(body);
//    Console.WriteLine($"Message Received: {message}");
//};


#endregion


#region Publish/Subscribe Messaging Example

// string exchangeName = "pubsub-exchange";

// channel.ExchangeDeclare(exchange: exchangeName,
//     type: ExchangeType.Fanout);

// string queueName = channel.QueueDeclare().QueueName; // Rastgele queue ismi. Fanout ile exchange üzerinden çalışacağımız için bir önemi yok.

// channel.QueueBind(
//     exchange: exchangeName,
//     queue: queueName,
//     routingKey: string.Empty);


// //Ölçeklendirme için // Şart değildir. Genelde Pub/Sub yapısında kullanılır.
// channel.BasicQos( // Tüm consumerların aynı anda 1 işlem yapmasını sağlar ve toplam mesaj olarak da sonsuz adet mesaj(prefetchSize) alabilir.
//     prefetchSize: 0,
//     prefetchCount: 1,
//     global: false); // Bir 


// EventingBasicConsumer consumer = new EventingBasicConsumer(channel);

// channel.BasicConsume(
//     queue: queueName,
//     autoAck: true,
//     consumer: consumer);


// consumer.Received += (sender, e) =>
// {
//     byte[] body = e.Body.ToArray();
//     string message = Encoding.UTF8.GetString(body);
//     Console.WriteLine(message);
// };


#endregion


#region  Work Queue Messagin Example

// string queueName = "work-queue";

// channel.QueueDeclare(
//     queue: queueName,
//     durable: false,
//     exclusive: false,
//     autoDelete: false);

// EventingBasicConsumer consumer = new(channel);
// channel.BasicConsume(
//     queue: queueName,
//     autoAck: true,
//     consumer: consumer);

// channel.BasicQos(
//     prefetchCount: 1,
//     prefetchSize: 0,
//     global: false);

// consumer.Received += (sender, e) =>
// {
//     byte[] body = e.Body.ToArray();
//     string message = Encoding.UTF8.GetString(body);
//     Console.WriteLine(message);
// };

#endregion

#region Request Response Messaging Example


string requestQueueName = "request-response-queue";

channel.QueueDeclare(
    queue: requestQueueName,
    durable: false,
    exclusive: false,
    autoDelete: false
    );

EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(
    queue: requestQueueName,
    autoAck: true,
    consumer: consumer
    ); // Gelen mesajı dinledik.

consumer.Received += (sender, e) =>
{
    // Gelen mesaja dair işlemler
    byte[] body = e.Body.ToArray();
    string message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Request Received: {message}");
    // Gelen mesaj işlemleri bitti


    //Reply/Response işlemleri
    string replyQueueName = e.BasicProperties.ReplyTo; // !
    string correlationId = e.BasicProperties.CorrelationId;

    IBasicProperties properties = channel.CreateBasicProperties();
    properties.CorrelationId = correlationId;

    byte[] responseMessage = Encoding.UTF8.GetBytes($"Response to {message}"); // Burada gelen mesaja response dönülecek
    channel.BasicPublish(
        exchange: string.Empty,
        routingKey: replyQueueName, // ! ReplyTo'yu ifade eder.Yukarıda değişkenlere aldık.
        basicProperties: properties,
        body: responseMessage
        );
};
#endregion

Console.Read();
