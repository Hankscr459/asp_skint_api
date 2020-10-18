using System.Threading.Tasks;
using Core.Entities;
using Core.Interface;

namespace Infrastructure.Data
{
    public class BasketRepository : IBasketRepository
    {
        public Task<bool> DeleteBasketAsync(string basketId)
        {
            throw new System.NotImplementedException();
        }

        public Task<CustomerBasket> GetBasketAsync(string basketId)
        {
            throw new System.NotImplementedException();
        }

        public Task<CustomerBasket> UpdateBasketAsync(string basketId)
        {
            throw new System.NotImplementedException();
        }
    }
}