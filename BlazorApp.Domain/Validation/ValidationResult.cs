namespace BlazorApp.Domain.Validation;

internal class ValidationResult
{
    public static ValidationResult DefaultValid = new();

    private readonly ValidationError[] _errors = [];
    public bool IsValid => !_errors.Any();

    public ValidationError[] Errors => _errors;

    public ValidationResult()
    {
    }

    public ValidationResult(ValidationError[] errors)
    {
        if (errors is not null)
        {
            _errors = errors;
        }
    }
}
