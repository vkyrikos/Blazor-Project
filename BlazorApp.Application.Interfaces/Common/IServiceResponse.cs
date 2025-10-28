using BlazorApp.Application.Interfaces.Validation;

namespace BlazorApp.Application.Interfaces.Common
{
    public interface IServiceResponse<T>
    {
        IError? Error { get; }
        bool IsSuccess { get; }
        T? Model { get; }
    }
}