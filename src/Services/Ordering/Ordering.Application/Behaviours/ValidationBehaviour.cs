namespace Ordering.Application.Behaviours
{
    using System.Threading;
    using System.Threading.Tasks;

    using FluentValidation;
    using FluentValidation.Results;

    using MediatR;

    using Ordering.Application.Exceptions;

    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> validators;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            this.validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (validators.Any())
            {
                ValidationContext<TRequest> context = new(request);

                ValidationResult[] validationResults = await Task.WhenAll(
                    validators.Select(v => v.ValidateAsync(context, cancellationToken))
                );

                List<ValidationFailure> failures = validationResults
                    .SelectMany(r => r.Errors)
                    .Where(f => f != null)
                    .ToList();

                if (failures.Any())
                {
                    throw new CustomValidationException(failures);
                }
            }

            return await next();
        }
    }
}
