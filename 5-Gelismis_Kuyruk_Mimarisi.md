# Gelişmiş Kuyruk Mimarisi

RabbitMQ teknolojisinin an fikri, yoğun kaynak gerektiren işleri/görevleri/operasyonlar hemen yapmaya koyularak tamamlanmasını beklemek zorunda kalmaksızın, bu işleri ölçeklendirilebilir bir vaziyette daya sonra yapılacak bir şekilde planlamaktır.

## Round-Robin Dispatching
Round-Robin Dispatching, RabbitMQ'da kuyrukta bekleyen mesajların consumer'lara sıralamaya göre gönderimine odaklanır. 
Consumer'ı birden fazla ayağa kaldırıp aynı anda çalıştırarak, kuyruktaki mesajları paralel bir şekilde işleyebiliriz.

## Message Acknowledgement (Mesaj onaylama)

RabbitMQ, default olarak consumer'a gönderdiği mesajın başarılı bir şekilde işlensin ya da işlenmesin kuyruktan silinmesi için işaretler. RabbitMQ'nun davranışını değiştireceğiz. Consumer'lara gönderilen mesajların silinmemesini, consumer'dan haber gelene kadar saklanmasını isteyeceğiz.Eğer haber gelmiyorsa bu mesajı tekrar yayınla.

Mesajların güvenliğini sağlar.Cosumer'dan mesaj işleminin başarıyla sonuçlandığına dair cevap alan RabbitMQ mesajı silecektir.

### Message Acknowledgement Probremleri

1. Bir mesaj işlenmeden consumer problem yaşarsa bu mesajın sağlıklı bir şekilde işlenebilmesi için başka bir consumer tarafından tüketilebilir olmalıdır. Bu davranışı uygulamazsak veri kaybı olabilir.
2. Bu özelliği kullanıyorsak mesaj işlendikten sonra RabbitMQ'ya mesajın silinmesi için haber göndermeyi unutmamalıyız. Aksi takdirde mesaj tekrar yayınlanır ve birden fazla kez işlenir.
3. Mesajlar onaylanarak silinmediği durumda kuyrukta kalmasından dolayı kuyruğun büyümesine neden olarak yavaşlamayla sonuçlanacak, performans düşecektir.

#### Özet

Bir mesajın kaybolmadığından emin olabilir hale geldik. Consumer açısından mesajın alınıp işlendiğini ve kuyruktan silinmesi gerektiğini ifade ederek süreci daha güvenilir hale getirdik.RabbitMQ açısından consumer'dan gelecek olan onay bildirimi için bir zaman aşımı süresi söz konusudur.Bu zaman aşımı süresi default olarak 30dk'dır.


<strong>Message Acknowledgement özelliği Consumer'da tanımlanır.</strong>

#### BasicNack ile İşlenmeyen Mesajları Geri Gönderme

Bazı durumlarda Consumer'larda istemsiz durumların dışında kendi kontrollerimiz neticesinde mesajları işlememek isteyebilir veya ilgili mesajların işlenmesini başarıyla sonuçlandıramayacağımızı anlayabiliriz.

channel.BasicNack() methodunu kullanarak RabbitMQ'ya bilgi verebilir ve mesajı tekrardan işletebiliriz.
<p style="color:green">Example : channel.BasicNack(deliveryTag:eventArgs.DeliveryTag,multiple:false,require:true);</p>
<p style='color:red'>require parametresi : Bu parametre ,bu consumer tarafından işlenemeyeceğini ifade edilen bu mesajın tekrar kuyruğa eklenip eklenmemesinin kararını vermektedir.True olursa tekrar işlenmek üzere kuyruğa eklenir. False olarak ayarlanırsa silinir.</p>

#### BasicCancel ile Kuyruktaki Tüm mesajların işlenmesini reddetme

Kuyruktaki gelecek bütün mesajları reddedebiliriz.

#### Basic Reject ile Tek bir Mesajın İşlenmesini Reddetme

## Message Durability / Mesaj Dayanıklığı - Sürekliliği

RabbitMQ sunucusunda meydana gelebilecek durumlarda tüm kuyruklar ve mesajlar silinecektir.Böyle bir durumu ele alacağız.

Outbox ve Inbox gibi Pattern'ler ile verisel kaybı garanti altına almaya çalışırız fakat RabbitMQ üzerinde de kalıcılık sağlamaya çalışmalıyız.

Bu konfigürasyonun Publisher'da yapılması gerekir.
 olarak ayarlamamız gerekir. Burada kuyruk için konfigurasyon yaparız.
```
channel.QueueDeclare(durable:true);
IBasicProperties properties=channel.CreateBasicProperties();
properties.Persistent=true;
```
Yukarıdaki QueueDeclare kısmı kuyruk için,
properties kısımları da Mesaj için konfigüre edilmiştir.

<p style="color:red">İletileri kalıcı olarak işaretlemek iletinin kaybolmayacağını tam olarak Garanti Etmez.</p>


### Fair Dispatch / Adil Dağıtım

Consumerlara eşit şekilde mesaj iletilmesi sistemdeki performansı düzenli bir hale getirir. Diğer consumer'ların daha fazla yük alması ve sistemdeki diğer consumer'ların kısmi aç kalması engellenmiş olur.
RabbitMQ'da BasicQos methodu ile mesajların işleme hızını ve teslimat sırasını belirleyebiliriz. Böylece Fair Dispatch özelliği konfigüre edilebilir.
```
channel.BasicQos(prefetchSize:0,prefetchCount:1,global:false)
```
Yukarıdaki kod bloğu incelendiğinde ;
<u>prefetchSize</u> : Bir consumer tarafından alınabilecek en büyük mesaj boyutunu byte cinsinden belirler. 0 Sınırsız demektir.
<u>prefetchCount</u> : Bir consumer tarafından aynı anda alınabilecek en fazla mesaj sayısını belirler.
<u>global</u> : Bu parametre true ise tüm consumer'lar için geçerli olur. False ise sadece bu consumer için geçerli olur.

<strong>Ölçeklendirilebilirlik açısından önemli bir konfigürasyondur.</strong>
