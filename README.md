# SinergyIT-mulakat
İki adet uygulama mevcut. UnitTestApp uygulaması sadece unit testi anlamak, yazmak ve test etmek için hazırlanmıştır. CRUDApp uygulaması ise  katmanlı mimariye dayanarak api yazmak ve apilerin iş sınıfları test etmek için hazırlanmıştır.

# Saga Pattern - Mikroservis Mimarisi

## 1. Saga pattern mikroservis mimarisinde hangi sorunları çözmeye çalışır?

Mikroservis mimarisinde birden fazla veritabanı ile çalışma durumu söz konusudur. Mikroservisler birbirinden bağımsız olarak çalıştığı için, bir işlem birden fazla servisi kapsıyorsa klasik monolitik mimarilerdeki gibi bir "ACID transaction" gerçekleştirmek zordur. Servisler arasında veri tutarlılığı, hata yönetimini ve işlem koordinasyonu sağlamak oldukça zordur. Servisler arası tutarlılığı, veri akışını, hata yönetimini takip etmek ve işlemleri koordine etmek için Saga Pattern mimarisi kullanılır.


## 2.Saga patterndeki choreography ve orchestration yaklaşımları arasındaki temel fark nedir?  

 Choreography yaklaşımında herhangi bir merkezi yönetimi yokken Orchestration  yaklaşımında bu durum tam tersidir. Choreography’de her servis event yayınlarak diğer mikroservisleri tetiklerken Orchestration yaklaşımında bir  Orchestrator (koordinatör) tüm işlemleri kontrol eder.Orchestrator, hangi servisin ne zaman çalışacağını belirler ve sırasıyla çağrılar yapar. Orchestration yaklaşımında merkezi yönetim olduğu için daha dağıtık sistemlerde kullanılması uygundur. Choreography’de servisler doğrudan birbiriyle iletişim kurmaz, olaylar asenkron olarak işlenir.Orchestration yaklaşımda ise işlemler genellikle senkron şekilde yürütülür. Distributed transaction (birden fazla farklı veritabanının bir bütün olarak çalıştığı durum)’a katılacak olan microservice sayısı kriterlerden birisidir. Eğer 4 ve daha az servis kullanılacaksa Choreography yaklaşımı kullanmak daha uygundur. Sistem çok daha fazla dağıtık, servis sayısı fazla ise Orchestration  yaklaşımını kullanmak, takibini yapmak daha kolay olur. Sisteme sonradan eklenecek servislerin de entegresi kolay olur. 

---

## Choreography ve Orchestration Arasındaki Temel Farklar

| Özellik                  | Choreography                                   | Orchestration                                |
|--------------------------|-----------------------------------------------|---------------------------------------------|
| **Merkezi Kontrol**      | Yok                                           | Var                                         |
| **İletişim Şekli**       | Mikroservisler arasında asenkron olay tabanlı | Merkezi koordinasyon                        |
| **İzlenebilirlik**       | Zor                                           | Kolay                                       |
| **Hata Yönetimi**        | Karmaşık                                     | Merkezi ve daha kolay                       |
| **Uygunluk**             | Basit sistemler                              | Daha karmaşık ve dağıtık sistemler          |
| **Esneklik**             | Servis eklemek/dönüştürmek zordur            | Yeni servisler kolayca entegre edilebilir   |

---

## 3.Orchestration Saga pattern avantajları ve dezavantajları nelerdir?

### Avantajları
- Karmaşık sistemleri adım adım takip etmek ve yönetmek kolaydır.
- Tek taraflı olarak Saga katılımcılarına(servisler) bağlı olduğundan dolayı döngüsel bağımlılıklar söz konusu değildir.
- Her bir servisin, diğer servisler hakkında bilgiye sahip olmasına gerek duyulmuyor.
- Hatalar, Orchestrator tarafından yönetildiği için kolayca tespit edilebilir ve müdahale edilebilir.
- İş akışı sırası değiştirildiğinde sadece Orchestrator üzerinde güncelleme yapılması yeterlidir. Diğer mikroservislerde değişiklik gerekmez.

