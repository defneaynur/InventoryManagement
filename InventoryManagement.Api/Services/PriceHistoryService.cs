using InventoryManagement.Api.Services.Processor;
using InventoryManagement.Domain.Models.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using Moonlight.Response.Response;

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
                ResponseCode = ResponseCode.NoData,
                ErrorMessages = new List<string>(),
                Message = "Aradığınız kriterlerde data bulunamadı."
            };
        }

        return new CoreResponse<IEnumerable<PriceHistoryResponse>>
        {
            Data = result,
            ResponseCode = ResponseCode.Success,
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
                ResponseCode = ResponseCode.NoData,
                ErrorMessages = new List<string>(),
                Message = "Aradığınız kriterlerde data bulunamadı."
            };
        }

        return new CoreResponse<IEnumerable<PriceHistoryResponse>>
        {
            Data = result,
            ResponseCode = ResponseCode.Success,
            ErrorMessages = new List<string>(),
            Message = ""
        };
    }

}

