namespace BetaCinema.PayLoads.Responses
{
    public class ResponseObject<T>
    {
        public int status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public ResponseObject() { }

        public ResponseObject(int status, string message, T data)
        {
            this.status = status;
            Message = message;
            Data = data;
        }
        public ResponseObject<T> ResponseSuccess(string message, T data)
        {
            return new ResponseObject<T>(StatusCodes.Status200OK, message, data);
        }
        public ResponseObject<T> ResponseError(int status, string message, T data)
        {
            return new ResponseObject<T>(status, message, data);
        }
    }
}
