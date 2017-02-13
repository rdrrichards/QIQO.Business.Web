using QIQO.Business.Client.Contracts;
using QIQO.Business.Client.Entities;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace QIQO.Business.Client.Proxies
{
    public class InvoiceClient : ProxyBase, IInvoiceService
    {
        private IInvoiceService channel = null;

        public InvoiceClient() : this(EndpointConfiguration.NetTcpBinding_IInvoiceService) { }

        public InvoiceClient(EndpointConfiguration endpoint)
        {
            channel = new ChannelFactory<IInvoiceService>(GetBindingForEndpoint(EndpointConfigurationType.NetTcpBinding),
                GetEndpointAddress(endpoint)).CreateChannel();
        }

        public int CreateInvoice(Invoice order)
        {
            return channel.CreateInvoice(order);
        }

        public Task<int> CreateInvoiceAsync(Invoice order)
        {
            return channel.CreateInvoiceAsync(order);
        }

        public bool DeleteInvoice(Invoice order)
        {
            return channel.DeleteInvoice(order);
        }

        public Task<bool> DeleteInvoiceAsync(Invoice order)
        {
            return channel.DeleteInvoiceAsync(order);
        }

        public Invoice GetInvoice(int order_key)
        {
            return channel.GetInvoice(order_key);
        }

        public Task<Invoice> GetInvoiceAsync(int order_key)
        {
            return channel.GetInvoiceAsync(order_key);
        }

        public List<Invoice> GetInvoicesByAccount(Account account)
        {
            return channel.GetInvoicesByAccount(account);
        }

        public Task<List<Invoice>> GetInvoicesByAccountAsync(Account account)
        {
            return channel.GetInvoicesByAccountAsync(account);
        }

        public List<Invoice> GetInvoicesByCompany(Company company)
        {
            return channel.GetInvoicesByCompany(company);
        }

        public Task<List<Invoice>> GetInvoicesByCompanyAsync(Company company)
        {
            return channel.GetInvoicesByCompanyAsync(company);
        }

        public List<Invoice> FindInvoicesByCompany(Company company, string search_pattern)
        {
            return channel.FindInvoicesByCompany(company, search_pattern);
        }

        public Task<List<Invoice>> FindInvoicesByCompanyAsync(Company company, string search_pattern)
        {
            return channel.FindInvoicesByCompanyAsync(company, search_pattern);
        }

        public InvoiceItem GetInvoiceItemByOrderItemKey(int order_item_key)
        {
            return channel.GetInvoiceItemByOrderItemKey(order_item_key);
        }

        public Task<InvoiceItem> GetInvoiceItemByOrderItemKeyAsync(int order_item_key)
        {
            return channel.GetInvoiceItemByOrderItemKeyAsync(order_item_key);
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
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_IInvoiceService))
            {
                return new EndpointAddress("http://localhost:7476/QIQOInvoiceService/QIQOInvoiceService");
            }
            if ((endpointConfiguration == EndpointConfiguration.NetTcpBinding_IInvoiceService))
            {
                return new EndpointAddress("net.tcp://localhost:7478/QIQOInvoiceService/QIQOInvoiceService");
            }
            throw new System.InvalidOperationException(string.Format("\"Cannot find endpoint with name \'{0}\'.\"", endpointConfiguration));
        }
    }
}
