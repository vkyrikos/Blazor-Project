namespace BlazorApp.Contracts.Api.Output.Response;

public sealed class ResponseDto<T> : ResponseDto where T : class
{
    public T Data { get; init; }

    public ResponseDto()
    {
        
    }
}

public class ResponseDto
{
    public ResponseCodeDto ResponseCode { get; init; }

    public ResponseResultDto ResponseResult { get; init; }

    public ResponseDto()
    {
        
    }
}
