using BlazorApp.Domain;

namespace BlazorApp.Application.Interfaces.Validation
{
    public interface IError
    {
        ErrorCode Code { get; }
        string Message { get; }
    }
}