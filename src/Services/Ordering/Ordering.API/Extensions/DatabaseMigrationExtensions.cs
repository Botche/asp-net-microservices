namespace Discount.API.Extensions
{
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;

    public static class DatabaseMigrationExtensions
    {
        public static WebApplication MigrateDatabase<TContext>(this WebApplication app,
            Action<TContext, IServiceProvider> seeder,
            int? retry = 0)
            where TContext : DbContext
        {
            var logger = app.Services.GetRequiredService<ILogger<TContext>>();
            var context = app.Services.GetService<TContext>();
            
            try
            {
                logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(TContext).Name);

                InvokeSeeder(seeder, context, app.Services);

                logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(TContext).Name);
            }
            catch (SqlException ex)
            {
                logger.LogError(ex, "An error occurred while migrating the database used on context {DbContextName}", typeof(TContext).Name);

                int retryForAvailability = retry.Value;
                if (retryForAvailability < 50)
                {
                    retryForAvailability++;
                    Thread.Sleep(2000);
                    app.MigrateDatabase(seeder, retryForAvailability);
                }
            }

            return app;
        }

        private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder,
            TContext context,
            IServiceProvider serviceProvider)
            where TContext : DbContext
        {
            context.Database.Migrate();
            seeder(context, serviceProvider);
        }
    }
}
