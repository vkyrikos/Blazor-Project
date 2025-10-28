namespace BlazorApp.Contracts.Api.Output.Response;

public static class ResponseCreator
{
    public static ResponseDto<T> GetSuccessResponse<T>(T data) where T : class
    {
        return new()
        {
            ResponseCode = ResponseCodeDto.Success,
            Data = data,
            ResponseResult = new()
        };
    }

    public static ResponseDto<T> GetPartialSuccessResponse<T>(T data, ResponseResultDto? responseResult) where T : class
    {
        return new()
        {
            ResponseCode = ResponseCodeDto.PartialSuccess,
            Data = data,
            ResponseResult = responseResult ?? new()
        };
    }

    public static ResponseDto GetFailureResponse(ResponseResultDto? responseResult)
    {
        return new()
        {
            ResponseCode = ResponseCodeDto.Failure,
            ResponseResult = responseResult ?? new()
        };
    }
}
