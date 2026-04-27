using System.Security.Cryptography;
using System.Text;

namespace IT_Destek_Panel.Helpers
{
    // Kimlik doğrulama süreçleri için kriptografik güvenlik işlemlerini sağlar.
    public static class SecurityHelper
    {
        // Kullanıcıya özel, kriptografik olarak güvenli 32 byte'lık rastgele bir Salt (tuz) değeri üretir.
        // <returns>Base64 formatında rastgele üretilmiş salt değeri.</returns>
        public static string GenerateSalt()
        {
            byte[] saltBytes = new byte[32];
            using (var provider = RandomNumberGenerator.Create())
            {
                provider.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }
        // Ham parolayı verilen salt değeri ile birleştirerek SHA-256 algoritmasıyla özetler (Hash).
        // <param name="rawPassword">Kullanıcının girdiği ham parola.</param>
        // <param name="salt">Kullanıcıya özel üretilmiş benzersiz salt değeri.</param>
        // <returns>64 karakter uzunluğunda hexadecimal hash değeri.</returns>
        public static string HashPassword(string rawPassword, string salt)
        {
            if (string.IsNullOrEmpty(rawPassword)) return string.Empty;

            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Parola ve Salt birleştirilerek tek yönlü şifreleme uygulanır.
                string combinedPassword = rawPassword + salt;
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(combinedPassword));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}