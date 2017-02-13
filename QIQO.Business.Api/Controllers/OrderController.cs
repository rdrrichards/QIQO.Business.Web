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
        private readonly IServiceFactory _service_fact;
        private IEntityService _entity_service;

        public OrderController(IServiceFactory services, IEntityService entity_service)
        {
            _service_fact = services;
            _entity_service = entity_service;
        }

        [HttpGet("api/orders/{order_key}")]
        public JsonResult Get(int order_key)
        {
            Task<Order> order;
            Order ord;
            try
            {
                IOrderService proxy = _service_fact.CreateClient<IOrderService>();

                using (proxy)
                {
                    order = proxy.GetOrderAsync(order_key);
                    ord = order.Result;
                }

                OrderViewModel order_vm = _entity_service.Map(ord);

                foreach (var item in ord.OrderItems)
                    order_vm.OrderItems.Add(_entity_service.Map(item));

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
                IOrderService proxy = _service_fact.CreateClient<IOrderService>();

                using (proxy)
                {
                    Task<int> order_key = proxy.CreateOrderAsync(_entity_service.Map(order)); // Save order
                    Task<Order> new_order = proxy.GetOrderAsync(order_key.Result); // Get the new order and send it back to the client
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
                Order order = new Order() { OrderKey = order_key };
                IOrderService proxy = _service_fact.CreateClient<IOrderService>();

                using (proxy)
                {
                    Task<bool> order_res = proxy.DeleteOrderAsync(order); // delete order
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
                IOrderService proxy = _service_fact.CreateClient<IOrderService>();

                using (proxy)
                {
                    //Task<List<Order>> order = proxy.GetOrdersByAccountAsync(_entity_service.MapAccountViewModelToAccount(account));
                    var orders = proxy.GetOrdersByAccountAsync(account);
                    ords = orders.Result;
                }

                List<OrderViewModel> acct_vms = new List<OrderViewModel>();

                foreach (var ord in ords)
                {
                    OrderViewModel order_vm = _entity_service.Map(ord);

                    foreach (var item in ord.OrderItems)
                        order_vm.OrderItems.Add(_entity_service.Map(item));

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
                IOrderService proxy = _service_fact.CreateClient<IOrderService>();

                using (proxy)
                {
                    var orders = proxy.GetOrderAsync(order_key);
                    ord = orders.Result;
                }

                if (ord.Account.AccountKey != account_key)
                    throw new InvalidOperationException("Order -> account access violation");

                OrderViewModel order_vm = _entity_service.Map(ord);

                foreach (var item in ord.OrderItems)
                    order_vm.OrderItems.Add(_entity_service.Map(item));

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
            Company company = new Company() { CompanyKey = 1 };
            try
            {
                IOrderService proxy = _service_fact.CreateClient<IOrderService>();

                using (proxy)
                {
                    var orders = proxy.GetOrdersByCompanyAsync(company);
                    ords = orders.Result;
                }

                List<OrderViewModel> acct_vms = new List<OrderViewModel>();

                foreach (var ord in ords)
                {
                    OrderViewModel order_vm = _entity_service.Map(ord);

                    foreach (var item in ord.OrderItems)
                        order_vm.OrderItems.Add(_entity_service.Map(item));

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
            Company company = new Company() { CompanyKey = 1 };
            try
            {
                IOrderService proxy = _service_fact.CreateClient<IOrderService>();

                using (proxy)
                {
                    var orders = proxy.FindOrdersByCompanyAsync(company, q);
                    ords = orders.Result;
                }

                List<OrderViewModel> acct_vms = new List<OrderViewModel>();

                foreach (var ord in ords)
                {
                    OrderViewModel order_vm = _entity_service.Map(ord);

                    foreach (var item in ord.OrderItems)
                        order_vm.OrderItems.Add(_entity_service.Map(item));

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
