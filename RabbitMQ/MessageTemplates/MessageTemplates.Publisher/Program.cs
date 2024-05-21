using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory connectionFactory = new ConnectionFactory();

connectionFactory.Uri = new Uri("amqps://xtdxdiyy:wJSGJ8qvPSgIFVBDDiJYu5a5w4PiI8qq@rattlesnake.rmq.cloudamqp.com/xtdxdiyy");

using IConnection connection = connectionFactory.CreateConnection();
using IModel channel = connection.CreateModel();


#region P2P (Point to Point) Messaging Example

//// Declare the queue
//string queueName = "p2p-queue";

//channel.QueueDeclare(queue: queueName,
//    durable:false,
//    exclusive:false,
//    autoDelete:false);

//byte[] message=Encoding.UTF8.GetBytes("Hello, RabbitMQ! P2P Example");
//channel.BasicPublish(
//    exchange: string.Empty,
//    routingKey: queueName,
//    body: message
//    );

#endregion



#region Publish/Subscribe Messaging Example

// string exchangeName = "pubsub-exchange";

// channel.ExchangeDeclare(exchange: exchangeName,
//     type: ExchangeType.Fanout);


// for (int i = 0; i < 50; i++)
// {
//     byte[] message = Encoding.UTF8.GetBytes($"Hello, RabbitMQ! This is {i}. Publish/Subscribe Example Message ");

//     await Task.Delay(1000);
//     channel.BasicPublish(
//         exchange: exchangeName,
//         routingKey: string.Empty,
//         body: message
//         );
// }

#endregion



#region Work Queue Messaging Example

// string queueName = "work-queue";

// channel.QueueDeclare(queue: queueName,
//     durable: false,
//     exclusive: false,
//     autoDelete: false);

// for (int i = 0; i < 50; i++)
// {
//     byte[] message = Encoding.UTF8.GetBytes($"Hello, RabbitMQ! This is {i}. Work Queue Example Message ");

//     await Task.Delay(500);
//     channel.BasicPublish(
//         exchange: string.Empty,
//         routingKey: queueName,
//         body: message
//         );
// }
#endregion



#region Request Response Messaging Example

string requestQueueName = "request-response-queue";
channel.QueueDeclare(
    queue: requestQueueName,
    durable: false,
    exclusive: false,
    autoDelete: false
    );

string replyQueueName = channel.QueueDeclare().QueueName;
string correlationId = Guid.NewGuid().ToString();

#region Request Mesajını Oluşturma ve Gönderme

IBasicProperties properties = channel.CreateBasicProperties();
properties.CorrelationId = correlationId;
properties.ReplyTo = replyQueueName;

for (int i = 0; i < 50; i++)
{
    byte[] message = Encoding.UTF8.GetBytes($"Merhaba {i + 1}"); // Bu mesaj yollanacak
    channel.BasicPublish(
        exchange: string.Empty,
        routingKey: requestQueueName,
        basicProperties: properties,
        body: message
        );
}

#endregion

#region Response Kuyruğunu dinleme 

EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(
    queue: replyQueueName,
    autoAck: true,
    consumer: consumer
    );

consumer.Received += (sender, e) =>
{
    if (e.BasicProperties.CorrelationId == correlationId) // Bu kuyruğa gelen tüm mesajları değil, Gönderilen mesaja dönen response mesajı maiyetindeki mesajları işleyeceğiz.
    {
        byte[] body = e.Body.ToArray();
        string message = Encoding.UTF8.GetString(body);
        Console.WriteLine(message); //Reply message burada ekrana yazılacak.
        Console.WriteLine("**************************");
    }
};

#endregion


#endregion

Console.Read();
