# SinergyIT-mulakat
İki adet uygulama mevcut. UnitTestApp uygulaması sadece unit testi anlamak, yazmak ve test etmek için hazırlanmıştır. CRUDApp uygulaması ise  katmanlı mimariye dayanarak api yazmak ve apilerin iş sınıfları test etmek için hazırlanmıştır.

# Saga Pattern - Mikroservis Mimarisi

## Giriş
Mikroservis mimarisinde birden fazla veritabanı ile çalışma durumu söz konusudur. Mikroservisler birbirinden bağımsız olarak çalıştığı için, bir işlem birden fazla servisi kapsıyorsa klasik monolitik mimarilerdeki gibi bir "ACID transaction" gerçekleştirmek zordur. Servisler arasında veri tutarlılığı, hata yönetimini ve işlem koordinasyonu sağlamak oldukça zordur. Bu tür problemleri çözmek için **Saga Pattern** kullanılır.

---

## Saga Pattern Nedir?
Saga Pattern, mikroservis mimarisinde servisler arası tutarlılığı, veri akışını, hata yönetimini takip etmek ve işlemleri koordine etmek için kullanılan bir tasarım desenidir.

---

## Saga Pattern Yaklaşımları

### Choreography ve Orchestration
Saga Pattern, iki ana yaklaşım kullanır: **Choreography** ve **Orchestration**.

### Choreography
- **Choreography** yaklaşımında herhangi bir merkezi yönetim yoktur.
- Her servis bir olay (event) yayınlayarak diğer mikroservisleri tetikler.
- **Temel Özellikleri:**
  - Servisler birbirinden doğrudan bağımsızdır.
  - İşlemler genellikle asenkron olarak yürütülür.

### Orchestration
- **Orchestration** yaklaşımında merkezi bir Orchestrator (koordinatör) bulunur.
- Orchestrator, hangi servisin ne zaman çalışacağını belirler ve sırasıyla çağrılar yapar.
- **Temel Özellikleri:**
  - Merkezi bir kontrol mekanizması vardır.
  - İşlemler genellikle senkron bir şekilde yürütülür.

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

## Kullanım Kriterleri

- **Choreography:**  
  Eğer 4 ve daha az servis kullanılacaksa ve sistem daha az dağıtık ise Choreography yaklaşımı tercih edilmelidir.
  
- **Orchestration:**  
  Sistem çok daha fazla dağıtık ve servis sayısı fazla ise Orchestration yaklaşımı kullanılması daha uygundur. Ayrıca, sisteme sonradan eklenecek servislerin entegrasyonu daha kolaydır.

