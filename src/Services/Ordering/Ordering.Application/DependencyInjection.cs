namespace Ordering.Application
{
    using System.Reflection;

    using FluentValidation;

    using MediatR;

    using Microsoft.Extensions.DependencyInjection;

    using Ordering.Application.Behaviours;

    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            services.AddAutoMapper(executingAssembly);
            services.AddValidatorsFromAssembly(executingAssembly);
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(executingAssembly));

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

            return services;
        }
    }
}
