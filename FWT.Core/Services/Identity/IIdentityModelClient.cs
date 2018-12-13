using System.Threading.Tasks;
using IdentityModel.Client;
using OpenTl.Schema;

namespace FWT.Core.Services.Identity
{
    public interface IIdentityModelClient
    {
        Task<TokenResponse> RequestClientCredentialsTokenAsync(TUser user);
    }
}
