using FlakeId;

namespace Conflict.Shared
{
    public class Message
    {
        public Id Id { get; set; }

        public string? Content { get; set; }

        public Id ChannelId { get; set; }

        public User Author { get; set; }


        public Message(string content, User author, Id id)
        {
            Content = content;
            Id = id;
            //Id = Id.Create();
            Author = author;

            //Console.WriteLine($"Created new message!\nContent: {Content}\nId: {Id}\n");
        }
    }
}
