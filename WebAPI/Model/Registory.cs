namespace WebAPI.Model
{
    public class Registory
    {
        public required string Username { get; set; }

        public required string PasswordHash { get; set; }

        public required string Role { get; set; }
    }
}
