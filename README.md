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
## 4.Orchestration Saga pattern avantajları ve dezavantajları nelerdir?
Bir e-ticaret uygulaması tasarladığınızı düşünelim. Bu uygulamada müşteriler sipariş verdiklerinde, birden fazla hizmetin birlikte çalışması gerekiyor. Müşteri bir sipariş verdiğinde şu adımlar gerçekleşmeli:
1.Stokta mevcut ürünleri kontrol eder ve onları rezerve eder.
2.Müşterinin yeterli bakiye olup olmadığı kontrol edilir ve ödeme işlemi gerçekleştirilir. 
3.Kargo ödeme onaylandıktan sonra gönderi için hazırlık yapar ve teslimat planlanır.
Burada dikkat etmeniz gereken bir nokta var: Eğer bu adımlardan herhangi biri başarısız olursa (örneğin, ödeme başarısız olursa veya stokta ürün yoksa), sistem önceki adımları geri alarak verilerin tutarlılığını sağlamalıdır. Yani, ödeme başarısız olursa stoktaki rezerv kaldırılmalı, kargo işlemi başarısız olursa ödeme iade edilmelidir..
Soru:
 - Bu süreci yönetmek için bir Saga pattern tasarlayın ve basit bir durum makinesi (state machine) diyagramı çizin. Sipariş Verildi aşamasından Sipariş Tamamlandı aşamasına kadar olan her bir durumu çizin ve her bir başarısızlık durumunda geri alma adımlarını gösterin.
 - Her bir durumda, ilgili hizmetin başarılı ya da başarısız olması durumunda nasıl bir geçiş yapılacağını açıklayın.



