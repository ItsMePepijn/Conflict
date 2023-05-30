
namespace Conflict.Shared.Models
{
    public class Message
    {
        public long Id { get; set; } = 0;

        public string Content { get; set; } = string.Empty;

        public long ChannelId { get; set; } = 0;

		public User Author { get; set; } = new();

    }
}
