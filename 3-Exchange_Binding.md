# Exhange ve Binding

## Exchange

Publisher tarafından gönderilen mesajların nasıl yönetileceğini ve hangi route'lara yönlendirileceğini belirlememiz konusunda kontrol sağlayan/karar veren yapıdır.

Route ise mesajların exchange üzerinden kuyruklara nasıl göndderileceğini tanımlayan mekanizmadır. Bütün exchange türlerinde route kullanılmaz.

## Binding

Exchange ve Queue'lar arasındaki ilişkiye Binding denir.

## Exhange Types

### Direct Exchange
Direct Exchange, mesajlar direkt olarak queue'e gönderilir.
Mesaj routing key'e uygun hedef kuyruklara gönderilir. Genellikle Hata mesajlarının işlendiği durumlarda kullanılabilir.Dosya yükleme hatası,db connection hatası gibi..
Örnek : E Ticaret sisteminde sipariş süreçleri düşünülebilir. Örneğin Onaylandı,iptal edildi ya da iade edildi şeklinde üç farklı durumda üç farklı kuyruk oluşturulabilir ve siparişleri direkt bu kuyruklara göndererek işleyebiliriz.
Default message yapılanmasıdır.

### Fanout Exchange

Mesajların, bu exchange bind olmuş tüm kuyruklara gönderilmesini sağlar.Publisher mesajların gönderildiği kuyruk isimlerini dikkate almaz ve mesajları tüm kuyruklara gönderir.
Tüm servislerde tek bir davranış yapacağımız durumlar için uygundur. 

### Topic Exchange

Routing keyleri kullanarak mesajları kuyruklara yönlendirmek için kullanılır.Bu exchange ile routng key'in bir kısmına/formatına/yapısındaki keylere göre kuyruklara mesajlar gönderilir.

Bu exchange tipi loglama sistemleri uygundur. Kuyruklar log seviyelerine göre bind olabilir ve sadece routing key'ine uygun ilgili log seviyesine ait mesajları alabilirler.Böylece sistem yöneticileri ya da ilgili servisler; belirli bir kategoriye veya key'e göre logları filtreleyebilir ve böylece istedikleri hata yahut uyarı mesajlarına odaklaklanabilir,izleyebilir ve istedikleri log mesajlarını gözardı edebilirler.

### Header Exchange

Routing key yerine header'ları kullanarak mesajları kuyruklara yönlendirmek için kullanılan exchange'dir. Topic exchange ile aynı mantıklıdır. Birisinde routing key diğerinde header key'ler kullanılmaktadır.
Topic exchange kullanılabilecek tüm senaryolarda kullanılabilir.