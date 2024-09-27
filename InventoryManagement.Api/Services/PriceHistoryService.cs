using Core.Framework.Response;
using InventoryManagement.Api.Services.Processor;
using InventoryManagement.Domain.Models.DatabaseModel;
using InventoryManagement.Domain.Models.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Api.Services;

[ApiController]
[Route("api/[controller]")]
public class PriceHistoryService : ControllerBase
{

    private readonly IPriceProcessors _priceProcessors;

    public PriceHistoryService(IPriceProcessors priceProcessors)
    {
        _priceProcessors = priceProcessors;
    }

    [HttpGet]
    public async Task<CoreResponse<IEnumerable<PriceHistoryResponse>>> GetPriceHistories()
    {
        var result = await _priceProcessors.GetPriceHistoriesAsync();
        if (!result.Any())
        {
            return new CoreResponse<IEnumerable<PriceHistoryResponse>>
            {
                Data = null,
                ResponseCode = CoreResponseCode.NoData,
                ErrorMessages = new List<string>(),
                Message = "Aradığınız kriterlerde data bulunamadı."
            };
        }

        return new CoreResponse<IEnumerable<PriceHistoryResponse>>
        {
            Data = result,
            ResponseCode = CoreResponseCode.Success,
            ErrorMessages = new List<string>(),
            Message = ""
        };
    }

    [HttpGet("{id}")]
    public async Task<CoreResponse<IEnumerable<PriceHistoryResponse>>> GetPriceHistoryWithId(long id)
    {
        var result = await _priceProcessors.GetPriceHistoryWithIdAsync(id);
        if (!result.Any())
        {
            return new CoreResponse<IEnumerable<PriceHistoryResponse>>
            {
                Data = null,
                ResponseCode = CoreResponseCode.NoData,
                ErrorMessages = new List<string>(),
                Message = "Aradığınız kriterlerde data bulunamadı."
            };
        }

        return new CoreResponse<IEnumerable<PriceHistoryResponse>>
        {
            Data = result,
            ResponseCode = CoreResponseCode.Success,
            ErrorMessages = new List<string>(),
            Message = ""
        };
    }

}

