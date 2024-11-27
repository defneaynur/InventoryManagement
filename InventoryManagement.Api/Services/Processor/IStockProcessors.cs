using Dapper;
using InventoryManagement.Api.Services.Base.Abstract;
using InventoryManagement.Domain.Models.DatabaseModel;
using InventoryManagement.Domain.Models.Enums;
using InventoryManagement.Domain.Models.ResponseModel;
using System.Data;

namespace InventoryManagement.Api.Services.Processor;
public interface IStockProcessors
{
    Task<StockMovements> CreateStockMovements(StockMovements stockMovementLog);
    Task<Products> UpdateStockAsync(Products product);
    Task<IEnumerable<StockMovementsResponse>> GetStockMovementsAsync();
}

public class StockProcessors : BaseRepository, IStockProcessors
{
    private readonly IDbConnection _dbConnection;

    public StockProcessors(IDbConnection dbConnection) : base(dbConnection)
    {
        _dbConnection = dbConnection;
    }
    /// <summary>
    /// This method run When updated product quantity. Adder stock movements
    /// </summary>
    /// <param name="stockMovement">StockMovements Model</param>
    /// <returns></returns>
    public async Task<StockMovements> CreateStockMovements(StockMovements stockMovement)
    {
        const string query = @"
                INSERT INTO StockMovements (ProductId, OldQuantity, CurrentQuantity, MovementType, Note, Creator)
                VALUES (@ProductId, @OldQuantity, @CurrentQuantity, @MovementType, @Note, @Creator)"
        ;
        var result = await _dbConnection.ExecuteAsync(query, stockMovement);

        return stockMovement;
    }

    /// <summary>
    /// This method run for Product stock update operation.
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    public async Task<Products> UpdateStockAsync(Products product)
    {
        var existingProduct = await base.GetProductWithIdAsync(product.Id);

        bool isQuantityChange = existingProduct?.Quantity != product.Quantity;

        var movementType = product.Quantity > existingProduct.Quantity ? MovementType.In : MovementType.Out;

        const string query = @"UPDATE Products SET  Quantity = @Quantity, Changer = @Changer, Changed = @Changed WHERE Id = @Id";

        var result = await _dbConnection.ExecuteAsync(query, product);

        #region CreateLog Operations
        if (result == 1)
        {
            if (isQuantityChange)
            {
                CreateStockMovements(new()
                {
                    OldQuantity = existingProduct.Quantity,
                    CurrentQuantity = product.Quantity,
                    ProductId = product.Id,
                    MovementType = movementType,
                    Note = "",
                    Creator = product.Creator
                });
            }

        }
        #endregion

        return product;
    }

    /// <summary>
    /// This method return StockMovements
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<StockMovementsResponse>> GetStockMovementsAsync()
    {
        const string query = "SELECT  s.Id, s.ProductId, s.OldQuantity, s.CurrentQuantity, s.MovementType, s.Note, s.Created, s.Creator, p.ProductName, c.CategoryName, c.Id AS CategoryId FROM            StockMovements AS s INNER JOIN Products AS p ON s.ProductId = p.Id INNER JOIN Categories AS c ON p.CategoryId = c.Id WHERE        (p.IsDeleted = 0) OR(p.IsDeleted IS NULL)";

        var result = await _dbConnection.QueryAsync<StockMovementsResponse>(query);

        return result;
    }
}

