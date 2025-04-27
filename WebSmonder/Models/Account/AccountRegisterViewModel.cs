using System.ComponentModel.DataAnnotations;

namespace WebSmonder.Models.Account
{
    public class AccountRegisterViewModel
    {
        [Required(ErrorMessage = "Поле логін обов'язкове")]
        [Display(Name = "Логін")]
        [StringLength(50, ErrorMessage = "Логін має містити не більше 50 символів")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Поле ім'я обов'язкове")]
        [Display(Name = "Ім'я")]
        [StringLength(50, ErrorMessage = "Ім'я має містити не більше 50 символів")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Поле прізвище обов'язкове")]
        [Display(Name = "Прізвище")]
        [StringLength(50, ErrorMessage = "Прізвище має містити не більше 50 символів")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Поле email обов'язкове")]
        [EmailAddress(ErrorMessage = "Невірний формат електронної пошти")]
        [Display(Name = "Електронна пошта")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Поле пароль обов'язкове")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль має бути не менше 6 символів")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Поле номер телефону обов'язкове")]
        [Phone(ErrorMessage = "Невірний формат номера телефону")]
        [Display(Name = "Номер телефону")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Display(Name = "Фото профілю")]
        public IFormFile Image { get; set; } = null!;
    }

}
