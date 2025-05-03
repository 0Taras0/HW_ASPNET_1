namespace WebSmonder.Areas.Admin.Models.Users
{
    public class UserItemViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new();
    }
}
