namespace BlazorApp.Domain.Validation;

public sealed class ValidationError
{
    public string Message { get; init; } = string.Empty;
    public string[] MemberNames { get; init; } = [];
}
