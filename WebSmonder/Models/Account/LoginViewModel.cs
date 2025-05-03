using System.ComponentModel.DataAnnotations;

namespace WebSmonder.Models.Account
{
    public class LoginViewModel
    {
        [Display(Name = "Електронна пошта")]
        [Required(ErrorMessage = "Поле електронна пошта обов'язкове для заповнення")]
        [EmailAddress(ErrorMessage = "Поле електронна пошта повинно містити коректну електронну адресу")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Пароль")]
        [Required(ErrorMessage = "Поле пароль обов'язкове для заповнення")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
