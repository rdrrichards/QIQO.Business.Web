using System.ComponentModel.DataAnnotations;

namespace QIQO.Business.ViewModels.Mvc
{
    public class LoginViewModel
    {
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }

        public string ReturnURL { get; set; }
    }
}
