namespace SistemaReservasRestaurante.UI
{
    partial class FrmComensalNuevo
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblNombre, lblTelefono, lblEmail;
        private TextBox txtNombre, txtTelefono, txtEmail;
        private Button btnGuardar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            lblNombre = new Label(); lblTelefono = new Label(); lblEmail = new Label();
            txtNombre = new TextBox(); txtTelefono = new TextBox(); txtEmail = new TextBox();
            btnGuardar = new Button();
            SuspendLayout();

            lblNombre.Location = new Point(20, 20); lblNombre.Text = "Nombre:"; lblNombre.AutoSize = true;
            txtNombre.Location = new Point(120, 17); txtNombre.Size = new Size(220, 23);

            lblTelefono.Location = new Point(20, 55); lblTelefono.Text = "Teléfono:"; lblTelefono.AutoSize = true;
            txtTelefono.Location = new Point(120, 52); txtTelefono.Size = new Size(220, 23);

            lblEmail.Location = new Point(20, 90); lblEmail.Text = "Email:"; lblEmail.AutoSize = true;
            txtEmail.Location = new Point(120, 87); txtEmail.Size = new Size(220, 23);

            btnGuardar.Location = new Point(120, 130); btnGuardar.Size = new Size(100, 30); btnGuardar.Text = "Guardar";

            ClientSize = new Size(370, 180);
            Controls.Add(lblNombre); Controls.Add(txtNombre);
            Controls.Add(lblTelefono); Controls.Add(txtTelefono);
            Controls.Add(lblEmail); Controls.Add(txtEmail);
            Controls.Add(btnGuardar);
            Text = "Nuevo comensal";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            ResumeLayout(false);
        }
    }
}
