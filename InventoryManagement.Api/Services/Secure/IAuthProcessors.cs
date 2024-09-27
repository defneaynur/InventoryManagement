namespace InventoryManagement.Api.Services.Secure
{
    public interface IAuthProcessors
    {
        bool VerifyToken(string token);
    }

}
