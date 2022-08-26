using Dapper;
using Discount.Grpc.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Threading.Tasks;

namespace Discount.Grpc.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _config;

        public DiscountRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task<Coupon> GetDiscount(string productName)
        {
            //creates a connection to the postgre sql via the connection string
            using var connection = new NpgsqlConnection(_config.GetValue<string>("DatabaSettings:ConnectionString"));
            
            //using dapper to get the data from the database

            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>("SELECT * FROM Coupon WHERE ProductName = @ProductName", new { ProductName = productName });

            if (coupon == null) return new Coupon() { ProductName = productName ,Amount = 0,Description = "No discount exist for this"};
            return coupon;
        }
        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_config.GetValue<string>("DatabaSettings:ConnectionString"));
            var create = await connection.ExecuteAsync
                ("INSERT INTO Coupon(ProductName, Description, Amount) VALUES(@ProductName ,@Description, @Amount)",
                new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount });
            if (create == 0)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            using var connection = new NpgsqlConnection(_config.GetValue<string>("DatabaSettings:ConnectionString"));
            var toDelete =await connection.ExecuteAsync
                ("DELETE FROM Coupon WHERE ProductName = @ProductName",
                new { ProductName = productName });
            if (toDelete == 0)
            {
                return false;
            }
            return true;
        }


        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_config.GetValue<string>("DatabaSettings:ConnectionString"));
            var updated = await connection.ExecuteAsync
                ("UPDATE Coupon SET ProductName=@ProductName, Description=@Description, Amount=@Amount WHERE Id=@Id",
                new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount, Id = coupon.Id});
            if (updated == 0)
            {
                return false;
            }
            return true;
        }
    }
}
