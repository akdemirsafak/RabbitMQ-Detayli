using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

ConnectionFactory connectionFactory = new ConnectionFactory();
connectionFactory.Uri = new Uri("amqps://xtdxdiyy:wJSGJ8qvPSgIFVBDDiJYu5a5w4PiI8qq@rattlesnake.rmq.cloudamqp.com/xtdxdiyy");

using IConnection connection = connectionFactory.CreateConnection();
using IModel channel = connection.CreateModel();


/*
1. Adım : Publisher'daki exchange ile birebir aynı isim ve type'a sahip exchange tanımlanır.
2. Adım : Publisher tarafından routing key'de bulanan değerdeki kuyruğa gönderilen mesajları kendi oluşturduğumuz kuyruğa yönlendirerek tüketmemiz(consume etmemiz) gerekmektedir.
    Bunun için bir kuyruk oluşturmalıyız.

*/

// 1. Adım
channel.ExchangeDeclare(exchange: "direct-exchange-example", type: ExchangeType.Direct);

//2. Adım : 
string queueName = channel.QueueDeclare().QueueName; //konfigurasyonsuz kullanılırsa rabbitmq'da random isimli bir queue oluşturulur. Random oluşturulan queue name' i aldık ve gelen mesajları bu queue ya yönlendireceğiz.
//channel.QueueDeclare(queue:"direct-exchange-example"); Tercihen isim belirleyip kullanılabilir

// 3. Adım
channel.QueueBind(queue: queueName,
exchange: "direct-exchange-example",
routingKey: "direct-queue-example"); // bu routingKey değerine karşılık gelen mesajları queue olarak belirtilen parametreye denk gelen queue'ya yönlendiriyoruz/Bind ediyoruz.

EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

consumer.Received += (object sender, BasicDeliverEventArgs e) =>
{
    //byte[] body = e.Body.ToArray();
    string message = Encoding.UTF8.GetString(e.Body.Span);
    System.Console.WriteLine("Gelen Mesaj : " + message);
};

Console.Read();
