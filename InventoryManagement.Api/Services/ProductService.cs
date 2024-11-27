using InventoryManagement.Api.Services.Processor;
using InventoryManagement.Domain.Models.DatabaseModel;
using InventoryManagement.Domain.Models.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moonlight.Response.Response;

namespace InventoryManagement.Api.Services;

[ApiController]
[Route("api/[controller]")]
public class ProductService : ControllerBase
{

    private readonly IProductProcessors _productProcessors;

    public ProductService(IProductProcessors productProcessors)
    {
        _productProcessors = productProcessors;
    }

   
    [HttpGet]
    public async Task<CoreResponse<IEnumerable<ProductResponse>>> GetProducts()
    {
        var result = await _productProcessors.GetProductsAsync(); 
        if (!result.Any())
        {
            return new CoreResponse<IEnumerable<ProductResponse>>
            {
                Data = null,
                CoreResponseCode = CoreResponseCode.NoData,
                ErrorMessages = new List<string>(),
                Message = "Aradığınız kriterlerde data bulunamadı."
            };
        }

        return new CoreResponse<IEnumerable<ProductResponse>>
        {
            Data = result,
            CoreResponseCode = CoreResponseCode.Success,
            ErrorMessages = new List<string>(),
            Message = ""
        };
    }

    [Authorize]
    [HttpPost]
    public async Task<CoreResponse<Products>> CreateProductAsync(Products product)
    {
        var result = await _productProcessors.CreateProductAsync(product);


        return new CoreResponse<Products>
        {
            Data = result,
            CoreResponseCode = CoreResponseCode.Success,
            ErrorMessages = new List<string>(),
            Message = ""
        };
    }

    [Authorize]
    [HttpPut("{id}")] 
    public async Task<CoreResponse<Products>> UpdateProductAsync(long id, [FromBody] Products product)
    {
        product.Id = id;

        var result =await _productProcessors.UpdateProductAsync(product);

        return new CoreResponse<Products>
        {
            Data = result,
            CoreResponseCode = CoreResponseCode.Success,
            ErrorMessages = new List<string>(),
            Message = "Ürün başarıyla güncellendi."
        };
    }

    [Authorize]
    [HttpPut("delete/{id}")]
    public async Task<CoreResponse<bool>> DeleteProductAsync(long id)
    {
        var result = await _productProcessors.DeleteProductAsync(id);

        return new CoreResponse<bool>
        {
            Data = result,
            CoreResponseCode = CoreResponseCode.Success,
            ErrorMessages = new List<string>(),
            Message = "Ürün başarıyla güncellendi."
        };
    }
}

