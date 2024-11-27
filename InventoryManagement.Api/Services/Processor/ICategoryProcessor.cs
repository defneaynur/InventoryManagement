using Dapper;
using InventoryManagement.Domain.Models.DatabaseModel;
using System.Data;

namespace InventoryManagement.Api.Services.Processor;
public interface ICategoryProcessors
{
    Task<IEnumerable<Categories>> GetCategoriesAsync();
    Task<Categories> CreateCategoryAsync(Categories category);
    Task<Categories> UpdateCategoryAsync(Categories product);
    Task<bool> DeleteCategoryAsync(long id);
    Task<Categories> GetCategoryWithIdAsync(long categoryId);
}

public class CategoryProcessors : ICategoryProcessors
{
    private readonly IDbConnection _dbConnection;

    public CategoryProcessors(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    /// <summary>
    /// This method run for Category create operation.
    /// </summary>
    /// <param name="category">Categories Datamodel</param>
    /// <returns></returns>
    public async Task<Categories> CreateCategoryAsync(Categories category)
    {
        const string query = @"
                INSERT INTO Categories (CategoryName, Description, Creator, IsDeleted)
                VALUES (@CategoryName, @Description, @Creator, @IsDeleted)";

        var result = await _dbConnection.ExecuteAsync(query, category);
    
        return category;
    }

    /// <summary>
    /// This method run for Category delete operation
    /// </summary>
    /// <param name="id">Category Id</param>
    /// <returns></returns>
    public async Task<bool> DeleteCategoryAsync(long id)
    {
        const string query = @"UPDATE Categories SET IsDeleted = 1 WHERE Id = @Id";

        var result = await _dbConnection.ExecuteAsync(query, new { Id = id });

        return result == 1;
    }

    /// <summary>
    /// This method return Categories
    /// </summary>
    /// <returns>Categories Datamodel</returns>
    public async Task<IEnumerable<Categories>> GetCategoriesAsync()
    {
        const string query = "SELECT * FROM Categories WHERE IsDeleted = 0 OR IsDeleted IS NULL";

        var result = await _dbConnection.QueryAsync<Categories>(query);

        return result;
    }

    /// <summary>
    /// This method run for Category update operation.
    /// </summary>
    /// <param name="category">Categories Datamodel</param>
    /// <returns></returns>
    public async Task<Categories> UpdateCategoryAsync(Categories category)
    {
        const string query = @"UPDATE Categories SET CategoryName = @CategoryName, Description = @Description, Changer = @Changer, Changed = @Changed WHERE Id = @Id";

        var result = await _dbConnection.ExecuteAsync(query, category);

        return  category;
    }

    /// <summary>
    /// This method return category filtered with id  
    /// </summary>
    /// <param name="categoryId"></param>
    /// <returns></returns>
    public async Task<Categories> GetCategoryWithIdAsync(long categoryId)
    {
        const string query = "SELECT * FROM Categories WHERE Id = @Id AND (IsDeleted IS NULL OR IsDeleted = 0)";

        var result = await _dbConnection.QuerySingleOrDefaultAsync<Categories>(query, new { Id = categoryId });

        return result;
    }


}