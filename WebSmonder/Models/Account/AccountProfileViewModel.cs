using System.ComponentModel.DataAnnotations;

namespace WebSmonder.Models.Account
{
    public class AccountProfileViewModel
    {
        [Display(Name = "Логін")]
        [StringLength(50, ErrorMessage = "Логін має містити не більше 50 символів")]
        public string UserName { get; set; } = string.Empty;

        [Display(Name = "Ім'я")]
        [StringLength(50, ErrorMessage = "Ім'я має містити не більше 50 символів")]
        public string FirstName { get; set; } = string.Empty;

        [Display(Name = "Прізвище")]
        [StringLength(50, ErrorMessage = "Прізвище має містити не більше 50 символів")]
        public string LastName { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Невірний формат електронної пошти")]
        [Display(Name = "Електронна пошта")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Невірний формат номера телефону")]
        [Display(Name = "Номер телефону")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Display(Name = "Фото профілю")]
        public IFormFile Image { get; set; } = null!;
        public string? ViewImage { get; set; } = string.Empty;
    }
}
