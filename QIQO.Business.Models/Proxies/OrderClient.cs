using QIQO.Business.Client.Contracts;
using QIQO.Business.Client.Entities;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace QIQO.Business.Client.Proxies
{
    public class OrderClient : ProxyBase, IOrderService
    {
        private IOrderService channel = null;

        public OrderClient() : this(EndpointConfiguration.NetTcpBinding_IOrderService) { }

        public OrderClient(EndpointConfiguration endpoint)
        {
            channel = new ChannelFactory<IOrderService>(GetBindingForEndpoint(EndpointConfigurationType.NetTcpBinding), 
                GetEndpointAddress(endpoint)).CreateChannel();
        }

        public int CreateOrder(Order order)
        {
            return channel.CreateOrder(order);
        }

        public Task<int> CreateOrderAsync(Order order)
        {
            return channel.CreateOrderAsync(order);
        }

        public bool DeleteOrder(Order order)
        {
            return channel.DeleteOrder(order);
        }

        public Task<bool> DeleteOrderAsync(Order order)
        {
            return channel.DeleteOrderAsync(order);
        }

        public Order GetOrder(int order_key)
        {
            return channel.GetOrder(order_key);
        }

        public Task<Order> GetOrderAsync(int order_key)
        {
            return channel.GetOrderAsync(order_key);
        }

        public List<Order> GetOrdersByAccount(Account account)
        {
            return channel.GetOrdersByAccount(account);
        }

        public Task<List<Order>> GetOrdersByAccountAsync(Account account)
        {
            return channel.GetOrdersByAccountAsync(account);
        }

        public List<Order> GetOrdersByCompany(Company company)
        {
            return channel.GetOrdersByCompany(company);
        }

        public Task<List<Order>> GetOrdersByCompanyAsync(Company company)
        {
            return channel.GetOrdersByCompanyAsync(company);
        }

        public List<Order> FindOrdersByCompany(Company company, string search_pattern)
        {
            return channel.FindOrdersByCompany(company, search_pattern);
        }

        public Task<List<Order>> FindOrdersByCompanyAsync(Company company, string search_pattern)
        {
            return channel.FindOrdersByCompanyAsync(company, search_pattern);
        }

        public List<Order> GetInvoicableOrdersByAccount(int company_key, int account_key)
        {
            return channel.GetInvoicableOrdersByAccount(company_key, account_key);
        }

        public Task<List<Order>> GetInvoicableOrdersByAccountAsync(int company_key, int account_key)
        {
            return channel.GetInvoicableOrdersByAccountAsync(company_key, account_key);
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
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_IOrderService))
            {
                return new EndpointAddress("http://localhost:7476/QIQOOrderService/QIQOOrderService");
            }
            if ((endpointConfiguration == EndpointConfiguration.NetTcpBinding_IOrderService))
            {
                return new EndpointAddress("net.tcp://localhost:7478/QIQOOrderService/QIQOOrderService");
            }
            throw new System.InvalidOperationException(string.Format("\"Cannot find endpoint with name \'{0}\'.\"", endpointConfiguration));
        }
    }
}