namespace SistemaReservasRestaurante.UI
{
    partial class FrmReservas
    {
        private System.ComponentModel.IContainer components = null;
        private DataGridView grid;
        private Label lblFecha, lblComensal, lblMesa, lblHoraInicio, lblHoraFin, lblCantidad, lblEstado;
        private DateTimePicker dtpFecha, dtpHoraInicio, dtpHoraFin;
        private Button btnBuscar, btnNuevoComensal, btnNueva, btnGuardar, btnCancelar;
        private ComboBox cboComensal, cboMesa;
        private NumericUpDown numCantidadPersonas;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            grid = new DataGridView();
            lblFecha = new Label(); lblComensal = new Label(); lblMesa = new Label();
            lblHoraInicio = new Label(); lblHoraFin = new Label(); lblCantidad = new Label(); lblEstado = new Label();
            dtpFecha = new DateTimePicker(); dtpHoraInicio = new DateTimePicker(); dtpHoraFin = new DateTimePicker();
            btnBuscar = new Button(); btnNuevoComensal = new Button();
            btnNueva = new Button(); btnGuardar = new Button(); btnCancelar = new Button();
            cboComensal = new ComboBox(); cboMesa = new ComboBox();
            numCantidadPersonas = new NumericUpDown();
            SuspendLayout();

            lblFecha.Location = new Point(20, 20); lblFecha.Text = "Fecha:"; lblFecha.AutoSize = true;
            dtpFecha.Location = new Point(90, 17); dtpFecha.Size = new Size(150, 23);
            dtpFecha.Format = DateTimePickerFormat.Short;

            btnBuscar.Location = new Point(250, 16); btnBuscar.Size = new Size(90, 25); btnBuscar.Text = "Buscar";

            grid.Location = new Point(20, 55);
            grid.Size = new Size(680, 180);
            grid.ReadOnly = true;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            lblComensal.Location = new Point(20, 250); lblComensal.Text = "Comensal:"; lblComensal.AutoSize = true;
            cboComensal.Location = new Point(120, 247); cboComensal.Size = new Size(220, 23);
            cboComensal.DropDownStyle = ComboBoxStyle.DropDownList;

            btnNuevoComensal.Location = new Point(350, 246); btnNuevoComensal.Size = new Size(120, 25);
            btnNuevoComensal.Text = "Nuevo comensal";

            lblMesa.Location = new Point(20, 285); lblMesa.Text = "Mesa:"; lblMesa.AutoSize = true;
            cboMesa.Location = new Point(120, 282); cboMesa.Size = new Size(220, 23);
            cboMesa.DropDownStyle = ComboBoxStyle.DropDownList;

            lblEstado.Location = new Point(480, 285); lblEstado.Text = "Estado: (nueva)"; lblEstado.AutoSize = true;
            lblEstado.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            lblHoraInicio.Location = new Point(20, 320); lblHoraInicio.Text = "Inicio:"; lblHoraInicio.AutoSize = true;
            dtpHoraInicio.Location = new Point(120, 317); dtpHoraInicio.Size = new Size(180, 23);
            dtpHoraInicio.Format = DateTimePickerFormat.Custom;
            dtpHoraInicio.CustomFormat = "dd/MM/yyyy HH:mm";

            lblHoraFin.Location = new Point(320, 320); lblHoraFin.Text = "Término:"; lblHoraFin.AutoSize = true;
            dtpHoraFin.Location = new Point(390, 317); dtpHoraFin.Size = new Size(180, 23);
            dtpHoraFin.Format = DateTimePickerFormat.Custom;
            dtpHoraFin.CustomFormat = "dd/MM/yyyy HH:mm";

            lblCantidad.Location = new Point(20, 355); lblCantidad.Text = "Personas:"; lblCantidad.AutoSize = true;
            numCantidadPersonas.Location = new Point(120, 352); numCantidadPersonas.Size = new Size(60, 23);
            numCantidadPersonas.Minimum = 1;
            numCantidadPersonas.Maximum = 50;
            numCantidadPersonas.Value = 2;

            btnNueva.Location = new Point(20, 400); btnNueva.Size = new Size(100, 32); btnNueva.Text = "Nueva";
            btnGuardar.Location = new Point(130, 400); btnGuardar.Size = new Size(100, 32); btnGuardar.Text = "Guardar";
            btnCancelar.Location = new Point(240, 400); btnCancelar.Size = new Size(100, 32); btnCancelar.Text = "Cancelar";

            ClientSize = new Size(720, 450);
            Controls.Add(lblFecha); Controls.Add(dtpFecha); Controls.Add(btnBuscar);
            Controls.Add(grid);
            Controls.Add(lblComensal); Controls.Add(cboComensal); Controls.Add(btnNuevoComensal);
            Controls.Add(lblMesa); Controls.Add(cboMesa); Controls.Add(lblEstado);
            Controls.Add(lblHoraInicio); Controls.Add(dtpHoraInicio);
            Controls.Add(lblHoraFin); Controls.Add(dtpHoraFin);
            Controls.Add(lblCantidad); Controls.Add(numCantidadPersonas);
            Controls.Add(btnNueva); Controls.Add(btnGuardar); Controls.Add(btnCancelar);
            Text = "Reservas";
            StartPosition = FormStartPosition.CenterParent;
            ResumeLayout(false);
        }
    }
}