### Dezavantajları
- Merkezi bir kontrol mekanizması olduğu için Orchestrator’da bir hata meydana gelirse sistemin tüm akışı durur.
- Tüm iş akışı Orchestrator’a yüklendiği için zamanla karmaşık hale gelebilir.
- Mikroservisler Orchestrator’a bağımlı hale gelir. Bu durum, mikroservislerin bağımsız çalışma ilkesine ters düşer.

---
## 5.Uygulama
Bir e-ticaret uygulaması tasarladığınızı düşünelim. Bu uygulamada müşteriler sipariş verdiklerinde, birden fazla hizmetin birlikte çalışması gerekiyor. Müşteri bir sipariş verdiğinde şu adımlar gerçekleşmeli:
1.Stokta mevcut ürünleri kontrol eder ve onları rezerve eder.
2.Müşterinin yeterli bakiye olup olmadığı kontrol edilir ve ödeme işlemi gerçekleştirilir. 
3.Kargo ödeme onaylandıktan sonra gönderi için hazırlık yapar ve teslimat planlanır.
Burada dikkat etmeniz gereken bir nokta var: Eğer bu adımlardan herhangi biri başarısız olursa (örneğin, ödeme başarısız olursa veya stokta ürün yoksa), sistem önceki adımları geri alarak verilerin tutarlılığını sağlamalıdır. Yani, ödeme başarısız olursa stoktaki rezerv kaldırılmalı, kargo işlemi başarısız olursa ödeme iade edilmelidir..
Soru:
 - Bu süreci yönetmek için bir Saga pattern tasarlayın ve basit bir durum makinesi (state machine) diyagramı çizin. Sipariş Verildi aşamasından Sipariş Tamamlandı aşamasına kadar olan her bir durumu çizin ve her bir başarısızlık durumunda geri alma adımlarını gösterin.
 - Her bir durumda, ilgili hizmetin başarılı ya da başarısız olması durumunda nasıl bir geçiş yapılacağını açıklayın.

 **Aşağıda Saga pattern'ine uygun bir durum makinesi (state machine) diyagramı oluşturulmuştur. Adım adım açıklama şu şekildedir:**

 - Order Placed (Sipariş Verildi): Süreç "Sipariş Verildi" adımıyla başlar.
 - Check Stock (Stok Kontrolü): Eğer stok mevcutsa "Reserve Stock" (Stok Rezerve Et) adımına geçilir.
 - Stok mevcut değilse "Cancel Stock Reservation" (Stok Rezervasyonunu İptal Et) ile süreç sonlanır.
 - Reserve Stock (Stok Rezervasyonu): Rezervasyon başarılıysa "Payment Processing" (Ödeme İşlemi) adımına geçilir.Başarısız olursa, "Cancel Stock Reservation" (Stok Rezervasyonunu İptal Et) adımı devreye girer.
 - Payment Processing (Ödeme İşleme): Ödeme başarılıysa "Shipment Preparation" (Kargo Hazırlığı) adımına geçilir.Başarısız olursa, "Refund Payment" (Ödeme İadesi) yapılır.
 - Shipment Preparation (Kargo Hazırlığı): Kargo hazırlığı başarılıysa "Delivery Scheduled" (Teslimat Planlandı) adımına geçilir. Başarısız olursa, "Cancel Shipment" (Kargo İptali) yapılır.
 - Delivery Scheduled (Teslimat Planlandı): Teslimat başarılı olursa "Order Completed" (Sipariş Tamamlandı) adımına geçilir.

Hata Durumunda Geri Alma:

 - Stok rezervasyonunda Cancel Stock Reservation adımı devreye girer. Stok rezervasyonu yapılmaz  ve sipariş iptal edilir. 
 - Ödemede bir hata varsa Refund Payment devreye girer. Stok rezervasyonunu geri alır ve ödemeyi geri iade eder  ve sipariş iptal edilir.
 - Kargo hazırlığı başarısız olursa "Cancel Shipment" adımı uygulanır.Bu adımda ise rezervasyon geri alınır ve ödeme iade edilir ve sipariş iptal edilir.
   
