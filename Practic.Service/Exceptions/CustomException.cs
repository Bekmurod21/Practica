namespace Practic.Service.Exceptions
{
    public class CustomException : Exception
    {
        public int Code { get; set; }
        public CustomException(int code = 500, string message = "Something went wrong") : base(message)
        {
            this.Code = code;
        }
    }
}
