using QIQO.Business.Client.Contracts;
using QIQO.Business.Client.Entities;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace QIQO.Business.Client.Proxies
{
    public class CompanyClient : ProxyBase, ICompanyService
    {
        private ICompanyService channel = null;

        public CompanyClient() : this(EndpointConfiguration.NetTcpBinding_ICompanyService) { }

        public CompanyClient(EndpointConfiguration endpoint)
        {
            channel = new ChannelFactory<ICompanyService>(GetBindingForEndpoint(EndpointConfigurationType.NetTcpBinding),
                GetEndpointAddress(endpoint)).CreateChannel();
        }

        public int CompanyAddEmployee(Company company, Employee emp, string role, string comment)
        {
            return channel.CompanyAddEmployee(company, emp, role, comment);
        }

        public Task<int> CompanyAddEmployeeAsync(Company company, Employee emp, string role, string comment)
        {
            return channel.CompanyAddEmployeeAsync(company, emp, role, comment);
        }

        public bool CompanyDeleteEmployee(Company company, Employee emp)
        {
            return channel.CompanyDeleteEmployee(company, emp);
        }

        public Task<bool> CompanyDeleteEmployeeAsync(Company company, Employee emp)
        {
            return channel.CompanyDeleteEmployeeAsync(company, emp);
        }

        public int CreateCompany(Company company)
        {
            return channel.CreateCompany(company);
        }

        public Task<int> CreateCompanyAsync(Company company)
        {
            return channel.CreateCompanyAsync(company);
        }

        public bool DeleteCompany(Company company)
        {
            return channel.DeleteCompany(company);
        }

        public Task<bool> DeleteCompanyAsync(Company company)
        {
            return channel.DeleteCompanyAsync(company);
        }

        public List<Company> GetCompanies(Employee emp)
        {
            return channel.GetCompanies(emp);
        }

        public Task<List<Company>> GetCompaniesAsync(Employee emp)
        {
            return channel.GetCompaniesAsync(emp);
        }

        public Company GetCompany(int company_key)
        {
            return channel.GetCompany(company_key);
        }

        public Task<Company> GetCompanyAsync(int company_key)
        {
            return channel.GetCompanyAsync(company_key);
        }

        public string GetCompanyNextNumber(Company company, QIQOEntityNumberType number_type)
        {
            return channel.GetCompanyNextNumber(company, number_type);
        }

        public Task<string> GetCompanyNextNumberAsync(Company company, QIQOEntityNumberType number_type)
        {
            return channel.GetCompanyNextNumberAsync(company, number_type);
        }

        public string GetEmployeeRoleInCompany(Employee emp)
        {
            return channel.GetEmployeeRoleInCompany(emp);
        }

        public Task<string> GetEmployeeRoleInCompanyAsync(Employee emp)
        {
            return channel.GetEmployeeRoleInCompanyAsync(emp);
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
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_ICompanyService))
            {
                return new EndpointAddress("http://localhost:7476/QIQOCompanyService/QIQOCompanyService");
            }
            if ((endpointConfiguration == EndpointConfiguration.NetTcpBinding_ICompanyService))
            {
                return new EndpointAddress("net.tcp://localhost:7478/QIQOCompanyService/QIQOCompanyService");
            }
            throw new System.InvalidOperationException(string.Format("\"Cannot find endpoint with name \'{0}\'.\"", endpointConfiguration));
        }
    }
}