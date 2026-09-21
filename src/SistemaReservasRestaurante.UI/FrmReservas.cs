using SistemaReservasRestaurante.BLL;
using SistemaReservasRestaurante.Entidades;

namespace SistemaReservasRestaurante.UI
{
    // Capa UI del principio rector: antes de ofrecer el alta, se filtra por
    // fecha; la BLL y el procedimiento almacenado vuelven a validar el cruce
    // de horarios como defensa en profundidad.
    public partial class FrmReservas : Form
    {
        private readonly ReservaBLL _reservaBLL = new();
        private readonly MesaBLL _mesaBLL = new();
        private readonly ComensalBLL _comensalBLL = new();
        private int? _reservaIdSeleccionada;
        private List<Comensal> _comensales = new();

        public FrmReservas()
        {
            InitializeComponent();
            Load += FrmReservas_Load;
            grid.SelectionChanged += Grid_SelectionChanged;
            btnBuscar.Click += (_, _) => CargarLista();
            btnNueva.Click += (_, _) => LimpiarFormulario();
            btnGuardar.Click += BtnGuardar_Click;
            btnCancelar.Click += BtnCancelar_Click;
            btnNuevoComensal.Click += BtnNuevoComensal_Click;
        }

        private void FrmReservas_Load(object? sender, EventArgs e)
        {
            try
            {
                cboMesa.DataSource = _mesaBLL.Listar();
                cboMesa.DisplayMember = nameof(Mesa.Etiqueta);
                cboMesa.ValueMember = nameof(Mesa.MesaId);

                CargarComensales();
                dtpFecha.Value = DateTime.Today;
                dtpHoraInicio.Value = DateTime.Today.AddHours(20);
                dtpHoraFin.Value = DateTime.Today.AddHours(22);
                CargarLista();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarComensales()
        {
            _comensales = _comensalBLL.Listar();
            cboComensal.DataSource = _comensales;
            cboComensal.DisplayMember = nameof(Comensal.Nombre);
            cboComensal.ValueMember = nameof(Comensal.ComensalId);
        }

        private void CargarLista()
        {
            try
            {
                grid.DataSource = _reservaBLL.Listar(fecha: dtpFecha.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Grid_SelectionChanged(object? sender, EventArgs e)
        {
            if (grid.CurrentRow?.DataBoundItem is not Reserva resumen) return;

            var reserva = _reservaBLL.ObtenerPorId(resumen.ReservaId);
            if (reserva is null) return;

            _reservaIdSeleccionada = reserva.ReservaId;
            cboComensal.SelectedValue = reserva.ComensalId;
            cboMesa.SelectedValue = reserva.MesaId;
            dtpHoraInicio.Value = reserva.FechaHoraInicio;
            dtpHoraFin.Value = reserva.FechaHoraFin;
            numCantidadPersonas.Value = reserva.CantidadPersonas;
            lblEstado.Text = $"Estado: {reserva.Estado}";

            bool editable = reserva.Estado == EstadoReserva.CONFIRMADA;
            btnGuardar.Enabled = editable;
            btnCancelar.Enabled = editable;
        }

        private void LimpiarFormulario()
        {
            _reservaIdSeleccionada = null;
            cboComensal.SelectedIndex = _comensales.Count > 0 ? 0 : -1;
            cboMesa.SelectedIndex = -1;
            dtpHoraInicio.Value = DateTime.Today.AddHours(20);
            dtpHoraFin.Value = DateTime.Today.AddHours(22);
            numCantidadPersonas.Value = 2;
            lblEstado.Text = "Estado: (nueva)";
            btnGuardar.Enabled = true;
            btnCancelar.Enabled = false;
            grid.ClearSelection();
        }

        private void BtnNuevoComensal_Click(object? sender, EventArgs e)
        {
            using var frm = new FrmComensalNuevo();
            if (frm.ShowDialog() == DialogResult.OK && frm.ComensalRegistrado is not null)
            {
                CargarComensales();
                cboComensal.SelectedValue = frm.ComensalRegistrado.ComensalId;
            }
        }

        private void BtnGuardar_Click(object? sender, EventArgs e)
        {
            var reserva = new Reserva
            {
                ReservaId = _reservaIdSeleccionada ?? 0,
                ComensalId = cboComensal.SelectedValue is int cId ? cId : 0,
                MesaId = cboMesa.SelectedValue is int mId ? mId : 0,
                FechaHoraInicio = dtpHoraInicio.Value,
                FechaHoraFin = dtpHoraFin.Value,
                CantidadPersonas = (int)numCantidadPersonas.Value
            };

            try
            {
                if (_reservaIdSeleccionada.HasValue)
                {
                    _reservaBLL.Actualizar(reserva);
                    MessageBox.Show("Reserva actualizada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    _reservaBLL.Registrar(reserva);
                    MessageBox.Show("Reserva confirmada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                LimpiarFormulario();
                CargarLista();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "No se pudo guardar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnCancelar_Click(object? sender, EventArgs e)
        {
            if (_reservaIdSeleccionada is null) return;

            var confirmacion = MessageBox.Show("¿Cancelar esta reserva? Queda marcada como CANCELADA, nunca se elimina.",
                "Confirmar cancelación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmacion != DialogResult.Yes) return;

            try
            {
                _reservaBLL.Cancelar(_reservaIdSeleccionada.Value);
                LimpiarFormulario();
                CargarLista();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "No se pudo cancelar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
