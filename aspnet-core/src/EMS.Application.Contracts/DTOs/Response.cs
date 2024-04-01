namespace EMS.DTOs
{
    public class LoginResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public string token { get; set; }
    }
}