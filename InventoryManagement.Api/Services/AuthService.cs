using InventoryManagement.Api.Services.Secure;
using InventoryManagement.Domain.Models.RequestModel;
using Microsoft.AspNetCore.Mvc;
using Moonlight.Response.Response;

[Route("api/[controller]")]
[ApiController]
public class AuthService : ControllerBase
{
    private readonly IAuthProcessors _authProcessors;

    public AuthService(IAuthProcessors authProcessors)
    {
        _authProcessors = authProcessors;
    }



    [HttpPost("tokenVerify")]
    public CoreResponse<bool> VerifyToken(ValidateRequest validate)
    {
        if (validate.Token.StartsWith("Bearer "))
            validate.Token = validate.Token.Substring(7);

        var isValid = _authProcessors.VerifyToken(validate.Token);

        if (isValid)
        {
            return new CoreResponse<bool>
            {
                Data = true,
                CoreResponseCode = CoreResponseCode.Success,
                Message = "Token geçerli.",
                ErrorMessages = new List<string>()
            };
        }
        else
        {
            return new CoreResponse<bool>
            {
                Data = false,
                CoreResponseCode = CoreResponseCode.InvalidToken,
                Message = "Token geçerli değil.",
                ErrorMessages = new List<string> { "Geçersiz veya süresi dolmuş token." }
            };
        }
    }
}
