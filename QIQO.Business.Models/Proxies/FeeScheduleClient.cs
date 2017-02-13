using QIQO.Business.Client.Contracts;
using QIQO.Business.Client.Entities;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace QIQO.Business.Client.Proxies
{
    public class FeeScheduleClient : ProxyBase, IFeeScheduleService
    {
        private IFeeScheduleService channel = null;

        //[InjectionConstructor]
        public FeeScheduleClient() : this(EndpointConfiguration.NetTcpBinding_IFeeScheduleService) { }

        public FeeScheduleClient(EndpointConfiguration endpoint)
        {
            channel = new ChannelFactory<IFeeScheduleService>(GetBindingForEndpoint(EndpointConfigurationType.NetTcpBinding),
                GetEndpointAddress(endpoint)).CreateChannel();
        }

        public int CreateFeeSchedule(FeeSchedule fee_schedule)
        {
            return channel.CreateFeeSchedule(fee_schedule);
        }

        public Task<int> CreateFeeScheduleAsync(FeeSchedule fee_schedule)
        {
            return channel.CreateFeeScheduleAsync(fee_schedule);
        }

        public bool DeleteFeeSchedule(FeeSchedule fee_schedule)
        {
            return channel.DeleteFeeSchedule(fee_schedule);
        }

        public Task<bool> DeleteFeeScheduleAsync(FeeSchedule fee_schedule)
        {
            return channel.DeleteFeeScheduleAsync(fee_schedule);
        }

        public FeeSchedule GetFeeSchedule(int fee_schedule)
        {
            return channel.GetFeeSchedule(fee_schedule);
        }

        public Task<FeeSchedule> GetFeeScheduleAsync(int fee_schedule)
        {
            return channel.GetFeeScheduleAsync(fee_schedule);
        }

        public List<FeeSchedule> GetFeeScheduleByCompany(Company company)
        {
            return channel.GetFeeScheduleByCompany(company);
        }

        public Task<List<FeeSchedule>> GetFeeScheduleByCompanyAsync(Company company)
        {
            return channel.GetFeeScheduleByCompanyAsync(company);
        }

        public List<FeeSchedule> GetFeeSchedulesByAccount(Account account)
        {
            return channel.GetFeeSchedulesByAccount(account);
        }

        public Task<List<FeeSchedule>> GetFeeSchedulesByAccountAsync(Account account)
        {
            return channel.GetFeeSchedulesByAccountAsync(account);
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
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_IFeeScheduleService))
            {
                return new EndpointAddress("http://localhost:7476/QIQOFeeScheduleService/QIQOFeeScheduleService");
            }
            if ((endpointConfiguration == EndpointConfiguration.NetTcpBinding_IFeeScheduleService))
            {
                return new EndpointAddress("net.tcp://localhost:7478/QIQOFeeScheduleService/QIQOFeeScheduleService");
            }
            throw new System.InvalidOperationException(string.Format("\"Cannot find endpoint with name \'{0}\'.\"", endpointConfiguration));
        }
    }
}