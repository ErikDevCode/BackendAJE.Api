namespace BackEndAje.Api.Application.Dtos.Const
{
    public static class ConstName
    {
        public const string UserId = "UserId";
        public const string MessageErrorUserId = "Usuario ID no encontrado.";
        public const string MessageOkUpdatedResult = "Se actualizó satisfactoriamente.";
        public const string MessageOkCreatedResult = "Se creó satisfactoriamente.";
        public const string MessageOkAssignResult = "Se asignó satisfactoriamente.";
        public const string MessageOkBurkUploadResult = "Carga masiva completada satisfactoriamente.";
        public const string MessageNotSelectFileResult = "El archivo es requerido.";

        public static string GetMessageUpdateStatusById(int id) =>
            $"Activo con ID: {id} fue actualizado satisfactoriamente.";

        public static string GetMessageUpdateClientStatusById(int id) =>
            $"Cliente con ID: '{id}' fue actualizado satisfactoriamente.";

        public static string MessageUpdateStatusPositionById(int id) =>
            $"Cargo con ID: '{id}' fue actualizado correctamente.";

        public static string MessageUpdateRoleById(int id) =>
            $"Rol con ID: '{id}' fue actualizado correctamente.";

        public static string MessageUpdateUserById(int id) =>
            $"Usuario con ID: '{id}' fue actualizado correctamente.";
    }
}

