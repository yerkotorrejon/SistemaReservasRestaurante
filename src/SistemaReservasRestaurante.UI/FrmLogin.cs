using SistemaReservasRestaurante.BLL;
using SistemaReservasRestaurante.Entidades;

namespace SistemaReservasRestaurante.UI
{
    // Selector de rol simplificado: sin contraseña, el usuario elige con
    // quién entra y el sistema condiciona cada acción según ese rol.
    public partial class FrmLogin : Form
    {
        private readonly UsuarioBLL _usuarioBLL = new();
        private List<Usuario> _usuarios = new();

        public FrmLogin()
        {
            InitializeComponent();
            Load += FrmLogin_Load;
        }

        private void FrmLogin_Load(object? sender, EventArgs e)
        {
            try
            {
                _usuarios = _usuarioBLL.Listar();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No fue posible conectar con la base de datos:\n{ex.Message}",
                    "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            cboUsuario.DataSource = _usuarios;
            cboUsuario.DisplayMember = nameof(Usuario.NombreUsuario);
        }

        private void BtnIngresar_Click(object? sender, EventArgs e)
        {
            if (cboUsuario.SelectedItem is not Usuario usuario)
            {
                MessageBox.Show("Seleccione un usuario para continuar.", "Datos incompletos",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SesionActual.IniciarSesion(usuario);

            var principal = new FrmPrincipal();
            Hide();
            principal.FormClosed += (_, _) => Close();
            principal.Show();
        }
    }
}
