namespace Discount.Grpc.Repositories
{
    using Dapper;

    using Discount.Grpc.Entities;

    using Microsoft.Extensions.Configuration;

    using Npgsql;

    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration configuration;

        public DiscountRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<bool> CreateDiscountAsync(Coupon coupon)
        {
            using NpgsqlConnection connection = GetDatabaseConnection();

            int affectedRows = await connection.ExecuteAsync(
                "INSERT INTO coupons (product_name, description, amount) VALUES (@ProductName, @Description, @Amount)",
                new
                {
                    coupon.ProductName,
                    coupon.Description,
                    coupon.Amount,
                }
            );

            return affectedRows > 0;
        }

        public async Task<bool> DeleteDiscountAsync(string productName)
        {
            using NpgsqlConnection connection = GetDatabaseConnection();

            int affectedRows = await connection.ExecuteAsync(
                "DELETE FROM coupons WHERE product_name = @ProductName",
                new { ProductName = productName }
            );

            return affectedRows > 0;
        }

        public async Task<Coupon> GetDiscountAsync(string productName)
        {
            using NpgsqlConnection connection = GetDatabaseConnection();

            Coupon coupon = await connection.QueryFirstOrDefaultAsync<Coupon>(
                "SELECT product_name AS ProductName, * FROM coupons WHERE product_name = @ProductName",
                new { ProductName = productName }
            );

            if (coupon == null)
            {
                return new Coupon
                {
                    ProductName = "No Discount",
                    Amount = 0,
                    Description = "No Discount Desc",
                };
            }

            return coupon;
        }

        public async Task<bool> UpdateDiscountAsync(Coupon coupon)
        {
            using NpgsqlConnection connection = GetDatabaseConnection();

            int affectedRows = await connection.ExecuteAsync(
                "UPDATE coupons SET product_name=@ProductName, description = @Description, amount = @Amount WHERE Id = @Id",
                new
                {
                    coupon.ProductName,
                    coupon.Description,
                    coupon.Amount,
                    coupon.Id
                }
            );

            return affectedRows > 0;
        }

        private NpgsqlConnection GetDatabaseConnection()
        {
            string connectionString = configuration.GetValue<string>("DatabaseSettings:ConnectionString");
            return new NpgsqlConnection(connectionString);
        }
    }
}
