namespace WebAPI.Model
{
    public class Login
    {//client will enter username and password, we will validate them and return a JWT token if valid
        public required string Username { get; set; }  // needed for login
        public required string PasswordHash { get; set; }  // needed for login
    }
}
