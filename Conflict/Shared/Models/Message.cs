

using System.ComponentModel.DataAnnotations.Schema;

namespace Conflict.Shared.Models
{
    public class Message
    {
        public long Id { get; set; }

        public string Content { get; set; }

        public long ChannelId { get; set; }

        public User Author { get; set; }

        public long AuthorId { get; set; }

    }
}
