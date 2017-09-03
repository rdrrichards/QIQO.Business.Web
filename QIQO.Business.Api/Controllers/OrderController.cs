using Microsoft.AspNetCore.Mvc;
using QIQO.Business.Client.Contracts;
using QIQO.Business.Client.Entities;
using QIQO.Business.Core;
using QIQO.Business.Services;
using QIQO.Business.ViewModels.Api;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QIQO.Business.Api.Controllers
{
    public class OrderController : Controller
    {
        private readonly IServiceFactory _serviceFactory;
        private readonly IEntityService _entityService;

        public OrderController(IServiceFactory serviceFactory, IEntityService entityService)
        {
            _serviceFactory = serviceFactory;
            _entityService = entityService;
        }

        [HttpGet("api/orders/{order_key}")]
        public async Task<JsonResult> Get(int order_key)
        {
            Order ord;
            try
            {
                using (var proxy = _serviceFactory.CreateClient<IOrderService>())
                {
                    ord = await proxy.GetOrderAsync(order_key);
                }

                var order_vm = _entityService.Map(ord);

                foreach (var item in ord.OrderItems)
                    order_vm.OrderItems.Add(_entityService.Map(item));

                return Json(order_vm);
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }

        [HttpPost("api/orders")]
        public async Task<JsonResult> Post([FromBody] OrderViewModel order)
        {
            try
            {
                using (var proxy = _serviceFactory.CreateClient<IOrderService>())
                {
                    var order_key = await proxy.CreateOrderAsync(_entityService.Map(order)); // Save order
                    var new_order = await proxy.GetOrderAsync(order_key); // Get the new order and send it back to the client
                    return Json(new_order);
                }
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }

        [HttpPut("api/orders")]
        public async Task<JsonResult> Put([FromBody] OrderViewModel order)
        {
            return await Post(order);
        }

        [HttpDelete("api/orders/{order_key}")]
        public async Task<JsonResult> Delete(int order_key)
        {
            try
            {
                var order = new Order() { OrderKey = order_key };
                using (var proxy = _serviceFactory.CreateClient<IOrderService>())
                {
                    var order_res = await proxy.DeleteOrderAsync(order); // delete order
                    return Json(order_res);
                }
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }

        [HttpGet("api/accounts/{account_key}/orders")]
        public async Task<JsonResult> GetAccountOrders(int account_key)
        {
            List<Order> ords;
            var account = new Account() { AccountKey = account_key };
            try
            {
                using (var proxy = _serviceFactory.CreateClient<IOrderService>())
                {
                    //Task<List<Order>> order = proxy.GetOrdersByAccountAsync(_entity_service.MapAccountViewModelToAccount(account));
                    ords = await proxy.GetOrdersByAccountAsync(account);
                }

                var acct_vms = new List<OrderViewModel>();

                foreach (var ord in ords)
                {
                    var order_vm = _entityService.Map(ord);

                    foreach (var item in ord.OrderItems)
                        order_vm.OrderItems.Add(_entityService.Map(item));

                    acct_vms.Add(order_vm);
                }

                return Json(acct_vms);

            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }

        [HttpGet("api/accounts/{account_key}/orders/{order_key}")]
        public async Task<JsonResult> GetAccountOrder(int account_key, int order_key)
        {
            Order ord;
            //var account = new Account() { AccountKey = account_key };
            try
            {
                using (var proxy = _serviceFactory.CreateClient<IOrderService>())
                {
                    ord = await proxy.GetOrderAsync(order_key);
                }

                if (ord.Account.AccountKey != account_key)
                    throw new InvalidOperationException("Order -> account access violation");

                var order_vm = _entityService.Map(ord);

                foreach (var item in ord.OrderItems)
                    order_vm.OrderItems.Add(_entityService.Map(item));

                return Json(order_vm);

            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }

        [HttpGet("api/openorders")]
        public async Task<JsonResult> Get()
        {
            List<Order> ords;
            var company = new Company() { CompanyKey = 1 };
            try
            {
                using (var proxy = _serviceFactory.CreateClient<IOrderService>())
                {
                    ords = await proxy.GetOrdersByCompanyAsync(company);
                }

                var acct_vms = new List<OrderViewModel>();

                foreach (var ord in ords)
                {
                    var order_vm = _entityService.Map(ord);

                    foreach (var item in ord.OrderItems)
                        order_vm.OrderItems.Add(_entityService.Map(item));

                    acct_vms.Add(order_vm);
                }

                return Json(acct_vms);

            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }

        [HttpGet("api/orders&q={q}")]
        public async Task<JsonResult> Get(string q = "")
        {
            if (string.IsNullOrWhiteSpace(q)) return Json(new List<OrderViewModel>());

            List<Order> ords;
            var company = new Company() { CompanyKey = 1 };
            try
            {
                using (var proxy = _serviceFactory.CreateClient<IOrderService>())
                {
                    ords = await proxy.FindOrdersByCompanyAsync(company, q);
                }

                var acct_vms = new List<OrderViewModel>();

                foreach (var ord in ords)
                {
                    var order_vm = _entityService.Map(ord);

                    foreach (var item in ord.OrderItems)
                        order_vm.OrderItems.Add(_entityService.Map(item));

                    acct_vms.Add(order_vm);
                }

                return Json(acct_vms);

            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }
    }
}
