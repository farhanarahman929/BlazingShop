using BlazingShop.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazingShop.Shared.Services
{
    public interface IBlazingShopServices
    {
        Task<IEnumerable<Products>> GetAllProductAsync();
        Task AddProductAsync(Products product);
        Task UpdateProductAsync(Products product);
        Task DeleteProductAsync(int productId);

        Task<List<string>> GetDistinctTitlesAsync();
        Task<List<string>> GetDistinctDescriptionsAsync();
    }
}