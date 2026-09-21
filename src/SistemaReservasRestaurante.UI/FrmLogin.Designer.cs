namespace SistemaReservasRestaurante.UI
{
    partial class FrmLogin
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblTitulo;
        private Label lblUsuario;
        private ComboBox cboUsuario;
        private Button btnIngresar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            lblTitulo = new Label();
            lblUsuario = new Label();
            cboUsuario = new ComboBox();
            btnIngresar = new Button();
            SuspendLayout();

            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblTitulo.Location = new Point(30, 20);
            lblTitulo.Text = "Administración de Reservas";

            lblUsuario.AutoSize = true;
            lblUsuario.Location = new Point(30, 80);
            lblUsuario.Text = "Usuario:";

            cboUsuario.Location = new Point(120, 77);
            cboUsuario.Size = new Size(240, 23);
            cboUsuario.DropDownStyle = ComboBoxStyle.DropDownList;

            btnIngresar.Location = new Point(120, 120);
            btnIngresar.Size = new Size(120, 32);
            btnIngresar.Text = "Ingresar";
            btnIngresar.Click += BtnIngresar_Click;

            ClientSize = new Size(400, 190);
            Controls.Add(lblTitulo);
            Controls.Add(lblUsuario);
            Controls.Add(cboUsuario);
            Controls.Add(btnIngresar);
            Text = "Ingreso al sistema";
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            ResumeLayout(false);
        }
    }
}
