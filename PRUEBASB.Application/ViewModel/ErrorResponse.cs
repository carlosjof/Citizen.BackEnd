using System.Net;

namespace PRUEBASB.Application.ViewModel
{
    public class ErrorResponse
    {
        public bool IsSuccess { get; set; } = false;
        public IDictionary<string, string[]>? Errors { get; set; }
        public string? Error { get; set; }

        public ErrorResponse(bool isSuccess, IDictionary<string, string[]> errors)
        {
            IsSuccess = isSuccess;
            Errors = (Dictionary<string, string[]>?)errors;
        }

        public ErrorResponse(bool isSuccess, string error) {
            IsSuccess = isSuccess;
            Error = error;
        }
    }
}
