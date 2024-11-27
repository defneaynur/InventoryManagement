using Core.Config.Injection;
using Core.Config.Config;
using System.Data.SqlClient;
using System.Data;
using InventoryManagement.Api.Services.Processor;
using InventoryManagement.Api.Services.Secure;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryManagement.Api.Base;

public static class ConfigureInjection
{
    private static readonly IBaseInjection _baseInjection;
	public static void BaseInject(this WebApplicationBuilder builder)
	{
		builder.BaseDefaultInjection();

        var connectionString = builder.GetConfigFromAppSettings<ConfigProject>();
        builder.Services.AddSingleton<IDbConnection>(sp => new SqlConnection(connectionString.ApiInformations.ConnectionStrings.DefaultConnection));
        builder.Services.AddScoped<IProductProcessors, ProductProcessors>();
        builder.Services.AddScoped<IUserProcessors, UserProcessors>();
        builder.Services.AddScoped<IAuthProcessors, AuthProcessors>();
        builder.Services.AddScoped<IStockProcessors, StockProcessors>();
        builder.Services.AddScoped<IPriceProcessors, PriceProcessors>();
        builder.Services.AddScoped<ICategoryProcessors, CategoryProcessors>();

       
    }
 
}
