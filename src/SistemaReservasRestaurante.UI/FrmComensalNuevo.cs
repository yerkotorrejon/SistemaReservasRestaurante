using SistemaReservasRestaurante.BLL;
using SistemaReservasRestaurante.Entidades;

namespace SistemaReservasRestaurante.UI
{
    public partial class FrmComensalNuevo : Form
    {
        private readonly ComensalBLL _bll = new();
        public Comensal? ComensalRegistrado { get; private set; }

        public FrmComensalNuevo()
        {
            InitializeComponent();
            btnGuardar.Click += BtnGuardar_Click;
        }

        private void BtnGuardar_Click(object? sender, EventArgs e)
        {
            var comensal = new Comensal
            {
                Nombre = txtNombre.Text.Trim(),
                Telefono = txtTelefono.Text.Trim(),
                Email = txtEmail.Text.Trim()
            };

            try
            {
                comensal.ComensalId = _bll.Registrar(comensal);
                ComensalRegistrado = comensal;
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "No se pudo registrar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
