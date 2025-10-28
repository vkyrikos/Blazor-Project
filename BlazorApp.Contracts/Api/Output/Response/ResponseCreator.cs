namespace BlazorApp.Contracts.Api.Output.Response;

public static class ResponseCreator
{
    public static ResponseDto<T> GetSuccessResponse<T>(T data) where T : class => new()
    {
        ResponseCode = ResponseCodeDto.Success,
        Data = data,
        ResponseResult = null
    };

    public static ResponseDto<T> GetPartialSuccessResponse<T>(T data, ResponseResultDto? responseResult) where T : class => new()
    {
        ResponseCode = ResponseCodeDto.PartialSuccess,
        Data = data,
        ResponseResult = responseResult
    };

    public static ResponseDto GetFailureResponse(ResponseResultDto? responseResult) => new()
    {
        ResponseCode = ResponseCodeDto.Failure,
        ResponseResult = responseResult
    };
}
