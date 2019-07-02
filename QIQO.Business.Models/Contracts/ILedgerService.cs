using QIQO.Business.Client.Entities;
using QIQO.Business.Core;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace QIQO.Business.Client.Contracts
{
    [ServiceContract]
    public interface ILedgerService : IServiceContract, IDisposable
    {
        [OperationContract]
        List<ChartOfAccount> GetChartOfAccounts(Company company);

        [OperationContract]
        int CreateChartOfAccount(ChartOfAccount chart_of_account);

        [OperationContract]
        bool DeleteChartOfAccount(ChartOfAccount chart_of_account);

        [OperationContract]
        ChartOfAccount GetChartOfAccount(int chart_of_account_key);



        [OperationContract]
        Task<List<ChartOfAccount>> GetChartOfAccountsAsync(Company company);

        [OperationContract]
        Task<int> CreateChartOfAccountAsync(ChartOfAccount chart_of_account);

        [OperationContract]
        Task<bool> DeleteChartOfAccountAsync(ChartOfAccount chart_of_account);

        [OperationContract]
        Task<ChartOfAccount> GetChartOfAccountAsync(int chart_of_account_key);
    }
}