using Constants.Enums;

namespace Constants
{
    public class ResultSet
    {
        public Result Result { get; set; } = Result.Success;
        public string Message { get; set; } = "İşlem Başarılı";
        public int? Id { get; set; }
    }
}
