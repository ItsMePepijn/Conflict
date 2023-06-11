

namespace Conflict.Shared.Dto
{
    public class CreateChannelDto
    {
        public string Name { get; set;} = string.Empty;
        public long OwnerId { get; set;}
    }
}
