namespace BlazorApp.Contracts.Api.Output.Response;

public sealed class ResponseResultDto
{
    public ResponseResultTypeDto Type { get; init; }
    public string Code { get; init; }
}
