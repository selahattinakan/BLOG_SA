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

        [JsonPropertyName("Id")]
        public int? Id { get; set; }
    }
}
