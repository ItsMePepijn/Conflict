using FlakeId;

namespace Conflict.Shared
{
    public class Message
    {
        public Id Id { get; set; }

        public string? Content { get; set; }

        public Id ChannelId { get; set; }


        public Message(string content, Id id)
        {
            Content = content;
            Id = id;
            //Id = Id.Create();

            //Console.WriteLine($"Created new message!\nContent: {Content}\nId: {Id}\n");
        }
    }
}
