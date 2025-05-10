using System.ComponentModel.DataAnnotations;

namespace WebSmonder.Models.Account
{
    public class ResetPasswordViewModel
    {
        public string Email { get; set; } = string.Empty;
        [Display(Name = "Новий пароль")]
        [Required(ErrorMessage = "Поле новий пароль обов'язкове")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Підтвердження паролю")]
        [Required(ErrorMessage = "Поле підтвердження паролю обов'язкове")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Паролі не співпадають")]
        public string ConfirmPassword { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
