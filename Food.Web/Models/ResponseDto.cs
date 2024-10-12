namespace Food.Web.Models
{
    public class ResponseDto
    {
        public object Result { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string ErrorMessage { get; set; } = "";
    }
}
