# SinergyIT-mulakat
İki adet uygulama mevcut. UnitTestApp uygulaması sadece unit testi anlamak, yazmak ve test etmek için hazırlanmıştır. CRUDApp uygulaması ise  katmanlı mimariye dayanarak api yazmak ve apilerin iş sınıfları test etmek için hazırlanmıştır.

# Saga Pattern - Mikroservis Mimarisi

## 1. Saga pattern mikroservis mimarisinde hangi sorunları çözmeye çalışır?
Mikroservis mimarisinde birden fazla veritabanı ile çalışma durumu söz konusudur. Mikroservisler birbirinden bağımsız olarak çalıştığı için, bir işlem birden fazla servisi kapsıyorsa klasik monolitik mimarilerdeki gibi bir "ACID transaction" gerçekleştirmek zordur. Servisler arasında veri tutarlılığı, hata yönetimini ve işlem koordinasyonu sağlamak oldukça zordur. Servisler arası tutarlılığı, veri akışını, hata yönetimini takip etmek ve işlemleri koordine etmek için**Saga Pattern** kullanılır.
---

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

## Orchestration Saga Pattern Avantaj ve Dezavantajları

### Avantajları
- Karmaşık sistemleri adım adım takip etmek ve yönetmek kolaydır.
- Döngüsel bağımlılıklar yoktur. Her bir servisin diğer servisler hakkında bilgiye sahip olmasına gerek yoktur.
- Hatalar, Orchestrator tarafından yönetildiği için kolayca tespit edilebilir ve müdahale edilebilir.
- İş akışı sırası değiştirildiğinde sadece Orchestrator üzerinde güncelleme yapılması yeterlidir. Diğer mikroservislerde değişiklik gerekmez.

### Dezavantajları
- Merkezi bir kontrol mekanizması olduğu için Orchestrator’da bir hata meydana gelirse sistemin tüm akışı durur.
- Tüm iş akışı Orchestrator’a yüklendiği için zamanla karmaşık hale gelebilir.
- Mikroservisler Orchestrator’a bağımlı hale gelir. Bu durum, mikroservislerin bağımsız çalışma ilkesine ters düşer.

---



