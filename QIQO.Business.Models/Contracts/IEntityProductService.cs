using QIQO.Business.Client.Entities;
using QIQO.Business.Core;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace QIQO.Business.Client.Contracts
{
    [ServiceContract]
    public interface IEntityProductService : IServiceContract, IDisposable
    {
        [OperationContract]
        List<EntityProduct> GetAllEntityProducts();

        [OperationContract]
        List<EntityProduct> GetEntityProducts(Company company);

        [OperationContract]
        int CreateEntityProduct(EntityProduct product);

        [OperationContract]
        bool DeleteEntityProduct(EntityProduct product);

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        EntityProduct GetEntityProduct(int product_key);



        [OperationContract]
        Task<List<EntityProduct>> GetAllEntityProductsAsync();

        [OperationContract]
        Task<List<EntityProduct>> GetEntityProductsAsync(Company company);

        [OperationContract]
        Task<int> CreateEntityProductAsync(EntityProduct product);

        [OperationContract]
        Task<bool> DeleteEntityProductAsync(EntityProduct product);

        [OperationContract]
        Task<EntityProduct> GetEntityProductAsync(int product_key);
    }
}
