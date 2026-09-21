using SistemaReservasRestaurante.BLL;
using SistemaReservasRestaurante.Entidades;

namespace SistemaReservasRestaurante.UI
{
    // El sistema no muestra opciones que el usuario no puede ejecutar:
    // Administración reúne todas las capacidades de Encargado, más el
    // gobierno del catálogo de mesas.
    public partial class FrmPrincipal : Form
    {
        public FrmPrincipal()
        {
            InitializeComponent();
            Load += FrmPrincipal_Load;
        }

        private void FrmPrincipal_Load(object? sender, EventArgs e)
        {
            var usuario = SesionActual.Usuario!;
            lblSesion.Text = $"Sesión: {usuario.NombreUsuario} ({usuario.Rol})";

            bool esAdministracion = usuario.Rol == RolUsuario.ADMINISTRACION;
            btnMesas.Visible = esAdministracion;
        }

        private void BtnReservas_Click(object? sender, EventArgs e) => new FrmReservas().ShowDialog();
        private void BtnMesas_Click(object? sender, EventArgs e) => new FrmMesas().ShowDialog();

        private void BtnCerrarSesion_Click(object? sender, EventArgs e)
        {
            SesionActual.CerrarSesion();
            Close();
        }
    }
}
