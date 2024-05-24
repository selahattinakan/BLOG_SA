using Constants.Enums;
using System.Text.Json.Serialization;

namespace Constants
{
    public class ResultSet
    {
        [JsonPropertyName("Result")]
        public Result Result { get; set; } = Result.Success;

        [JsonPropertyName("Message")]
        public string Message { get; set; } = "İşlem Başarılı";

        /// <summary>
        /// İşlem yapılan nesnenin Id bilgisini tutar.
        /// </summary>
        [JsonPropertyName("Id")]
        public int? Id { get; set; }

        /// <summary>
        /// İşlem yapılan nesneyi tutar.
        /// </summary>
        [JsonPropertyName("Object")]
        public object Object { get; set; }

        /// <summary>
        /// İşlem sonrası kullanılabilecek değişkenleri tutar.
        /// </summary>
        [JsonPropertyName("TempData")]
        public object TempData { get; set; }
    }
}
