namespace SistemaReservasRestaurante.UI
{
    partial class FrmMesas
    {
        private System.ComponentModel.IContainer components = null;
        private DataGridView grid;
        private Label lblNumero, lblCapacidad, lblSector;
        private TextBox txtNumero, txtCapacidad;
        private ComboBox cboSector;
        private Button btnNueva, btnGuardar, btnAnular;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            grid = new DataGridView();
            lblNumero = new Label(); lblCapacidad = new Label(); lblSector = new Label();
            txtNumero = new TextBox(); txtCapacidad = new TextBox();
            cboSector = new ComboBox();
            btnNueva = new Button(); btnGuardar = new Button(); btnAnular = new Button();
            SuspendLayout();

            grid.Location = new Point(20, 20);
            grid.Size = new Size(560, 200);
            grid.ReadOnly = true;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            lblNumero.Location = new Point(20, 240); lblNumero.Text = "Número:"; lblNumero.AutoSize = true;
            txtNumero.Location = new Point(150, 237); txtNumero.Size = new Size(100, 23);

            lblCapacidad.Location = new Point(20, 270); lblCapacidad.Text = "Capacidad:"; lblCapacidad.AutoSize = true;
            txtCapacidad.Location = new Point(150, 267); txtCapacidad.Size = new Size(100, 23);

            lblSector.Location = new Point(20, 300); lblSector.Text = "Sector:"; lblSector.AutoSize = true;
            cboSector.Location = new Point(150, 297); cboSector.Size = new Size(200, 23);
            cboSector.DropDownStyle = ComboBoxStyle.DropDownList;

            btnNueva.Location = new Point(20, 340); btnNueva.Size = new Size(100, 32); btnNueva.Text = "Nueva";
            btnGuardar.Location = new Point(130, 340); btnGuardar.Size = new Size(100, 32); btnGuardar.Text = "Guardar";
            btnAnular.Location = new Point(240, 340); btnAnular.Size = new Size(100, 32); btnAnular.Text = "Anular";

            ClientSize = new Size(600, 400);
            Controls.Add(grid);
            Controls.Add(lblNumero); Controls.Add(txtNumero);
            Controls.Add(lblCapacidad); Controls.Add(txtCapacidad);
            Controls.Add(lblSector); Controls.Add(cboSector);
            Controls.Add(btnNueva); Controls.Add(btnGuardar); Controls.Add(btnAnular);
            Text = "Mantenedor de Mesas";
            StartPosition = FormStartPosition.CenterParent;
            ResumeLayout(false);
        }
    }
}
