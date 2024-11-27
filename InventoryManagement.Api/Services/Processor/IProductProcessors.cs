using Dapper;
using InventoryManagement.Api.Services.Base.Abstract;
using InventoryManagement.Domain.Models.DatabaseModel;
using InventoryManagement.Domain.Models.Enums;
using InventoryManagement.Domain.Models.ResponseModel;
using System.Data;

namespace InventoryManagement.Api.Services.Processor;
public interface IProductProcessors
{
    Task<IEnumerable<ProductResponse>> GetProductsAsync();
    Task<Products> CreateProductAsync(Products product);
    Task<Products> UpdateProductAsync(Products product);
    Task<bool> DeleteProductAsync(long id);
}

public class ProductProcessors : BaseRepository, IProductProcessors
{
    private readonly IDbConnection _dbConnection;
    private readonly IStockProcessors _stockProcessors;
    private readonly IPriceProcessors _priceProcessors;

    public ProductProcessors(IDbConnection dbConnection, IStockProcessors stockProcessors, IPriceProcessors priceProcessors) : base(dbConnection)
    {
        _dbConnection = dbConnection;
        _stockProcessors = stockProcessors;
        _priceProcessors = priceProcessors;
    }

    /// <summary>
    /// This method return Products data
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<ProductResponse>> GetProductsAsync()
    {
        const string query = "SELECT p.*,c.CategoryName AS Category FROM Products AS p INNER JOIN " +
                            "Categories AS c ON p.CategoryId = c.Id  WHERE p.IsDeleted = 0 OR p.IsDeleted IS NULL";

        var result = await _dbConnection.QueryAsync<ProductResponse>(query);

        return result;
    }

    /// <summary>
    /// This method run for Products create operation.
    /// </summary>
    /// <param name="product">Product Datamodel</param>
    /// <returns></returns>
    public async Task<Products> CreateProductAsync(Products product)
    {
        const string query = @"
                INSERT INTO Products (CategoryId, ProductName, Quantity, Description, CostPrice, SalePrice, MinQuantityValue, MaxCapacity, Creator, IsDeleted)
                VALUES (@CategoryId, @ProductName, @Quantity, @Description, @CostPrice, @SalePrice, @MinQuantityValue, @MaxCapacity, @Creator, @IsDeleted)"
        ;
        var result = await _dbConnection.ExecuteAsync(query, product);

        return product;
    }

    /// <summary>
    /// This method run for Products update operation.
    /// </summary>
    /// <param name="product">Product Datamodel</param>
    /// <returns></returns>
    public async Task<Products> UpdateProductAsync(Products product)
    {
        var existingProduct = await base.GetProductWithIdAsync(product.Id);

        bool isQuantityChange = existingProduct?.Quantity != product.Quantity;
        bool isPriceChange = existingProduct?.SalePrice != product.SalePrice;

        var movementType = product.Quantity > existingProduct.Quantity ? MovementType.In : MovementType.Out;

        const string query = @"UPDATE Products SET CategoryId = @CategoryId, ProductName = @ProductName, CostPrice = @CostPrice, SalePrice = @SalePrice, Quantity = @Quantity, Description = @Description, Changer = @Changer, Changed = @Changed WHERE Id = @Id";

        var result = await _dbConnection.ExecuteAsync(query, product);

        #region CreateLog Operations
        if (result == 1)
        {
            if (isQuantityChange)
            {
                _stockProcessors.CreateStockMovements(new()
                {
                    OldQuantity = existingProduct.Quantity,
                    CurrentQuantity = product.Quantity,
                    ProductId = product.Id,
                    MovementType = movementType,
                    Note = "",
                    Creator = "aynur"
                });
            }

            if (isPriceChange)
            {
                _priceProcessors.CreatePriceHistoryLogAsync(new()
                {
                    OldSalePrice = existingProduct.SalePrice,
                    CurrentSalePrice = product.SalePrice,
                    CostPrice = product.CostPrice,
                    ProductId = product.Id,
                    Note = "",
                    Creator = "aynur"
                });
            }
        }
        #endregion

        return product;
    }

    /// <summary>
    /// This method run for Product delete operation
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<bool> DeleteProductAsync(long id)
    {
        const string query = @"UPDATE Products SET IsDeleted = 1 WHERE Id = @Id";

        var result = await _dbConnection.ExecuteAsync(query, new { Id = id });

        return result == 1;
    }

    /// <summary>
    /// This method return Product filtered with category id  
    /// </summary>
    /// <param name="categoryId"></param>
    /// <returns></returns>
    public Products GetProductWithCategoryId(long categoryId)
    {
        const string query = "SELECT * FROM Products WHERE CategoryId = @CategoryId AND (IsDeleted IS NULL OR IsDeleted = 0)";

        var result = _dbConnection.QuerySingleOrDefault<Products>(query, new { CategoryId = categoryId });

        return result;
    }

}
