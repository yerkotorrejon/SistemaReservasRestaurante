using SistemaReservasRestaurante.Entidades;

namespace SistemaReservasRestaurante.BLL
{
    // Selector de rol simplificado (sin login con contraseña): el usuario elige
    // con quién entra y el sistema condiciona cada acción según ese rol.
    // ADMINISTRACION reúne todas las capacidades de ENCARGADO, más el
    // gobierno del catálogo de mesas/sectores y de los usuarios.
    public static class SesionActual
    {
        public static Usuario? Usuario { get; private set; }

        public static void IniciarSesion(Usuario usuario)
        {
            Usuario = usuario;
        }

        public static void CerrarSesion()
        {
            Usuario = null;
        }

        public static bool HaySesion => Usuario != null;

        public static void RequerirSesion()
        {
            if (Usuario == null)
                throw new BllException("No hay una sesión activa. Debe seleccionar un usuario para continuar.");
        }

        // Solo ADMINISTRACION puede mantener el catálogo (mesas, sectores, usuarios).
        public static void RequerirAdministracion()
        {
            RequerirSesion();
            if (Usuario!.Rol != RolUsuario.ADMINISTRACION)
                throw new BllException("Acción no permitida: solo Administración puede mantener el catálogo.");
        }
    }
}
