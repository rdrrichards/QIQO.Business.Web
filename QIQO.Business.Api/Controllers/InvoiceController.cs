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
        private readonly IServiceFactory _serviceFactory;
        private readonly IEntityService _entityService;

        public InvoiceController(IServiceFactory serviceFactory, IEntityService entityService)
        {
            _serviceFactory = serviceFactory;
            _entityService = entityService;
        }

        [HttpGet("api/invoices/{invoice_key}")]
        public async Task<IActionResult> Get(int invoice_key)
        {
            try
            {
                using (var proxy = _serviceFactory.CreateClient<IInvoiceService>())
                {
                    var invoice = await proxy.GetInvoiceAsync(invoice_key);
                    var invoice_vm = _entityService.Map(invoice);

                    foreach (var item in invoice.InvoiceItems)
                        invoice_vm.InvoiceItems.Add(_entityService.Map(item));

                    return Json(invoice_vm);
                }
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }

        [HttpPost("api/invoices")]
        public async Task<IActionResult> Post([FromBody] InvoiceViewModel invoice)
        {
            try
            {
                using (var proxy = _serviceFactory.CreateClient<IInvoiceService>())
                {
                    var invoice_key = await proxy.CreateInvoiceAsync(_entityService.Map(invoice)); // Save invoice
                    var new_invoice = await proxy.GetInvoiceAsync(invoice_key); // Get the new invoice and send it back to the client
                    return Json(new_invoice);
                }
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }

        [HttpPut("api/invoices")]
        public async Task<IActionResult> Put([FromBody] InvoiceViewModel invoice)
        {
            return await Post(invoice);
        }

        [HttpDelete("api/invoices/{invoice_key}")]
        public async Task<IActionResult> Delete(int invoice_key)
        {
            try
            {
                var invoice = new Invoice() { InvoiceKey = invoice_key };
                using (var proxy = _serviceFactory.CreateClient<IInvoiceService>())
                {
                    var invoice_res = await proxy.DeleteInvoiceAsync(invoice); // delete invoice
                    return Json(invoice_res);
                }
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }

        [HttpGet("api/accounts/{account_key}/invoices")]
        public async Task<IActionResult> GetAccountInvoices(int account_key)
        {
            List<Invoice> invs;
            var account = new Account() { AccountKey = account_key };
            try
            {
                using (var proxy = _serviceFactory.CreateClient<IInvoiceService>())
                {
                    //Task<List<Invoice>> invoice = proxy.GetInvoicesByAccountAsync(_entity_service.MapAccountViewModelToAccount(account));
                    invs = await proxy.GetInvoicesByAccountAsync(account);
                }

                var acct_vms = new List<InvoiceViewModel>();

                foreach (var inv in invs)
                {
                    var invoice_vm = _entityService.Map(inv);

                    foreach (var item in inv.InvoiceItems)
                        invoice_vm.InvoiceItems.Add(_entityService.Map(item));

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
        public async Task<IActionResult> GetAccountInvoice(int account_key, int invoice_key)
        {
            Invoice inv;
            //var account = new Account() { AccountKey = account_key };
            try
            {
                using (var proxy = _serviceFactory.CreateClient<IInvoiceService>())
                {
                    inv = await proxy.GetInvoiceAsync(invoice_key);
                }

                if (inv.Account.AccountKey != account_key)
                    throw new InvalidOperationException("Invoice -> account access violation");

                var invoice_vm = _entityService.Map(inv);

                foreach (var item in inv.InvoiceItems)
                    invoice_vm.InvoiceItems.Add(_entityService.Map(item));

                return Json(invoice_vm);

            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }

        [HttpGet("api/openinvoices")]
        public async Task<IActionResult> Get()
        {
            List<Invoice> invs;
            var company = new Company() { CompanyKey = 1 };
            try
            {
                using (var proxy = _serviceFactory.CreateClient<IInvoiceService>())
                {
                    invs = await proxy.GetInvoicesByCompanyAsync(company);
                }

                var acct_vms = new List<InvoiceViewModel>();

                foreach (var inv in invs)
                {
                    var invoice_vm = _entityService.Map(inv);

                    foreach (var item in inv.InvoiceItems)
                        invoice_vm.InvoiceItems.Add(_entityService.Map(item));

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
        public async Task<IActionResult> Get(string q = "")
        {
            if (string.IsNullOrWhiteSpace(q)) return Json(new List<InvoiceViewModel>());

            List<Invoice> invs;
            var company = new Company() { CompanyKey = 1 };
            try
            {
                using (var proxy = _serviceFactory.CreateClient<IInvoiceService>())
                {
                    invs = await proxy.FindInvoicesByCompanyAsync(company, q);
                }

                var acct_vms = new List<InvoiceViewModel>();

                foreach (var inv in invs)
                {
                    var invoice_vm = _entityService.Map(inv);

                    foreach (var item in inv.InvoiceItems)
                        invoice_vm.InvoiceItems.Add(_entityService.Map(item));

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
