namespace IceCreamBE.DTO
{
    public class Response<T>
    {
        public Response()
        {
        }
        public Response(T data, bool succeeded, string message, string[] error)
        {
            Succeeded = succeeded;
            Message = message;
            Errors = error;
            Data = data;
        }
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public string[] Errors { get; set; }
        public T Data { get; set; }
        public TokenOutDTO? Token { get; set; }
    }
}
