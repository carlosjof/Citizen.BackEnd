using System.Net;

namespace PRUEBASB.Application.ViewModel
{
    public class SuccessResponse
    {
        public bool IsSuccess { get; set; } = true;
        public object Data { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public List<string> Errors { get; set; }


        public SuccessResponse(bool isSuccess, object data, HttpStatusCode statusCode, List<string> errors)
        {
            IsSuccess = isSuccess;
            Data = data;
            StatusCode = statusCode;
            Errors = errors;
        }
    }
}
