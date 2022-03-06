namespace Restaurant.Web.Data
{
    public class Response<T> where T : class
    {
        public int ResponseCode { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
    }
}