namespace Conflict.Shared.DbModels
{
    public class DbUser
    {
        public long Id { get; set; } = FlakeId.Id.Create();

        public string Name { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

    }
}
