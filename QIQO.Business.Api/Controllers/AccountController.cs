using System;
using System.Collections.Generic;
using QIQO.Business.Client.Contracts;
using QIQO.Business.Client.Entities;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using QIQO.Business.Core;
using QIQO.Business.Services;
using QIQO.Business.ViewModels.Api;

namespace QIQO.Business.Api.Controllers
{
    //[Route("api/accounts")]
    public class AccountController : Controller
    {
        private readonly IServiceFactory _serviceFactory;
        private readonly IEntityService _entityService;

        public AccountController(IServiceFactory serviceFactory, IEntityService entityService)
        {
            _serviceFactory = serviceFactory;
            _entityService = entityService;
        }

        // GET: api/values
        [HttpGet("api/accounts")]
        //[Authorize]
        public async Task<JsonResult> Get()
        {
            try
            {
                var company = new Company() { CompanyKey = 1 };
                List<Account> accts;

                using (var proxy = _serviceFactory.CreateClient<IAccountService>())
                {
                    accts = await proxy.GetAccountsByCompanyAsync(company);
                }

                var acct_vms = new List<AccountViewModel>();
                foreach (var acct in accts)
                {
                    var acct_vm = _entityService.Map(acct);
                    
                    foreach (var att in acct.AccountAttributes)
                        acct_vm.Attributes.Add(_entityService.Map(att));

                    foreach (var addr in acct.Addresses)
                        acct_vm.Addresses.Add(_entityService.Map(addr));

                    foreach (var emp in acct.Employees)
                        acct_vm.Employees.Add(_entityService.Map(emp));

                    acct_vms.Add(acct_vm);
                }
                
                return Json(acct_vms);
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }

        [HttpGet("api/accounts&q={q}")]
        public async Task<JsonResult> Get(string q = "")
        {
            if (q == "") return Json(new List<AccountViewModel>());
            try
            {
                var company = new Company() { CompanyKey = 1 };
                List<Account> accts;

                using (var proxy = _serviceFactory.CreateClient<IAccountService>())
                {
                    accts = await proxy.FindAccountByCompanyAsync(company, q);
                }

                var acct_vms = new List<AccountViewModel>();

                foreach (var acct in accts)
                {
                    AccountViewModel acct_vm = _entityService.Map(acct);

                    foreach (var att in acct.AccountAttributes)
                        acct_vm.Attributes.Add(_entityService.Map(att));

                    foreach (var addr in acct.Addresses)
                        acct_vm.Addresses.Add(_entityService.Map(addr));

                    foreach (var emp in acct.Employees)
                        acct_vm.Employees.Add(_entityService.Map(emp));

                    acct_vms.Add(acct_vm);
                }

                return Json(acct_vms);
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }

        // GET api/values/5
        [HttpGet("api/accounts/{account_key}")]
        public async Task<JsonResult> Get(int account_key)
        {
            try
            {
                Account acct;

                using (var proxy = _serviceFactory.CreateClient<IAccountService>())
                {
                    acct = await proxy.GetAccountByIDAsync(account_key, true);
                }

                var acct_vm = _entityService.Map(acct);

                foreach (var att in acct.AccountAttributes)
                    acct_vm.Attributes.Add(_entityService.Map(att));

                foreach (var addr in acct.Addresses)
                    acct_vm.Addresses.Add(_entityService.Map(addr));

                foreach (var emp in acct.Employees)
                    acct_vm.Employees.Add(_entityService.Map(emp));

                return Json(acct_vm);
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }

        // POST api/values
        [HttpPost("api/accounts")]
        public async Task<JsonResult> Post([FromBody] AccountViewModel account)
        {
            try
            {
                using (var proxy = _serviceFactory.CreateClient<IAccountService>())
                {
                    var return_val = await proxy.CreateAccountAsync(_entityService.Map(account));
                    return Json(return_val);
                }
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }

        // PUT api/values/5
        [HttpPut("api/accounts")]
        public async Task<JsonResult> Put([FromBody] AccountViewModel account)
        {
            return await Post(account);
        }

        // DELETE api/values/5
        [HttpDelete("api/accounts/{account_key}")]
        public async Task<JsonResult> Delete(int account_key)
        {
            try
            {
                var account = new Account() { AccountKey = account_key };
                using (var proxy = _serviceFactory.CreateClient<IAccountService>())
                {
                    var return_val = await proxy.DeleteAccountAsync(account);
                    return Json(return_val);
                }
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }
        [HttpGet("api/accounts/recent")]
        public async Task<JsonResult> GetRecent()
        {
            try
            {
                var company = new Company() { CompanyKey = 1 };
                List<Account> accts;

                using (var proxy = _serviceFactory.CreateClient<IAccountService>())
                {
                    accts = await proxy.GetAccountsByCompanyAsync(company);
                }

                var acct_vms = new List<AccountViewModel>();

                var recent_accts = accts.Where(a => a.UpdateDateTime >= DateTime.Now.AddDays(-90)).ToList();

                foreach (var acct in recent_accts)
                {
                    var acct_vm = _entityService.Map(acct);

                    foreach (var att in acct.AccountAttributes)
                        acct_vm.Attributes.Add(_entityService.Map(att));

                    foreach (var addr in acct.Addresses)
                        acct_vm.Addresses.Add(_entityService.Map(addr));

                    foreach (var emp in acct.Employees)
                        acct_vm.Employees.Add(_entityService.Map(emp));

                    acct_vms.Add(acct_vm);
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
