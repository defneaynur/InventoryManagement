using InventoryManagement.Api.Services.Processor;
using InventoryManagement.Domain.Models.DatabaseModel;
using InventoryManagement.Domain.Models.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moonlight.Response.Response;

namespace InventoryManagement.Api.Services;

[ApiController]
[Route("api/[controller]")]
public class StockService : ControllerBase
{

    private readonly IStockProcessors _stockProcessors;

    public StockService(IStockProcessors stockProcessors)
    {
        _stockProcessors = stockProcessors;
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<CoreResponse<Products>> UpdateStockAsync(long id, [FromBody] Products product)
    {
        product.Id = id;

        var result = await _stockProcessors.UpdateStockAsync(product);

        return new CoreResponse<Products>
        {
            Data = result,
            ResponseCode = ResponseCode.Success,
            ErrorMessages = new List<string>(),
            Message = "Stok başarıyla güncellendi."
        };
    }


    [HttpGet]
    public async Task<CoreResponse<IEnumerable<StockMovementsResponse>>> GetStockMovementsAsync()
    {
        var result = await _stockProcessors.GetStockMovementsAsync();
        if (!result.Any())
        {
            return new CoreResponse<IEnumerable<StockMovementsResponse>>
            {
                Data = null,
                ResponseCode = ResponseCode.NoData,
                ErrorMessages = new List<string>(),
                Message = "Aradığınız kriterlerde data bulunamadı."
            };
        }

        return new CoreResponse<IEnumerable<StockMovementsResponse>>
        {
            Data = result,
            ResponseCode = ResponseCode.Success,
            ErrorMessages = new List<string>(),
            Message = ""
        };
    }
}

