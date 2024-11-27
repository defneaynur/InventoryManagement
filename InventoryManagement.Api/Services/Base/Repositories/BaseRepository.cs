using Dapper;
using InventoryManagement.Domain.Models.DatabaseModel;
using System.Data;

namespace InventoryManagement.Api.Services.Base.Abstract
{
    public abstract class BaseRepository
    {
        private readonly IDbConnection _dbConnection;

        public BaseRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        /// <summary>
        /// This method run for common use across different classes(ProductProcessors, StockProcessors)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Products> GetProductWithIdAsync(long id)
        {
            const string query = "SELECT * FROM Products WHERE Id = @Id AND (IsDeleted IS NULL OR IsDeleted = 0)";

            var result = await _dbConnection.QuerySingleOrDefaultAsync<Products>(query, new { Id = id });

            return result;
        }
    }
}
