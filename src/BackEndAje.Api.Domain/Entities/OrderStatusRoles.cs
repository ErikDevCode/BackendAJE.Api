namespace BackEndAje.Api.Domain.Entities
{
    public class OrderStatusRoles
    {
        public int OrderStatusRolesId { get; set; }
        public int OrderStatusId { get; set; }
        public int RoleId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public Role Role { get; set; }
    }
}