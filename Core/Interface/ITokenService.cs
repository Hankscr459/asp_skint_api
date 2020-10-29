using System.Threading.Tasks;
using Core.Entities.Identity;

namespace Core.Interface
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
    }
}