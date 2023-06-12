

namespace Conflict.Shared.Models
{
    public class Message
    {
        public long Id { get; set; }

        public string Content { get; set; } = string.Empty;

        public long ChannelId { get; set; }

        public User? Author { get; set; }

        public long AuthorId { get; set; }

    }
}
