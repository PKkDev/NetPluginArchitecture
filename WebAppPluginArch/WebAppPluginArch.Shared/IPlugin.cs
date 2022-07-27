using Microsoft.Extensions.DependencyInjection;

namespace WebAppPluginArch.Shared
{
    public interface IPlugin
    {
        void Initialize(IServiceCollection services);
    }
}
