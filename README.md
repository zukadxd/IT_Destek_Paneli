
# 🛠️ IT Destek Paneli (Helpdesk & Ticket System)

![.NET Core](https://img.shields.io/badge/.NET%20Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white)
![MicrosoftSQLServer](https://img.shields.io/badge/Microsoft%20SQL%20Server-CC2927?style=for-the-badge&logo=microsoft%20sql%20server&logoColor=white)
![Bootstrap](https://img.shields.io/badge/bootstrap-%238511FA.svg?style=for-the-badge&logo=bootstrap&logoColor=white)

Kurum içi bilgi teknolojileri süreçlerini dijitalleştirmek amacıyla geliştirilmiş, güvenlik ve performans odaklı kapsamlı bir **ASP.NET Core MVC** web projesidir.

---

## 🔄 SON GÜNCELLEME (V4 Update : 29.04.2026)
Sistem mimarisi modernize edilerek aşağıdaki profesyonel özellikler eklenmiştir:

* **🛡️ Salted Hashing (Gelişmiş Güvenlik):** Parolalar artık sadece SHA-256 ile değil, her kullanıcıya özel üretilen **dinamik tuzlama (salting)** algoritmasıyla korunmaktadır.
*  💎 (NEW UPDATE) Best Practice Mimarisi (Enum + Lookup Table): Veritabanı esnekliğini sağlayan Lookup Tabloları ile C# tarafında kod okunabilirliğini maksimuma çıkaran Enum yapıları birbirine entegre edilerek, kurumsal kodlama standartlarına (Best Practice) ulaşılmıştır.
* **📸 ImageSharp Entegrasyonu:** Sunucu depolama maliyetlerini düşürmek için görseller otomatik olarak preslenir, **1280px**'e boyutlandırılır ve **.jpg** formatına optimize edilerek kaydedilir.
* **✅ Okundu Bilgisi (Mavi Tik):** Kullanıcı ve Admin arasındaki mesajlaşmalarda gerçek zamanlı "Okundu" takibi sağlayan mantıksal altyapı kurulmuştur.
* **📑 Excel Raporlama:** Tüm talepler `ClosedXML` kullanılarak formatlı Excel dosyası olarak dışa aktarılabilir hale getirilmiştir.
* ✅ SignalR Entegrasyonu: Kullanıcı ve Admin arasındaki iletişim, sayfa yenilemeye gerek kalmadan anlık (real-time) mesajlaşma altyapısına kavuşturulmuştur.

---

## 🚀 Projenin Öne Çıkan Özellikleri
* **Rol ve Yetki Yönetimi:** Admin (IT) ve Standart Kullanıcı seviyelerinde özelleştirilmiş paneller.
* **Gelişmiş Ticket Sistemi:** Taleplere durum, öncelik ve detaylı mesajlaşma akışı atama.
* **Kullanıcı Dostu Arayüz:** Bootstrap 5 ile tamamen responsive (mobil uyumlu) tasarım.
* **Soft Delete:** Verilerin kalıcı silinmesi yerine güvenli bir şekilde gizlenerek arşivlenmesi.
* **Weather Widget:** API üzerinden anlık konum bazlı hava durumu takibi.

---

## 💻 Kullanılan Teknolojiler ve Araçlar
* **Backend:** C#, ASP.NET Core MVC, Entity Framework Core
* **Veritabanı:** Microsoft SQL Server (T-SQL, Lookup Tables)
* **Güvenlik:** `System.Security.Cryptography` (Salted SHA-256)
* **Kütüphaneler:** `SixLabors.ImageSharp`, `ClosedXML`
* **Frontend:** HTML5, CSS3, JavaScript, Bootstrap

---

## ⚙️ Kurulum ve Geliştirme

1. **Projeyi Klonlayın:**
   ```bash
   git clone [https://github.com/yagizaliakman/IT_Destek_Paneli.git](https://github.com/yagizaliakman/IT_Destek_Paneli.git)
2. Veritabanı Kurulumu:

Proje dizinindeki ITHelpdeskDB_Backup.sql dosyasını SSMS içine sürükleyin.

Execute (F5) yaparak veritabanını, tabloları ve yeni güvenlik mimarisine uygun kolonları oluşturun.

3. Bağlantı Ayarı:

appsettings.json dosyasındaki DefaultConnection bilgisini kendi yerel SQL Server adınıza göre güncelleyin.

4. Çalıştırma:

Visual Studio üzerinde projeyi F5 ile derleyip yayına alabilirsiniz.

🔑 Test Hesabı Bilgileri
Önemli: V2 güncellemesi ile şifreleme altyapısı değiştiği için yeni kullanıcı kaydı oluşturmanız veya SQL scriptindeki güncel şifreleri kullanmanız önerilir.

Kullanıcı Adı: admin

Şifre: 1234

Yetki: IT Yöneticisi (Admin)

Bu proje, modern yazılım mimarilerini ve kurumsal güvenlik prensiplerini pekiştirmek amacıyla geliştirilmiştir.
