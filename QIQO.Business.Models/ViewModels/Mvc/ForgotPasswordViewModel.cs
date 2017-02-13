using System.ComponentModel.DataAnnotations;

namespace QIQO.Business.ViewModels.Mvc
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
