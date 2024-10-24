namespace BackEndAje.Api.Domain.Entities
{
    public class User : AuditableEntity
    {
        public int UserId { get; set; }
        
        public int? CediId { get; set; }
        
        public Cedi? Cedi { get; set; }
        
        public int? ZoneId { get; set; }
        
        public Zone? Zone { get; set; }
        
        public int? Route { get; set; }
        
        public int? Code { get; set; }
        
        public int PositionId { get; set; }
        
        public Position Position { get; set; }
        
        public string? DocumentNumber { get; set; }
        
        public string PaternalSurName { get; set; }

        public string MaternalSurName { get; set; }

        public string Names { get; set; }
        public string? Email { get; set; }

        public string Phone { get; set; }
        
        public bool IsActive { get; set; }
        
        public ICollection<UserRole> UserRoles { get; set; }
        
        public User()
        {
            UserRoles = new List<UserRole>();
        }
    }
}
