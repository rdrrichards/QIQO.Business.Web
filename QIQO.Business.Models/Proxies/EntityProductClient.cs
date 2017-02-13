using QIQO.Business.Client.Contracts;
using QIQO.Business.Client.Entities;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace QIQO.Business.Client.Proxies
{
    public class EntityProductClient : ProxyBase, IEntityProductService
    {
        private IEntityProductService channel = null;
        
        public EntityProductClient() : this(EndpointConfiguration.NetTcpBinding_IEntityProductService) { }

        public EntityProductClient(EndpointConfiguration endpoint)
        {
            channel = new ChannelFactory<IEntityProductService>(GetBindingForEndpoint(EndpointConfigurationType.NetTcpBinding),
                GetEndpointAddress(endpoint)).CreateChannel();
        }

        public int CreateEntityProduct(EntityProduct product)
        {
            return channel.CreateEntityProduct(product);
        }

        public Task<int> CreateEntityProductAsync(EntityProduct product)
        {
            return channel.CreateEntityProductAsync(product);
        }

        public bool DeleteEntityProduct(EntityProduct product)
        {
            return channel.DeleteEntityProduct(product);
        }

        public Task<bool> DeleteEntityProductAsync(EntityProduct product)
        {
            return channel.DeleteEntityProductAsync(product);
        }

        public List<EntityProduct> GetAllEntityProducts()
        {
            return channel.GetAllEntityProducts();
        }

        public Task<List<EntityProduct>> GetAllEntityProductsAsync()
        {
            return channel.GetAllEntityProductsAsync();
        }

        public EntityProduct GetEntityProduct(int product_key)
        {
            return channel.GetEntityProduct(product_key);
        }

        public Task<EntityProduct> GetEntityProductAsync(int product_key)
        {
            return channel.GetEntityProductAsync(product_key);
        }

        public List<EntityProduct> GetEntityProducts(Company company)
        {
            return channel.GetEntityProducts(company);
        }

        public Task<List<EntityProduct>> GetEntityProductsAsync(Company company)
        {
            return channel.GetEntityProductsAsync(company);
        }

        public void Dispose()
        {
            if (channel != null)
            {
                channel.Dispose();
                channel = null;
            }
        }

        protected override EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_IEntityProductService))
            {
                return new EndpointAddress("http://localhost:7476/QIQOEntityProductService/QIQOEntityProductService");
            }
            if ((endpointConfiguration == EndpointConfiguration.NetTcpBinding_IEntityProductService))
            {
                return new EndpointAddress("net.tcp://localhost:7478/QIQOEntityProductService/QIQOEntityProductService");
            }
            throw new System.InvalidOperationException(string.Format("\"Cannot find endpoint with name \'{0}\'.\"", endpointConfiguration));
        }
    }
}