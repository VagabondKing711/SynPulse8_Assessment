namespace SynPulse8_Identity
{
    public class BaseErrorResponse<T>
    {
        public StatusCode ErrorCode { get; set; }

        public T? Data { get; set; }

        public string? ErrorMsg { get; set; }
    }

    public enum StatusCode
    {
        Ok = 200,
        BadRequest = 400,
        Unauthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        Conflict = 409
    }
}