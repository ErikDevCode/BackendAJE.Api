﻿namespace BackEndAje.Api.Domain.Entities
{
    public class RolePermission
    {
        public int RolePermissionId { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public int PermissionId { get; set; }

        public bool Status { get; set; }
        public Permission Permission { get; set; }
    }
}
