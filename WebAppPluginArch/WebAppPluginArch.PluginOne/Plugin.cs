using Microsoft.Extensions.DependencyInjection;
using WebAppPluginArch.Shared;

namespace WebAppPluginArch.PluginOne
{
    public class Plugin : IPlugin
    {
        public void Initialize(IServiceCollection services)
        {
            services.AddSingleton<PluginService>();
        }
    }
}
