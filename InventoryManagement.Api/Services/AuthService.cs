using InventoryManagement.Api.Services.Secure;
using InventoryManagement.Domain.Models.RequestModel;
using Microsoft.AspNetCore.Mvc;
using Moonlight.Response.Response;
using Serilog;

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
        var methodName = nameof(VerifyToken);
        //Log.Information("Metod: {Method} - Token doğrulama başlatıldı. Token: {Token}", methodName, validate.Token);

        if (validate.Token.StartsWith("Bearer "))
            validate.Token = validate.Token.Substring(7);

        var isValid = _authProcessors.VerifyToken(validate.Token);

        if (isValid)
        {
            //Log.Information("Metod: {Method} - Token geçerli. Token: {Token}", methodName, validate.Token);
            return new CoreResponse<bool>
            {
                Data = true,
                ResponseCode = ResponseCode.Success,
                Message = "Token geçerli.",
                ErrorMessages = new List<string>()
            };
        }
        else
        {
            //Log.Warning("Metod: {Method} - Geçersiz token. Token: {Token}", methodName, validate.Token);
            return new CoreResponse<bool>
            {
                Data = false,
                ResponseCode = ResponseCode.InvalidToken,
                Message = "Token geçerli değil.",
                ErrorMessages = new List<string> { "Geçersiz veya süresi dolmuş token." }
            };
        }
    }

}
