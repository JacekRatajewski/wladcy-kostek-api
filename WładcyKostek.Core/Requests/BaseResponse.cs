
using System.Net;

namespace WładcyKostek.Core.Requests
{
    public class BaseResponse<T>
    {
        public BaseResponse(HttpStatusCode errorCode, string errorMessage)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        public BaseResponse(HttpStatusCode errorCode, T result)
        {
            ErrorCode = errorCode;
            Result = result;
        }
        public HttpStatusCode ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }
        public T? Result { get; set; }

        internal static BaseResponse<T> CreateError(string errorMessage)
        {
            return new BaseResponse<T>(HttpStatusCode.InternalServerError, errorMessage);
        }

        internal static BaseResponse<T> CreateResult(T result)
        {
            return new BaseResponse<T>(HttpStatusCode.OK, result);
        }
    }
}
