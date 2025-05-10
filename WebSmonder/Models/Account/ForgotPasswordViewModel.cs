using System.ComponentModel.DataAnnotations;

namespace WebSmonder.Models.Account
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Поле email обов'язкове")]
        [EmailAddress(ErrorMessage = "Невірний формат електронної пошти")]
        [Display(Name = "Електронна пошта")]
        public string Email { get; set; } = string.Empty;
    }
}
