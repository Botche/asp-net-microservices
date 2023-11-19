namespace Discount.Grpc.Extensions
{
    using Npgsql;

    public static class DatabaseMigrationExtensions
    {
        public static IServiceCollection MigrateDatabase<TContext>(this IServiceCollection services, IConfiguration configuration, int? retry = 0)
        {
            ServiceProvider serviceProvider = services.BuildServiceProvider();
            ILogger logger = serviceProvider.GetService<ILogger<TContext>>();

            string connectionString = configuration.GetValue<string>("DatabaseSettings:ConnectionString");
            var connection = new NpgsqlConnection(connectionString);

            try
            {
                logger.LogInformation("Migrating postgres database.");

                connection.Open();

                using var command = new NpgsqlCommand
                {
                    Connection = connection,
                };

                command.ExecuteDropTableIfExtis();
                command.ExecuteCreateTableCoupons();
                command.ExecuteSeedTableCoupons();

                logger.LogInformation("Migrated postgres database.");
            }
            catch (NpgsqlException ex)
            {
                logger.LogError(ex, "An error occurred while migrating the postgres database");

                int retryForAvailability = retry.Value;
                if (retryForAvailability < 50)
                {
                    retryForAvailability++;
                    Thread.Sleep(2000);
                    services.MigrateDatabase<TContext>(configuration, retryForAvailability);
                }
            }
            finally
            {
                connection.Close();
            }

            return services;
        }

        private static void ExecuteSeedTableCoupons(this NpgsqlCommand command)
        {
            command.CommandText = "INSERT INTO coupons (product_name, description, amount) VALUES ('IPhone X', 'IPhone  Discount', '150');";
            command.ExecuteNonQuery();

            command.CommandText = "INSERT INTO coupons (product_name, description, amount) VALUES ('Samsung 10', 'Samsung Discount', '100');";
            command.ExecuteNonQuery();
        }

        private static void ExecuteCreateTableCoupons(this NpgsqlCommand command)
        {
            command.CommandText = @"CREATE TABLE coupons (
                    id SERIAL PRIMARY KEY,
                    product_name VARCHAR(24) NOT NULL,
                    description TEXT,
                    amount INT)";
            command.ExecuteNonQuery();
        }

        private static void ExecuteDropTableIfExtis(this NpgsqlCommand command)
        {
            command.CommandText = "DROP TABLE IF EXISTS coupons";
            command.ExecuteNonQuery();
        }
    }
}
