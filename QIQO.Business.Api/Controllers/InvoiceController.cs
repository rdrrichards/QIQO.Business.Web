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
    public class InvoiceController : Controller
    {
        private readonly IServiceFactory _service_fact;
        private IEntityService _entity_service;

        public InvoiceController(IServiceFactory services, IEntityService entity_service)
        {
            _service_fact = services;
            _entity_service = entity_service;
        }

        [HttpGet("api/invoices/{invoice_key}")]
        public JsonResult Get(int invoice_key)
        {
            Task<Invoice> invoice;
            try
            {
                IInvoiceService proxy = _service_fact.CreateClient<IInvoiceService>();

                using (proxy)
                {
                    invoice = proxy.GetInvoiceAsync(invoice_key);
                }
                return Json(_entity_service.Map(invoice.Result));
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }

        [HttpPost("api/invoices")]
        public JsonResult Post([FromBody] InvoiceViewModel invoice)
        {
            try
            {
                IInvoiceService proxy = _service_fact.CreateClient<IInvoiceService>();

                using (proxy)
                {
                    Task<int> invoice_key = proxy.CreateInvoiceAsync(_entity_service.Map(invoice)); // Save invoice
                    Task<Invoice> new_invoice = proxy.GetInvoiceAsync(invoice_key.Result); // Get the new invoice and send it back to the client
                    return Json(new_invoice.Result);
                }
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }

        [HttpPut("api/invoices")]
        public JsonResult Put([FromBody] InvoiceViewModel invoice)
        {
            return Post(invoice);
        }

        [HttpDelete("api/invoices/{invoice_key}")]
        public JsonResult Delete(int invoice_key)
        {
            try
            {
                Invoice invoice = new Invoice() { InvoiceKey = invoice_key };
                IInvoiceService proxy = _service_fact.CreateClient<IInvoiceService>();

                using (proxy)
                {
                    Task<bool> invoice_res = proxy.DeleteInvoiceAsync(invoice); // delete invoice
                    return Json(invoice_res.Result);
                }
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }

        [HttpGet("api/accounts/{account_key}/invoices")]
        public JsonResult GetAccountInvoices(int account_key)
        {
            List<Invoice> ords;
            var account = new Account() { AccountKey = account_key };
            try
            {
                IInvoiceService proxy = _service_fact.CreateClient<IInvoiceService>();

                using (proxy)
                {
                    //Task<List<Invoice>> invoice = proxy.GetInvoicesByAccountAsync(_entity_service.MapAccountViewModelToAccount(account));
                    var invoices = proxy.GetInvoicesByAccountAsync(account);
                    ords = invoices.Result;
                }

                List<InvoiceViewModel> acct_vms = new List<InvoiceViewModel>();

                foreach (var ord in ords)
                {
                    InvoiceViewModel invoice_vm = _entity_service.Map(ord);

                    foreach (var item in ord.InvoiceItems)
                        invoice_vm.InvoiceItems.Add(_entity_service.Map(item));

                    acct_vms.Add(invoice_vm);
                }

                return Json(acct_vms);

            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }

        [HttpGet("api/accounts/{account_key}/invoices/{invoice_key}")]
        public JsonResult GetAccountInvoice(int account_key, int invoice_key)
        {
            Invoice ord;
            //var account = new Account() { AccountKey = account_key };
            try
            {
                IInvoiceService proxy = _service_fact.CreateClient<IInvoiceService>();

                using (proxy)
                {
                    var invoices = proxy.GetInvoiceAsync(invoice_key);
                    ord = invoices.Result;
                }

                if (ord.Account.AccountKey != account_key)
                    throw new InvalidOperationException("Invoice -> account access violation");

                InvoiceViewModel invoice_vm = _entity_service.Map(ord);

                foreach (var item in ord.InvoiceItems)
                    invoice_vm.InvoiceItems.Add(_entity_service.Map(item));

                return Json(invoice_vm);

            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }

        [HttpGet("api/openinvoices")]
        public JsonResult Get()
        {
            List<Invoice> ords;
            Company company = new Company() { CompanyKey = 1 };
            try
            {
                IInvoiceService proxy = _service_fact.CreateClient<IInvoiceService>();

                using (proxy)
                {
                    var invoices = proxy.GetInvoicesByCompanyAsync(company);
                    ords = invoices.Result;
                }

                List<InvoiceViewModel> acct_vms = new List<InvoiceViewModel>();

                foreach (var ord in ords)
                {
                    InvoiceViewModel invoice_vm = _entity_service.Map(ord);

                    foreach (var item in ord.InvoiceItems)
                        invoice_vm.InvoiceItems.Add(_entity_service.Map(item));

                    acct_vms.Add(invoice_vm);
                }

                return Json(acct_vms);

            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }

        [HttpGet("api/invoices&q={q}")]
        public JsonResult Get(string q = "")
        {
            if (string.IsNullOrWhiteSpace(q)) return Json(new List<InvoiceViewModel>());

            List<Invoice> ords;
            Company company = new Company() { CompanyKey = 1 };
            try
            {
                IInvoiceService proxy = _service_fact.CreateClient<IInvoiceService>();

                using (proxy)
                {
                    var invoices = proxy.FindInvoicesByCompanyAsync(company, q);
                    ords = invoices.Result;
                }

                List<InvoiceViewModel> acct_vms = new List<InvoiceViewModel>();

                foreach (var ord in ords)
                {
                    InvoiceViewModel invoice_vm = _entity_service.Map(ord);

                    foreach (var item in ord.InvoiceItems)
                        invoice_vm.InvoiceItems.Add(_entity_service.Map(item));

                    acct_vms.Add(invoice_vm);
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
