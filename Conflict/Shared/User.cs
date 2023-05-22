
using FlakeId;

namespace Conflict.Shared
{
    public class User
    {
        public Id Id { get; set; }

        public string Name { get; set; }

        public User(string name, Id id)
        {
            Name = name;
            Id = id;
            //Id = Id.Create();
        }
    }
}
