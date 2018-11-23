using IdentityModel.Client;
using OpenTl.Schema;
using System.Threading.Tasks;

namespace FWT.Core.Services.Identity
{
    public interface IIdentityModelClient
    {
        Task<TokenResponse> RequestClientCredentialsTokenAsync(TUser user);
    }
}