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
        public JsonResult Get(int order_key)
        {
            Task<Order> order;
            Order ord;
            try
            {
                using (var proxy = _serviceFactory.CreateClient<IOrderService>())
                {
                    order = proxy.GetOrderAsync(order_key);
                    ord = order.Result;
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
        public JsonResult Post([FromBody] OrderViewModel order)
        {
            try
            {
                using (var proxy = _serviceFactory.CreateClient<IOrderService>())
                {
                    var order_key = proxy.CreateOrderAsync(_entityService.Map(order)); // Save order
                    var new_order = proxy.GetOrderAsync(order_key.Result); // Get the new order and send it back to the client
                    return Json(new_order.Result);
                }
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }

        [HttpPut("api/orders")]
        public JsonResult Put([FromBody] OrderViewModel order)
        {
            return Post(order);
        }

        [HttpDelete("api/orders/{order_key}")]
        public JsonResult Delete(int order_key)
        {
            try
            {
                var order = new Order() { OrderKey = order_key };
                using (var proxy = _serviceFactory.CreateClient<IOrderService>())
                {
                    var order_res = proxy.DeleteOrderAsync(order); // delete order
                    return Json(order_res.Result);
                }
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }

        [HttpGet("api/accounts/{account_key}/orders")]
        public JsonResult GetAccountOrders(int account_key)
        {
            List<Order> ords;
            var account = new Account() { AccountKey = account_key };
            try
            {
                using (var proxy = _serviceFactory.CreateClient<IOrderService>())
                {
                    //Task<List<Order>> order = proxy.GetOrdersByAccountAsync(_entity_service.MapAccountViewModelToAccount(account));
                    var orders = proxy.GetOrdersByAccountAsync(account);
                    ords = orders.Result;
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
        public JsonResult GetAccountOrder(int account_key, int order_key)
        {
            Order ord;
            //var account = new Account() { AccountKey = account_key };
            try
            {
                using (var proxy = _serviceFactory.CreateClient<IOrderService>())
                {
                    var orders = proxy.GetOrderAsync(order_key);
                    ord = orders.Result;
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
        public JsonResult Get()
        {
            List<Order> ords;
            var company = new Company() { CompanyKey = 1 };
            try
            {
                using (var proxy = _serviceFactory.CreateClient<IOrderService>())
                {
                    var orders = proxy.GetOrdersByCompanyAsync(company);
                    ords = orders.Result;
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
        public JsonResult Get(string q = "")
        {
            if (string.IsNullOrWhiteSpace(q)) return Json(new List<OrderViewModel>());

            List<Order> ords;
            var company = new Company() { CompanyKey = 1 };
            try
            {
                using (var proxy = _serviceFactory.CreateClient<IOrderService>())
                {
                    var orders = proxy.FindOrdersByCompanyAsync(company, q);
                    ords = orders.Result;
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
