

namespace Conflict.Shared.Dto
{
    public class MessageDto
    {
        public long Id { get; set; }

        public string Content { get; set; } = string.Empty;

        public long ChannelId { get; set; }

        public UserDto Author { get; set; } = new();
    }
}
