# 🛠️ IT Destek Paneli (Helpdesk & Ticket System)

![.NET Core](https://img.shields.io/badge/.NET%20Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white)
![MicrosoftSQLServer](https://img.shields.io/badge/Microsoft%20SQL%20Server-CC2927?style=for-the-badge&logo=microsoft%20sql%20server&logoColor=white)
![Bootstrap](https://img.shields.io/badge/bootstrap-%238511FA.svg?style=for-the-badge&logo=bootstrap&logoColor=white)

Kurum içi bilgi teknolojileri süreçlerini dijitalleştirmek, kullanıcıların teknik donanım/yazılım sorunlarını hızlıca bildirmesini sağlamak ve IT personelinin bu talepleri tek bir merkezden yönetmesi amacıyla geliştirilmiş kapsamlı bir **ASP.NET Core MVC** web projesidir.

---

## 🚀 Projenin Öne Çıkan Özellikleri

* **Rol ve Yetki Yönetimi:** Sistem, Admin (IT Personeli) ve Standart Kullanıcı olmak üzere farklı yetki seviyeleriyle çalışır. Her rol kendi paneline ve yetkilerine sahiptir.
* **Güvenli Mimari:** Kullanıcı şifreleri veritabanında düz metin olarak değil, yüksek güvenlikli şifreleme algoritmaları (SHA-256) kullanılarak tutulur.
* **Gelişmiş Ticket (Talep) Sistemi:**
  * Kullanıcılar karşılaştıkları sorunlar için detaylı destek talebi (ticket) oluşturabilir.
  * Taleplere ait durum güncellemeleri (Açık, İşlemde, Çözüldü) ve öncelik seviyeleri (Düşük, Orta, Acil) atanabilir.
  * Talep detayında kullanıcı ve IT personeli arasında anlık mesajlaşma/yanıt akışı sağlanır.
* **Kullanıcı Dostu Modern Arayüz:** Tüm cihazlarla uyumlu (Responsive), Bootstrap altyapısıyla hazırlanmış, temiz ve anlaşılır tasarım.
* **Ekstra Entegrasyonlar:** * Kullanıcı tercihine bağlı **Dark Mode (Karanlık Tema)** desteği.
  * API üzerinden anlık veri çeken **Hava Durumu (Weather Widget)** modülü.

---

## 💻 Kullanılan Teknolojiler ve Araçlar

**Backend:**
* C# 
* ASP.NET Core MVC
* Entity Framework / ADO.NET Altyapısı

**Veritabanı:**
* Microsoft SQL Server (T-SQL)

**Frontend:**
* HTML5, CSS3, JavaScript
* Bootstrap Framework

---

## ⚙️ Kurulum ve Geliştirme Ortamında Çalıştırma

Projeyi kendi yerel ortamınızda (Localhost) çalıştırmak için aşağıdaki adımları sırasıyla uygulayın:

### 1. Projeyi Klonlayın
bash
git clone [https://github.com/yagizaliakman/IT_Destek_Paneli.git](https://github.com/yagizaliakman/IT_Destek_Paneli.git)

2. Veritabanı Kurulumu (Çok Önemli)
Proje dizininde yer alan script.sql dosyası, veritabanının iskeletini ve başlangıç verilerini barındırır.

SQL Server Management Studio (SSMS) programını açın.

script.sql dosyasını SSMS içine sürükleyin.

Kodu çalıştırarak (Execute - F5) ITHelpdeskDB veritabanını, tabloları ve şifrelenmiş varsayılan kullanıcı hesaplarını tek tıkla oluşturun.

3. Bağlantı Dizesi (Connection String) Ayarı
Visual Studio üzerinde projeyi açın.

appsettings.json dosyasını bulun.

DefaultConnection değerini kendi bilgisayarınızdaki SQL Server adınıza (Örn: Server=localhost;Database=ITHelpdeskDB;Trusted_Connection=True;) göre güncelleyin.

4. Çalıştırma
Visual Studio'da F5 tuşuna basarak projeyi derleyip çalıştırabilirsiniz.

🔑 Varsayılan Test Hesapları
Veritabanı script'i çalıştırıldığında test yapabilmeniz için sisteme otomatik olarak eklenen yetkili hesap:

Kullanıcı Adı: kraladmin

Şifre: 1234

Yetki: IT Yöneticisi (Admin)

Bu proje, yazılım geliştirme süreçlerini ve MVC mimarisini pekiştirmek amacıyla geliştirilmiştir.
