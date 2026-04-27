using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace IT_Destek_Panel.ViewComponents
{
    /// Open-Meteo API kullanarak dinamik hava durumu verilerini sağlayan bileşen.
    public class WeatherWidgetViewComponent : ViewComponent
    {
        
        // Belirtilen koordinatlara göre güncel sıcaklık verisini asenkron olarak getirir.
        // <param name="city">Görüntülenecek şehir adı.</param>
        //<param name="lat">Enlem (Latitude) bilgisi.</param>
        // <param name="lon">Boylam (Longitude) bilgisi.</param>
        public async Task<IViewComponentResult> InvokeAsync(string city = "İzmir", string lat = "38.4127", string lon = "27.1384")
        {
            string temp = "--";
            try
            {
                using (var client = new HttpClient())
                {
                    string url = $"https://api.open-meteo.com/v1/forecast?latitude={lat}&longitude={lon}&current_weather=true";
                    var response = await client.GetStringAsync(url);

                    using (JsonDocument doc = JsonDocument.Parse(response))
                    {
                        var current = doc.RootElement.GetProperty("current_weather");
                        temp = current.GetProperty("temperature").GetDouble().ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                // API bağlantı hataları için fallback değeri.
                temp = "N/A";
            }

            ViewBag.City = city;
            return View("Default", temp);
        }
    }
}