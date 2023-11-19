using FluentValidation.Results;

namespace Ordering.Application.Exceptions
{
    public class CustomValidationException : ApplicationException
    {
        public CustomValidationException()
            : base("One or more validation failures have occured.")
        {
            this.Errors = new Dictionary<string, string[]>();
        }

        public CustomValidationException(IEnumerable<ValidationFailure> failures)
            : this()
        {
            this.Errors = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
        }

        public IDictionary<string, string[]> Errors { get; }
    }
}
