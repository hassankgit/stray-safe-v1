namespace SquirrelSite.Models
{
    public class User
    {
        // test feature-hk-0001
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public int? NumberOfSubmissions { get; set; }
    }
}
