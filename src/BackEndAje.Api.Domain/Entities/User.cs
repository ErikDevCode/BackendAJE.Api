namespace BackEndAje.Api.Domain.Entities
{
    public class User
    {
        public int UserId { get; set; }
        
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public DateTime CreatedAt { get; set; }
        
        public DateTime UpdatedAt { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        
        public User()
        {
            UserRoles = new List<UserRole>();
        }
        
        public void SetPassword(string password)
        {
            this.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
        }
        
        public bool ValidatePassword(string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, this.PasswordHash);
        }
    }
}
