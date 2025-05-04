using System.ComponentModel.DataAnnotations;

namespace WebSmonder.Areas.Admin.Models.Users
{
    public class UserItemEditModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ім’я користувача є обов’язковим")]
        [StringLength(50, ErrorMessage = "Максимум 50 символів")]
        [Display(Name = "Ім’я користувача")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ім’я є обов’язковим")]
        [StringLength(50, ErrorMessage = "Максимум 50 символів")]
        [Display(Name = "Ім’я")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Прізвище є обов’язковим")]
        [StringLength(50, ErrorMessage = "Максимум 50 символів")]
        [Display(Name = "Прізвище")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email є обов’язковим")]
        [EmailAddress(ErrorMessage = "Невірний формат email")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Невірний номер телефону")]
        [Display(Name = "Телефон")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Display(Name = "Фото профілю")]
        public IFormFile Image { get; set; } = null!;
        public string? ViewImage { get; set; } = string.Empty;

        [Required(ErrorMessage = "Оберіть хоча б одну роль")]
        [MinLength(1, ErrorMessage = "Оберіть хоча б одну роль")]
        [Display(Name = "Ролі")]
        public List<string> Roles { get; set; } = new();
    }

}
