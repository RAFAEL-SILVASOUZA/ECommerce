using ECommerce.Order.Domain.Entities;
using ECommerce.Order.Domain.Services.Contracts;
using Flurl;
using Flurl.Http;

namespace ECommerce.Order.Domain.Services;

public class CatalogItemService : ICatalogItemService
{
    private readonly Url _baseUrl;
    public CatalogItemService(IConfiguration configuration)
       => _baseUrl = configuration["Catalog:url"];

    public async Task<IList<CatalogItem>> GetProductsByIdsAsync(Guid[] ids)
    {
        var request = _baseUrl
            .AppendPathSegment("/ids")
            .SetQueryParam("ids", ids);
        return await request.GetJsonAsync<CatalogItem[]>();
    }
}
