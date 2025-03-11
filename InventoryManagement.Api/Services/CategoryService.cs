using InventoryManagement.Api.Services.Processor;
using InventoryManagement.Domain.Models.DatabaseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moonlight.Response.Response;

namespace InventoryManagement.Api.Services
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryService : ControllerBase
    {
        private readonly ICategoryProcessors _categoryProcessors;

        public CategoryService(ICategoryProcessors categoryProcessors)
        {
            _categoryProcessors = categoryProcessors;
        }


        [HttpGet]
        public async Task<CoreResponse<IEnumerable<Categories>>> GetCategories()
        {
            var result = await _categoryProcessors.GetCategoriesAsync();
            if (!result.Any())
            {
                return new CoreResponse<IEnumerable<Categories>>
                {
                    Data = null,
                    ResponseCode = ResponseCode.NoData,
                    ErrorMessages = new List<string>(),
                    Message = "Aradığınız kriterlerde data bulunamadı."
                };
            }

            return new CoreResponse<IEnumerable<Categories>>
            {
                Data = result,
                ResponseCode = ResponseCode.Success,
                ErrorMessages = new List<string>(),
                Message = ""
            };
        }


        [Authorize]
        [HttpPost]
        public async Task<CoreResponse<Categories>> CreateCategoryAsync(Categories category)
        {
            var result = await _categoryProcessors.CreateCategoryAsync(category);


            return new CoreResponse<Categories>
            {
                Data = result,
                ResponseCode = ResponseCode.Success,
                ErrorMessages = new List<string>(),
                Message = ""
            };
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<CoreResponse<Categories>> UpdateCategory(long id, [FromBody] Categories category)
        {
            category.Id = id;

            var result = await _categoryProcessors.UpdateCategoryAsync(category);

            return new CoreResponse<Categories>
            {
                Data = result,
                ResponseCode = ResponseCode.Success,
                ErrorMessages = new List<string>(),
                Message = "Ürün başarıyla güncellendi."
            };
        }


        [Authorize]
        [HttpPut("delete/{id}")]
        public async Task<CoreResponse<bool>> DeleteCategoryAsync(long id)
        {
            var result = await _categoryProcessors.DeleteCategoryAsync(id);

            return new CoreResponse<bool>
            {
                Data = result,
                ResponseCode = ResponseCode.Success,
                ErrorMessages = new List<string>(),
                Message = "Ürün başarıyla silindi."
            };
        }

        [HttpGet("{id}")]
        public async Task<CoreResponse<Categories>> GetCategoryWithId(long id)
        {
            var result = await _categoryProcessors.GetCategoryWithIdAsync(id);


            return new CoreResponse<Categories>
            {
                Data = result,
                ResponseCode = ResponseCode.Success,
                ErrorMessages = new List<string>(),
                Message = ""
            };
        }
    }
}
