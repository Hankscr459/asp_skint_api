using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interface
{
    public interface IBasketRepository
    {
        Task<CustomerBasket> GetBasketAsync(string basketId);
        Task<CustomerBasket> UpdateBasketAsync(string basketId);
        Task<bool> DeleteBasketAsync(string basketId);
    }
}