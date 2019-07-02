using System.ServiceModel;

namespace QIQO.Business.Client.Proxies
{
    public class ProxyBase
    {
        protected virtual System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfigurationType endpointConfigurationType)
        {
            if ((endpointConfigurationType == EndpointConfigurationType.BasicHttpBinding))
            {
                var result = new BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            if ((endpointConfigurationType == EndpointConfigurationType.NetTcpBinding))
            {
                var result = new NetTcpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                return result;
            }
            throw new System.InvalidOperationException(string.Format("\"Cannot find endpoint with name \'{0}\'.\"", endpointConfigurationType));
        }

        protected virtual EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
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

    public enum EndpointConfiguration
    {

        BasicHttpBinding_IOrderService,
        NetTcpBinding_IOrderService,
        BasicHttpBinding_IInvoiceService,
        NetTcpBinding_IInvoiceService,
        BasicHttpBinding_IAccountService,
        NetTcpBinding_IAccountService,
        BasicHttpBinding_IAddressService,
        NetTcpBinding_IAddressService,
        BasicHttpBinding_ICompanyService,
        NetTcpBinding_ICompanyService,
        BasicHttpBinding_IEmployeeService,
        NetTcpBinding_IEmployeeService,
        BasicHttpBinding_IEntityProductService,
        NetTcpBinding_IEntityProductService,
        BasicHttpBinding_IFeeScheduleService,
        NetTcpBinding_IFeeScheduleService,
        BasicHttpBinding_IProductService,
        NetTcpBinding_IProductService,
        BasicHttpBinding_ITypeService,
        NetTcpBinding_ITypeService,
        NetTcpBinding_IIdentityUserService,
        BasicHttpBinding_IIdentityUserService,
        NetTcpBinding_IIdentityRoleService,
        BasicHttpBinding_IIdentityRoleService
    }

    public enum EndpointConfigurationType
    {
        BasicHttpBinding,
        NetTcpBinding,
    }
}
