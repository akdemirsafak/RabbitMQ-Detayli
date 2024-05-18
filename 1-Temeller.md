# RabbitMQ

### Message Queue Nedir?
Yazılım sistemlerinde iletişim için kullanılan bir yapıdır.Birbirinden bağmsız sistemler arasında veri alışverişi yapmak için kullanılır.
Gönderilen mesajları kuyrukta(queue) saklar ve sonradan bu mesajların işlenmesini sağlar.
Kuyruk mesaj gönderene <u>Producer(Yayıncı)</u> ya da <u>Publisher</u>, kuyruktaki mesajları işleyene ise <u>Consumer(Tüketici)</u> denir. 

Message dediğimiz kavram; iki sistem arasındaki iletişim için kullanılan veri birimidir.Yani Producer'ın, Consumer tarafından işlenmesini istediği verinin ta kendisidir.
Message Queue, mimaride Async davranış sergilenmesini sağlar.
First in First out mantığıyla çalışır. Klasik queue mantığıyla.


#### Senkron ve Asenkron İletişim Modelleri

Senkron iletişimde, bir iletişimin iki tarafı arasında bir iletişimin tamamlanması için beklenir.
Asenkron iletişimde ise, bir iletişimin iki tarafı arasında bir iletişimin tamamlanması için beklenmez.

Mail göndermek,fatura oluşturmak, stok güncellemek gibi zaman gerektiren işlemlerin asenkron iletişim modeliyle işlenmesi daha idealdir.


### Message Broker Nedir ?

Message Queu'yu kullanan teknolojilerin/genel sistemin adıdır.Bir Message Broker'da birden fazla queue olabilir.
Message Broker, Message Queue'ları yönetir ve Message Queue'ları birbirine bağlar.

#### Message Broker Teknolojileri

1. RabbitMQ
2. Kafka
3. ActiveMQ
4. NSQ
5. IronMQ
6. Redis

### RabbitMQ nedir 
Open source message queuing sistemidir.Cloud'da hizmeti mevcuttur. Cross platform desteklidir,farklı işletim sistemlerinde kullanılabilir.

#### Neden kullanmalıyız
Ölçeklendirilebilir bir ortam istiyorsak kullanılabilir.
Response time'ı uzun sürebilecek operasyonları uygulamadan bağımsızlaştırarak bu işlemler için farklı bir uygulamanın üstlenmesini sağlayacak olan bir mekanizma sunar. 

### RabbitMQ'nun İşleyişi

Publisher mesajı Exchange'e yollar(Publish eder).Exchange mesajı karşılar.Exchange belirtilen route ile mesajı queue'ya yönlendirir.
Exchange : Mesajların nasıl işleneceğinin modelini sunar.

Message Broker dil bağımsızdır Publisher ve Consumer servisler farklı dillerde yazılmış olabilir.
Tüm süreçte RabbitMQ AMQP(Advanced Message Queuing Protocol) kullanarak faaliyetini gerçekleştirmektedir.

