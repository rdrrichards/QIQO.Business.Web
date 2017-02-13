using QIQO.Business.Client.Contracts;
using QIQO.Business.Client.Entities;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace QIQO.Business.Client.Proxies
{
    public class EmployeeClient : ProxyBase, IEmployeeService
    {
        private IEmployeeService channel = null;

        public EmployeeClient() : this(EndpointConfiguration.NetTcpBinding_IEmployeeService) { }

        public EmployeeClient(EndpointConfiguration endpoint)
        {
            channel = new ChannelFactory<IEmployeeService>(GetBindingForEndpoint(EndpointConfigurationType.NetTcpBinding),
                GetEndpointAddress(endpoint)).CreateChannel();
        }

        public int CreateEmployee(Employee employee)
        {
            return channel.CreateEmployee(employee);
        }

        public Task<int> CreateEmployeeAsync(Employee employee)
        {
            return channel.CreateEmployeeAsync(employee);
        }

        public bool DeleteEmployee(Employee employee)
        {
            return channel.DeleteEmployee(employee);
        }

        public Task<bool> DeleteEmployeeAsync(Employee employee)
        {
            return channel.DeleteEmployeeAsync(employee);
        }

        public List<Representative> GetAccountRepsByCompany(int company_key)
        {
            return channel.GetAccountRepsByCompany(company_key);
        }

        public Task<List<Representative>> GetAccountRepsByCompanyAsync(int company_key)
        {
            return channel.GetAccountRepsByCompanyAsync(company_key);
        }

        public Employee GetEmployee(int entity_person_key)
        {
            return channel.GetEmployee(entity_person_key);
        }

        public Task<Employee> GetEmployeeAsync(int entity_person_key)
        {
            return channel.GetEmployeeAsync(entity_person_key);
        }

        public Employee GetEmployeeByCredentials(string user_name)
        {
            return channel.GetEmployeeByCredentials(user_name);
        }

        public Task<Employee> GetEmployeeByCredentialsAsync(string user_name)
        {
            return channel.GetEmployeeByCredentialsAsync(user_name);
        }

        public List<Employee> GetEmployees(Company company)
        {
            return channel.GetEmployees(company);
        }

        public Task<List<Employee>> GetEmployeesAsync(Company company)
        {
            return channel.GetEmployeesAsync(company);
        }

        public List<Representative> GetSalesRepsByCompany(int company_key)
        {
            return channel.GetSalesRepsByCompany(company_key);
        }

        public Task<List<Representative>> GetSalesRepsByCompanyAsync(int company_key)
        {
            return channel.GetSalesRepsByCompanyAsync(company_key);
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
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_IEmployeeService))
            {
                return new EndpointAddress("http://localhost:7476/QIQOEmployeeService/QIQOEmployeeService");
            }
            if ((endpointConfiguration == EndpointConfiguration.NetTcpBinding_IEmployeeService))
            {
                return new EndpointAddress("net.tcp://localhost:7478/QIQOEmployeeService/QIQOEmployeeService");
            }
            throw new System.InvalidOperationException(string.Format("\"Cannot find endpoint with name \'{0}\'.\"", endpointConfiguration));
        }
    }
}