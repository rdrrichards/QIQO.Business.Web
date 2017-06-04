using Microsoft.Extensions.DependencyInjection;
using QIQO.Business.Core;

namespace QIQO.Business.Api
{
    public class ServiceFactory : IServiceFactory
    {
        private readonly IServiceCollection _services;

        public ServiceFactory(IServiceCollection services)
        {
            _services = services;
        }

        public T CreateClient<T>() where T : IServiceContract
        {
            var p = _services.BuildServiceProvider();
            return p.GetService<T>();
        }
    }
}
