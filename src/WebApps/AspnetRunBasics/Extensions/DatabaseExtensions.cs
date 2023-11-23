namespace AspnetRunBasics.Extensions
{
    using AspnetRunBasics.Data;

    public static class DatabaseExtensions
    {
        public static void SeedDatabase(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

            try
            {
                var aspnetRunContext = serviceProvider.GetRequiredService<AspnetRunContext>();
                AspnetRunContextSeed.SeedAsync(aspnetRunContext, loggerFactory).Wait();
            }
            catch (Exception exception)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(exception, "An error occurred seeding the DB.");
            }
        }
    }
}
