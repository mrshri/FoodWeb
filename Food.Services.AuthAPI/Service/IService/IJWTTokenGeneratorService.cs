using Food.Services.AuthAPI.Models;

namespace Food.Services.AuthAPI.Service.IService
{
    public interface IJWTTokenGeneratorService
    {
        string GenerateToken(ApplicationUser applicationUser, IEnumerable<string> roles );
    }
}
