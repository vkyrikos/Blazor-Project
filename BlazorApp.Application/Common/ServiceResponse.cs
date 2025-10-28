using BlazorApp.Application.Interfaces.Common;
using BlazorApp.Application.Interfaces.Validation;

namespace BlazorApp.Application.Common;

public sealed class ServiceResponse<T> : IServiceResponse<T>
{
    public T? Model { get; }

    public IError? Error { get; }

    public bool IsSuccess => Error is null;

    public ServiceResponse(T model)
    {
        Model = model;
    }

    public ServiceResponse(IError error)
    {
        Error = error;
    }

    public static ServiceResponse<T> Failure(IError error) => new(error);

    public static ServiceResponse<T> Success(T model) => new(model);
}
