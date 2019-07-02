using QIQO.Business.Client.Contracts;
using QIQO.Business.Client.Entities;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace QIQO.Business.Client.Proxies
{
    public class AccountClient : ProxyBase, IAccountService //UserClientBase<IAccountService>, IAccountService
    {
        private IAccountService channel = null;

        public AccountClient() : this(EndpointConfiguration.NetTcpBinding_IAccountService)
        {
        }

        public AccountClient(EndpointConfiguration endpoint)
        {
            channel = new ChannelFactory<IAccountService>(GetBindingForEndpoint(EndpointConfigurationType.NetTcpBinding),
                GetEndpointAddress(endpoint)).CreateChannel();
        }

        public int CreateAccount(Account account)
        {
            return channel.CreateAccount(account);
        }

        public Task<int> CreateAccountAsync(Account account)
        {
            return channel.CreateAccountAsync(account);
        }

        public bool DeleteAccount(Account account)
        {
            return channel.DeleteAccount(account);
        }

        public Task<bool> DeleteAccountAsync(Account account)
        {
            return channel.DeleteAccountAsync(account);
        }

        public List<Account> FindAccountByCompany(Company company, string pattern)
        {
            return channel.FindAccountByCompany(company, pattern);
        }

        public Task<List<Account>> FindAccountByCompanyAsync(Company company, string pattern)
        {
            return channel.FindAccountByCompanyAsync(company, pattern);
        }

        public Account GetAccountByCode(string account_code, string company_code)
        {
            return channel.GetAccountByCode(account_code, company_code);
        }

        public Task<Account> GetAccountByCodeAsync(string account_code, string company_code)
        {
            return channel.GetAccountByCodeAsync(account_code, company_code);
        }

        public Account GetAccountByID(int account_key, bool full_load)
        {
            return channel.GetAccountByID(account_key, full_load);
        }

        public Task<Account> GetAccountByIDAsync(int account_key, bool full_load)
        {
            return channel.GetAccountByIDAsync(account_key, full_load);
        }

        public string GetAccountNextNumber(Account account, QIQOEntityNumberType number_type)
        {
            return channel.GetAccountNextNumber(account, number_type);
        }

        public Task<string> GetAccountNextNumberAsync(Account account, QIQOEntityNumberType number_type)
        {
            return channel.GetAccountNextNumberAsync(account, number_type);
        }

        public List<Account> GetAccountsByCompany(Company company)
        {
            return channel.GetAccountsByCompany(company);
        }

        public Task<List<Account>> GetAccountsByCompanyAsync(Company company)
        {
            return channel.GetAccountsByCompanyAsync(company);
        }

        public List<Account> GetAccountsByEmployee(Employee employee)
        {
            return channel.GetAccountsByEmployee(employee);
        }

        public Task<List<Account>> GetAccountsByEmployeeAsync(Employee employee)
        {
            return channel.GetAccountsByEmployeeAsync(employee);
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
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_IAccountService))
            {
                return new EndpointAddress("http://localhost:7476/QIQOAccountService/QIQOAccountService");
            }
            if ((endpointConfiguration == EndpointConfiguration.NetTcpBinding_IAccountService))
            {
                return new EndpointAddress("net.tcp://localhost:7478/QIQOAccountService/QIQOAccountService");
            }
            throw new System.InvalidOperationException(string.Format("\"Cannot find endpoint with name \'{0}\'.\"", endpointConfiguration));
        }
    }
}