![image](https://github.com/user-attachments/assets/871dbdcc-1ae8-4df1-98e2-d5ca1ce61208)![image](https://github.com/user-attachments/assets/d5f75e3c-0411-4d1e-a93f-d9002580075c)


---
# Unit Test
   Unit Test, Birim Test demektir. Yani uygulamadaki en küçük işlem yapan birimlerin/metotların test edilmesidir. Burada amaç, bir uygulamadaki metotların önceden belirlenmiş kurallar çerçevesinde davranışlarının test edilmesidir. Böylece, bireysel veya ekipsel çalışmalarda inşa edilen kodların belirlenmiş standartlar çerçevesinde olması sağlanmış olacak ve yapılacak değerlendirmelerden geçemeyen birimler standartlara uymadıklarından dolayı hızlıca kontrol edilip onarılabilecektirler. 
   Unit test yazmak Arrange, Act ve Assert olmak üzere üç aşamadan oluşur:
**Arrange**: Test edilecek metodun kullanacağı kaynakların hazırlandığı bölümdür. Değişken tanımlama, nesne oluşturma vs. gerçekleştirilir.
**Act** :Arrange aşamasında hazırlanan değişkenler, nesneler eşliğinde test edilecek olan metodun çalıştırıldığı bölümdür.
**Assert** : Act aşamasında yapılan testin doğrulama evresidir. Tek bir Act’te birden fazla sonuç gerçekleşebilir. 

### Popüler Unit Test framework’leri:

**1. xUnit**: Testleri yazmak ve çalıştırmak için kullanılır.
 - ***Assert sınıfı;***
   Birim testlerinde beklenen ve gerçek sonuçları karşılaştırmak için kullanılan temel bir test sınıfıdır.
    - Assert.True(condition), koşulun true olduğunu doğrular.
    - Assert.False(condition), koşulun false olduğunu doğrular.
    - Assert.Equal(expected, actual) ,beklenen ve gerçek değerlerin eşit olduğunu doğrular.
    - Assert.NotEqual(expected, actual), beklenen ve gerçek değerlerin farklı olduğunu doğrular.
    - Assert.Null(object), nesnenin null olduğunu doğrular.
    - Assert.NotNull(object), nesnenin null olmadığını doğrular.
    - Assert.Same(expected, actual), iki nesnenin aynı referansa sahip olduğunu doğrular.
    - Assert.NotSame(expected, actual),iki nesnenin farklı referansa sahip olduğunu doğrular.
    - Assert.Throws<TException>(Action), belirtilen türde bir istisnanın atılmasını bekler.
    - Assert.ThrowsAny<TException>(Action), belirtilen veya türetilmiş herhangi bir istisnanın atılmasını bekler.
    - Assert.Empty(collection), koleksiyonun boş olduğunu doğrular.
    - Assert.NotEmpty(collection), koleksiyonun boş olmadığını doğrular.
    - Assert.Contains(expected, collection), koleksiyonun belirtilen öğeyi içerdiğini doğrular.
    - Assert.DoesNotContain(expected, collection), koleksiyonun belirtilen öğeyi içermediğini doğrular.
    - Assert.All(collection, action) , koleksiyondaki her bir öğe için belirli bir durumu doğrular
 - ***Fact;***
  Sabit bir test senaryosu için kullanılır.
  Parametre almaz ve genellikle statik testler için tercih edilir.
  - ***Theory;***
  Farklı parametre kombinasyonlarıyla test yapmak için kullanılır.
  Parametreleri belirlemek için InlineData, MemberData veya ClassData kullanılır.

**2. Moq**:  nesneler oluşturarak bağımlılıkları izole etmek ve sadece test edilen kodu doğrulamak için kullanılır.Moq, arayüzlerin veya sınıfların sahte nesnelerini kolayca oluşturmanızı sağlar. Test edilmek istenilen sınıfların gerçek nesnelerini kullanmak yerine onları simüle etmemizi sağlayan ve böylece test süreçlerindeki maliyetleri minimize etmemizi hedefleyen bir framework’tür.Bir sınıfı mocklayabilmek için o sınıfın implemente ettiği interface kullanılmalıdır. 


