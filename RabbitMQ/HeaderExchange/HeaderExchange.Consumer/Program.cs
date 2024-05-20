using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory= new ConnectionFactory();
factory.Uri = new Uri("amqps://xtdxdiyy:wJSGJ8qvPSgIFVBDDiJYu5a5w4PiI8qq@rattlesnake.rmq.cloudamqp.com/xtdxdiyy");
using IConnection conn = factory.CreateConnection();
using IModel channel = conn.CreateModel();

string exchangeName="header-exchange";

channel.ExchangeDeclare(
    exchange:exchangeName,
    type:ExchangeType.Headers);


Console.Write("Lütfen header value'sunu giriniz : ");
string headerValue = Console.ReadLine();

string queueName=channel.QueueDeclare().QueueName; // kuyruk oluşturuldu oluşturulan random queue name'ini aldık.

channel.QueueBind(
    queue:queueName,
    exchange:exchangeName,
    routingKey:string.Empty,
    arguments:new Dictionary<string,object>{
        ["x-match"]="any", //any veya all olabilir. any bir tanesi uyuşsa yeterli, all hepsinin uyuşması gerekir. default olarak any'dir
        ["no"]=headerValue    
    }); //arguments kısmından header'ı tanımladık.


EventingBasicConsumer consumer=new EventingBasicConsumer(channel);

channel.BasicConsume(
    queue:queueName,
    autoAck:true,
    consumer:consumer);

consumer.Received+=(model,ea)=>{
    byte[] body=ea.Body.ToArray();
    string message=Encoding.UTF8.GetString(body);
    Console.WriteLine($"Gelen Mesaj : {message}");
};


Console.Read();
