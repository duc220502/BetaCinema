using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Common
{
    public class ResponseObject<T>
    {
        public string Message { get; set; }
        public T? Data { get; set; }

        private ResponseObject( string message, T? data)
        {
            Message = message;
            Data = data;
        }
        public static ResponseObject<T> ResponseSuccess(string message, T? data)
        {
            return new ResponseObject<T>( message, data);
        }
        public static ResponseObject<T> ResponseError(string message, T? data)
        {
            return new ResponseObject<T>( message, data);
        }
    }
}
