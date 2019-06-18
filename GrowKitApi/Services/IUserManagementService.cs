using GrowKitApi.Services.Enums;
using GrowKitApi.Services.Structs;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GrowKitApi.Services
{
    public interface IUserManagementService
    {
        Task<UserResult> ValidateUserAsync(string emailAdress, string password);
        Task<Results> ValidateAuthenticationCodeAsync(string code, long userId);
        Task<UserResult> CreateAsync(string emailAdress, string password);
        Task<string> GenerateEmailConfiramtionTokenAsync(long userId);

        long GetUserID(ClaimsPrincipal cp);
    }
}
