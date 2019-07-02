using QIQO.Business.Client.Entities;
using QIQO.Business.Core;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace QIQO.Business.Client.Contracts
{
    [ServiceContract]
    public interface IFeeScheduleService : IServiceContract, IDisposable
    {
        [OperationContract]
        List<FeeSchedule> GetFeeSchedulesByAccount(Account account);

        [OperationContract]
        List<FeeSchedule> GetFeeScheduleByCompany(Company company);

        [OperationContract]
        int CreateFeeSchedule(FeeSchedule fee_schedule);

        [OperationContract]
        bool DeleteFeeSchedule(FeeSchedule fee_schedule);

        [OperationContract]
        FeeSchedule GetFeeSchedule(int fee_schedule);



        [OperationContract]
        Task<List<FeeSchedule>> GetFeeSchedulesByAccountAsync(Account account);

        [OperationContract]
        Task<List<FeeSchedule>> GetFeeScheduleByCompanyAsync(Company company);

        [OperationContract]
        Task<int> CreateFeeScheduleAsync(FeeSchedule fee_schedule);

        [OperationContract]
        Task<bool> DeleteFeeScheduleAsync(FeeSchedule fee_schedule);

        [OperationContract]
        Task<FeeSchedule> GetFeeScheduleAsync(int fee_schedule);
    }
}