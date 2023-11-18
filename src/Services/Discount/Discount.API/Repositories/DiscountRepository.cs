namespace Discount.API.Repositories
{
    using System.Runtime.InteropServices;

    using Dapper;

    using Discount.API.Entities;

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
            using NpgsqlConnection connection = this.GetDatabaseConnection();

            int affectedRows = await connection.ExecuteAsync(
                "INSERT INTO coupons (product_name, description, amount) VALUES (@ProductName, @Description, @Amount)",
                new
                {
                    ProductName = coupon.ProductName,
                    Description = coupon.Description,
                    Amount = coupon.Amount,
                }
            );

            return affectedRows > 0;
        }

        public async Task<bool> DeleteDiscountAsync(string productName)
        {
            using NpgsqlConnection connection = this.GetDatabaseConnection();

            int affectedRows = await connection.ExecuteAsync(
                "DELETE FROM coupons WHERE product_name = @ProductName",
                new { ProductName = productName }
            );

            return affectedRows > 0;
        }

        public async Task<Coupon> GetDiscountAsync(string productName)
        {
            using NpgsqlConnection connection = this.GetDatabaseConnection();

            Coupon coupon = await connection.QueryFirstOrDefaultAsync<Coupon>(
                "SELECT * FROM coupons WHERE product_name = @ProductName",
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
            using NpgsqlConnection connection = this.GetDatabaseConnection();

            int affectedRows = await connection.ExecuteAsync(
                "UPDATE Coupon SET ProductName=@ProductName, Description = @Description, Amount = @Amount WHERE Id = @Id",
                new
                {
                    ProductName = coupon.ProductName,
                    Description = coupon.Description,
                    Amount = coupon.Amount,
                    Id = coupon.Id
                }
            );

            return affectedRows > 0;
        }

        private NpgsqlConnection GetDatabaseConnection()
        {
            string connectionString = this.configuration.GetValue<string>("DatabaseSettings:ConnectionString");
            return new NpgsqlConnection(connectionString);
        }
    }
}
