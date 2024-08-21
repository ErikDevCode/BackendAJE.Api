using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BackEndAje.Api.Shared.Abstractions
{
    public interface IInstaller
    {
        void InstallServices(IServiceCollecion services, IConfiguration configuration);
    }
}
