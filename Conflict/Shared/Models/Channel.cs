
namespace Conflict.Shared.Models
{
    public class Channel
    {

        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public long OwnerId { get; set; }
    }
}
