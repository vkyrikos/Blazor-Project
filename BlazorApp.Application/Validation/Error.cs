using BlazorApp.Application.Interfaces.Validation;
using BlazorApp.Domain;

namespace BlazorApp.Application.Validation;

public sealed class Error : IError
{
    public ErrorCode Code { get; }

    public string Message { get; }

    public Error(ErrorCode code, string message)
    {
        Code = code;
        Message = message;
    }
}
