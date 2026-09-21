using SistemaReservasRestaurante.BLL;
using SistemaReservasRestaurante.Entidades;

namespace SistemaReservasRestaurante.UI
{
    public partial class FrmMesas : Form
    {
        private readonly MesaBLL _bll = new();
        private readonly SectorBLL _sectorBLL = new();
        private int? _mesaIdSeleccionada;

        public FrmMesas()
        {
            InitializeComponent();
            Load += FrmMesas_Load;
            grid.SelectionChanged += Grid_SelectionChanged;
            btnNueva.Click += (_, _) => LimpiarFormulario();
            btnGuardar.Click += BtnGuardar_Click;
            btnAnular.Click += BtnAnular_Click;
        }

        private void FrmMesas_Load(object? sender, EventArgs e)
        {
            try
            {
                cboSector.DataSource = _sectorBLL.Listar();
                cboSector.DisplayMember = nameof(Sector.Nombre);
                cboSector.ValueMember = nameof(Sector.SectorId);
                CargarLista();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarLista()
        {
            try
            {
                grid.DataSource = _bll.Listar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Grid_SelectionChanged(object? sender, EventArgs e)
        {
            if (grid.CurrentRow?.DataBoundItem is not Mesa m) return;

            _mesaIdSeleccionada = m.MesaId;
            txtNumero.Text = m.Numero.ToString();
            txtCapacidad.Text = m.Capacidad.ToString();
            cboSector.SelectedValue = m.SectorId;
            txtNumero.Enabled = false;
        }

        private void LimpiarFormulario()
        {
            _mesaIdSeleccionada = null;
            txtNumero.Text = string.Empty;
            txtCapacidad.Text = string.Empty;
            cboSector.SelectedIndex = -1;
            txtNumero.Enabled = true;
            grid.ClearSelection();
        }

        private void BtnGuardar_Click(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtNumero.Text, out int numero) || !int.TryParse(txtCapacidad.Text, out int capacidad))
            {
                MessageBox.Show("Número y capacidad deben ser valores numéricos.", "Datos inválidos",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var mesa = new Mesa
            {
                MesaId = _mesaIdSeleccionada ?? 0,
                Numero = numero,
                Capacidad = capacidad,
                SectorId = cboSector.SelectedValue is int id ? id : 0
            };

            try
            {
                if (_mesaIdSeleccionada.HasValue)
                {
                    _bll.Actualizar(mesa);
                    MessageBox.Show("Mesa actualizada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    _bll.Registrar(mesa);
                    MessageBox.Show("Mesa registrada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                LimpiarFormulario();
                CargarLista();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "No se pudo guardar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnAnular_Click(object? sender, EventArgs e)
        {
            if (_mesaIdSeleccionada is null)
            {
                MessageBox.Show("Seleccione una mesa de la lista.", "Datos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmacion = MessageBox.Show("¿Anular esta mesa?", "Confirmar anulación",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmacion != DialogResult.Yes) return;

            try
            {
                _bll.Anular(_mesaIdSeleccionada.Value);
                LimpiarFormulario();
                CargarLista();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "No se pudo anular", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
