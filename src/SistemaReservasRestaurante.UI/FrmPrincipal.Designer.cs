namespace SistemaReservasRestaurante.UI
{
    partial class FrmPrincipal
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblSesion;
        private Button btnReservas;
        private Button btnMesas;
        private Button btnCerrarSesion;

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
            lblSesion = new Label();
            btnReservas = new Button();
            btnMesas = new Button();
            btnCerrarSesion = new Button();
            SuspendLayout();

            lblSesion.AutoSize = true;
            lblSesion.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblSesion.Location = new Point(20, 20);

            btnReservas.Location = new Point(20, 60);
            btnReservas.Size = new Size(240, 40);
            btnReservas.Text = "Reservas (alta / consulta / cancelación)";
            btnReservas.Click += BtnReservas_Click;

            btnMesas.Location = new Point(20, 110);
            btnMesas.Size = new Size(240, 40);
            btnMesas.Text = "Mantenedor de Mesas";
            btnMesas.Click += BtnMesas_Click;

            btnCerrarSesion.Location = new Point(20, 170);
            btnCerrarSesion.Size = new Size(240, 32);
            btnCerrarSesion.Text = "Cerrar sesión";
            btnCerrarSesion.Click += BtnCerrarSesion_Click;

            ClientSize = new Size(300, 230);
            Controls.Add(lblSesion);
            Controls.Add(btnReservas);
            Controls.Add(btnMesas);
            Controls.Add(btnCerrarSesion);
            Text = "Administración de Reservas";
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            ResumeLayout(false);
        }
    }
}
