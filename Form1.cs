using System;
using System.Windows.Forms;

namespace MiAplicacion
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Width = 1200;
            this.Height = 600;

            Panel panelDerecho = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = System.Drawing.Color.White
            };
            this.Controls.Add(panelDerecho);

            Interfaz.ConfigurarInterfaz(this, panelDerecho);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            MessageBox.Show("Formulario cargado correctamente");
        }
    }
}
