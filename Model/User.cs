
namespace Dotnetrest.Model
{
    public class UserModel
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Password { get; set; }
        public string? ExternalLoginName { get; set; }
    }
}