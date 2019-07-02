using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QIQO.Business.Client.Entities;
using QIQO.Business.Identity;
using QIQO.Business.ViewModels.Api;
using System.Threading.Tasks;

namespace QIQO.Web.Api.Controllers
{
    //[Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly QIQOUserManager _userManager;
        private readonly SignInManager<User> _signinManager;
        private readonly QIQORoleManager _roleManager;

        public AuthController(QIQOUserManager userManager, SignInManager<User> signinManager, QIQORoleManager roleManager)
        {
            _userManager = userManager;
            _signinManager = signinManager;
            _roleManager = roleManager;
        }

        [HttpGet("api/auth/test")]
        public IActionResult Get() => Json("Works!");

        [HttpPost("api/auth/authenticate")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            Microsoft.AspNetCore.Identity.SignInResult result = await _signinManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                return Ok(); // Json(new { Succeeded = true, Message = "Authentication succeeded" });
            }
            else
            {
                return BadRequest(model);
            }
        }

        [HttpPost("api/auth/logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _signinManager.SignOutAsync();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("api/auth/register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User() { Email = model.UserName, UserName = model.UserName };
                IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    IdentityResult r_result = await _userManager.AddToRoleAsync(user, "Users");
                    await _signinManager.SignInAsync(user, true);
                    return Json(new { Succeeded = true, Message = "Registration succeeded" });
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return Json(new { Succeeded = false, Message = "Registration failed", ModelState = ModelState });
                }
            }
            return Json(new { Succeeded = false, Message = "Invalid fields in model", ModelState = ModelState });
        }
    }
}
