
namespace Conflict.Shared.Models
{
    public class Channel
    {
        public Channel(long Id, string Name)
        {
            this.Id = Id;
            this.Name = Name;
        }

        public long Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
    }
}
