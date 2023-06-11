
namespace Conflict.Shared.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; } = string.Empty;

    }
}
