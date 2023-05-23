
namespace Conflict.Shared.Models
{
    public class Message
    {
        public long Id { get; set; } = FlakeId.Id.Create();

        public string? Content { get; set; }

        public long ChannelId { get; set; }

        public User Author { get; set; }

    }
}
