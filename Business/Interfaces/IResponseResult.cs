namespace Business.Interfaces;

public interface IResponseResult
{
    bool Success { get; }
    int StatusCode { get; }
    string? ErrorMessage { get; }
}
