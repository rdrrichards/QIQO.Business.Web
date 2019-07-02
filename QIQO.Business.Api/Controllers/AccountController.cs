using Microsoft.AspNetCore.Mvc;
using QIQO.Business.Client.Contracts;
using QIQO.Business.Client.Entities;
using QIQO.Business.Core;
using QIQO.Business.Identity;
using QIQO.Business.Services;
using QIQO.Business.ViewModels.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QIQO.Business.Api.Controllers
{
    //[Route("api/accounts")]
    public class AccountController : Controller
    {
        private readonly IServiceFactory _serviceFactory;
        private readonly IEntityService _entityService;

        public QIQOUserManager _userManager { get; }

        public AccountController(IServiceFactory serviceFactory, IEntityService entityService, QIQOUserManager userManager)
        {
            _serviceFactory = serviceFactory;
            _entityService = entityService;
            _userManager = userManager;
        }

        // GET: api/values
        [HttpGet("api/accounts")]
        // [Authorize]
        public async Task<IActionResult> Get()
        {
            //var usr = await _userManager.FindByNameAsync(this.User.Identity.Name);
            //if (usr == null) return BadRequest();
            try
            {
                Company company = new Company() { CompanyKey = 2 };
                List<Account> accts;

                using (IAccountService proxy = _serviceFactory.CreateClient<IAccountService>())
                {
                    accts = await proxy.GetAccountsByCompanyAsync(company);
                }

                List<AccountViewModel> acct_vms = new List<AccountViewModel>();
                foreach (Account acct in accts)
                {
                    AccountViewModel acct_vm = _entityService.Map(acct);

                    foreach (EntityAttribute att in acct.AccountAttributes)
                        acct_vm.Attributes.Add(_entityService.Map(att));

                    foreach (Address addr in acct.Addresses)
                        acct_vm.Addresses.Add(_entityService.Map(addr));

                    foreach (AccountPerson emp in acct.Employees)
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
        public async Task<IActionResult> Get(string q = "")
        {
            if (q == "") return Json(new List<AccountViewModel>());
            try
            {
                Company company = new Company() { CompanyKey = 1 };
                List<Account> accts;

                using (IAccountService proxy = _serviceFactory.CreateClient<IAccountService>())
                {
                    accts = await proxy.FindAccountByCompanyAsync(company, q);
                }

                List<AccountViewModel> acct_vms = new List<AccountViewModel>();

                foreach (Account acct in accts)
                {
                    AccountViewModel acct_vm = _entityService.Map(acct);

                    foreach (EntityAttribute att in acct.AccountAttributes)
                        acct_vm.Attributes.Add(_entityService.Map(att));

                    foreach (Address addr in acct.Addresses)
                        acct_vm.Addresses.Add(_entityService.Map(addr));

                    foreach (AccountPerson emp in acct.Employees)
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
        public async Task<IActionResult> Get(int account_key)
        {
            try
            {
                Account acct;

                using (IAccountService proxy = _serviceFactory.CreateClient<IAccountService>())
                {
                    acct = await proxy.GetAccountByIDAsync(account_key, true);
                }

                AccountViewModel acct_vm = _entityService.Map(acct);

                foreach (EntityAttribute att in acct.AccountAttributes)
                    acct_vm.Attributes.Add(_entityService.Map(att));

                foreach (Address addr in acct.Addresses)
                    acct_vm.Addresses.Add(_entityService.Map(addr));

                foreach (AccountPerson emp in acct.Employees)
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
        public async Task<IActionResult> Post([FromBody] AccountViewModel account)
        {
            try
            {
                using (IAccountService proxy = _serviceFactory.CreateClient<IAccountService>())
                {
                    int return_val = await proxy.CreateAccountAsync(_entityService.Map(account));
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
        public async Task<IActionResult> Put([FromBody] AccountViewModel account)
        {
            return await Post(account);
        }

        // DELETE api/values/5
        [HttpDelete("api/accounts/{account_key}")]
        public async Task<IActionResult> Delete(int account_key)
        {
            try
            {
                Account account = new Account() { AccountKey = account_key };
                using (IAccountService proxy = _serviceFactory.CreateClient<IAccountService>())
                {
                    bool return_val = await proxy.DeleteAccountAsync(account);
                    return Json(return_val);
                }
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }
        [HttpGet("api/accounts/recent")]
        public async Task<IActionResult> GetRecent()
        {
            try
            {
                Company company = new Company() { CompanyKey = 1 };
                List<Account> accts;

                using (IAccountService proxy = _serviceFactory.CreateClient<IAccountService>())
                {
                    accts = await proxy.GetAccountsByCompanyAsync(company);
                }

                List<AccountViewModel> acct_vms = new List<AccountViewModel>();

                List<Account> recent_accts = accts.Where(a => a.UpdateDateTime >= DateTime.Now.AddDays(-90)).ToList();

                foreach (Account acct in recent_accts)
                {
                    AccountViewModel acct_vm = _entityService.Map(acct);

                    foreach (EntityAttribute att in acct.AccountAttributes)
                        acct_vm.Attributes.Add(_entityService.Map(att));

                    foreach (Address addr in acct.Addresses)
                        acct_vm.Addresses.Add(_entityService.Map(addr));

                    foreach (AccountPerson emp in acct.Employees)
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
