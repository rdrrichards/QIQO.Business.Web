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
        private readonly IServiceFactory _service_fact;
        private IEntityService _entity_service;

        public AccountController(IServiceFactory services, IEntityService entity_service)
        {
            _service_fact = services;
            _entity_service = entity_service;
        }

        // GET: api/values
        [HttpGet("api/accounts")]
        //[Authorize]
        public JsonResult Get()
        {
            try
            {
                IAccountService proxy = _service_fact.CreateClient<IAccountService>();
                Company company = new Company() { CompanyKey = 1 };
                List<Account> accts;

                using (proxy)
                {
                    var accounts = proxy.GetAccountsByCompanyAsync(company);
                    accts = accounts.Result;
                }

                List<AccountViewModel> acct_vms = new List<AccountViewModel>();

                foreach (var acct in accts)
                {
                    AccountViewModel acct_vm = _entity_service.Map(acct);
                    
                    foreach (var att in acct.AccountAttributes)
                        acct_vm.Attributes.Add(_entity_service.Map(att));

                    foreach (var addr in acct.Addresses)
                        acct_vm.Addresses.Add(_entity_service.Map(addr));

                    foreach (var emp in acct.Employees)
                        acct_vm.Employees.Add(_entity_service.Map(emp));

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
        public JsonResult Get(string q = "")
        {
            if (q == "") return Json(new List<AccountViewModel>());
            try
            {
                IAccountService proxy = _service_fact.CreateClient<IAccountService>();
                Company company = new Company() { CompanyKey = 1 };
                List<Account> accts;

                using (proxy)
                {
                    var accounts = proxy.FindAccountByCompanyAsync(company, q);
                    accts = accounts.Result;
                }

                List<AccountViewModel> acct_vms = new List<AccountViewModel>();

                foreach (var acct in accts)
                {
                    AccountViewModel acct_vm = _entity_service.Map(acct);

                    foreach (var att in acct.AccountAttributes)
                        acct_vm.Attributes.Add(_entity_service.Map(att));

                    foreach (var addr in acct.Addresses)
                        acct_vm.Addresses.Add(_entity_service.Map(addr));

                    foreach (var emp in acct.Employees)
                        acct_vm.Employees.Add(_entity_service.Map(emp));

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
        public JsonResult Get(int account_key)
        {
            try
            {
                IAccountService proxy = _service_fact.CreateClient<IAccountService>();
                Account acct;

                using (proxy)
                {
                    var account = proxy.GetAccountByIDAsync(account_key, true);
                    acct = account.Result;
                    //return Json(account.Result);
                }

                AccountViewModel acct_vm = _entity_service.Map(acct);

                foreach (var att in acct.AccountAttributes)
                    acct_vm.Attributes.Add(_entity_service.Map(att));

                foreach (var addr in acct.Addresses)
                    acct_vm.Addresses.Add(_entity_service.Map(addr));

                foreach (var emp in acct.Employees)
                    acct_vm.Employees.Add(_entity_service.Map(emp));

                return Json(acct_vm);
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }

        // POST api/values
        [HttpPost("api/accounts")]
        public JsonResult Post([FromBody] AccountViewModel account)
        {
            try
            {
                IAccountService proxy = _service_fact.CreateClient<IAccountService>();

                using (proxy)
                {
                    Task<int> return_val = proxy.CreateAccountAsync(_entity_service.Map(account));
                    return Json(return_val.Result);
                }
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }

        // PUT api/values/5
        [HttpPut("api/accounts")]
        public JsonResult Put([FromBody] AccountViewModel account)
        {
            return Post(account);
        }

        // DELETE api/values/5
        [HttpDelete("api/accounts/{account_key}")]
        public JsonResult Delete(int account_key)
        {
            try
            {
                Account account = new Account() { AccountKey = account_key };
                IAccountService proxy = _service_fact.CreateClient<IAccountService>();

                using (proxy)
                {
                    Task<bool> return_val = proxy.DeleteAccountAsync(account);
                    return Json(return_val.Result);
                }
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }
        [HttpGet("api/accounts/recent")]
        public JsonResult GetRecent()
        {
            try
            {
                IAccountService proxy = _service_fact.CreateClient<IAccountService>();
                Company company = new Company() { CompanyKey = 1 };
                List<Account> accts;

                using (proxy)
                {
                    var accounts = proxy.GetAccountsByCompanyAsync(company);
                    accts = accounts.Result;
                }

                List<AccountViewModel> acct_vms = new List<AccountViewModel>();

                var recent_accts = accts.Where(a => a.UpdateDateTime >= DateTime.Now.AddDays(-90)).ToList();

                foreach (var acct in recent_accts)
                {
                    AccountViewModel acct_vm = _entity_service.Map(acct);

                    foreach (var att in acct.AccountAttributes)
                        acct_vm.Attributes.Add(_entity_service.Map(att));

                    foreach (var addr in acct.Addresses)
                        acct_vm.Addresses.Add(_entity_service.Map(addr));

                    foreach (var emp in acct.Employees)
                        acct_vm.Employees.Add(_entity_service.Map(emp));

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
