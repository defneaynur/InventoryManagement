using Dapper;
using InventoryManagement.Domain.Models.DatabaseModel;
using InventoryManagement.Domain.Models.ResponseModel;
using Moonlight.ExceptionHandling.Exceptions;
using System.Data;

namespace InventoryManagement.Api.Services.Processor;
public interface IPriceProcessors
{
    Task<PriceHistoryLog> CreatePriceHistoryLogAsync(PriceHistoryLog stockMovement);
    Task<IEnumerable<PriceHistoryResponse>> GetPriceHistoriesAsync();
    Task<IEnumerable<PriceHistoryResponse>> GetPriceHistoryWithIdAsync(long id);
}

public class PriceProcessors : IPriceProcessors
{
    private readonly IDbConnection _dbConnection;

    public PriceProcessors(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    /// <summary>
    /// This method return PriceHistories
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<PriceHistoryResponse>> GetPriceHistoriesAsync()
    {
        const string query = "SELECT ph.*,p.ProductName FROM PriceHistoryLog AS ph INNER JOIN Products AS p ON ph.ProductId = p.Id  WHERE p.IsDeleted = 0 OR p.IsDeleted IS NULL";

        var result = await _dbConnection.QueryAsync<PriceHistoryResponse>(query);

        return result;
    }

    /// <summary>
    /// This method run for PriceHistoryLog create operation.
    /// </summary>
    /// <param name="priceHistory"></param>
    /// <returns></returns>
    public async Task<PriceHistoryLog> CreatePriceHistoryLogAsync(PriceHistoryLog priceHistory)
    {
        const string query = @"
                INSERT INTO PriceHistoryLog (ProductId, CostPrice, OldSalePrice, CurrentSalePrice, Note, Creator)
                VALUES (@ProductId, @CostPrice, @OldSalePrice, @OldSalePrice, @Note, @Creator)"
        ;
        var result = await _dbConnection.ExecuteAsync(query, priceHistory);

        return priceHistory;
    }

    /// <summary>
    /// This method return PriceHistory filtered with id  
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="CoreNotificationException"></exception>
    public async Task<IEnumerable<PriceHistoryResponse>> GetPriceHistoryWithIdAsync(long id)
    {
        const string query = "SELECT ph.*, p.ProductName FROM PriceHistoryLog AS ph INNER JOIN Products AS p ON ph.ProductId = p.Id WHERE (p.IsDeleted = 0 OR p.IsDeleted IS NULL) AND ph.ProductId = @ProductId ORDER BY ph.Created DESC";

        var result = await _dbConnection.QueryAsync<PriceHistoryResponse>(query, new { ProductId = id });

        if (result.Count() == 0)
            throw new CoreException("Record Not Found");

        return result;
    }
}

