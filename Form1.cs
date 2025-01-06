using System;
using System.Windows.Forms;


namespace MiAplicacion
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Width = 1200; // Ancho de la ventana
            this.Height = 600; // Alto de la ventana

            ConfigurarInterfaz();
        }

        private void ConfigurarInterfaz()
        {
            // Crear el panel izquierdo (menú)
            Panel panelIzquierdo = new Panel
            {
                Dock = DockStyle.Left,
                Width = 200,
                BackColor = System.Drawing.Color.LightGray
            };
            this.Controls.Add(panelIzquierdo); // Agregar primero el menú

            // Crear el panel derecho (contenido dinámico)
            Panel panelDerecho = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = System.Drawing.Color.White
            };
            this.Controls.Add(panelDerecho); // Agregar después el contenido

            // Asegurarse de que el panel izquierdo esté por debajo en la jerarquía
            this.Controls.SetChildIndex(panelDerecho, 0);

            // Crear el botón "Agregar Productos"
            Button btnAgregarProductos = new Button
            {
                Text = "Agregar Productos",
                Dock = DockStyle.Top,
                Height = 50
            };
            btnAgregarProductos.Click += (s, e) =>
            {
                // Limpiar el panel derecho y mostrar el formulario
                panelDerecho.Controls.Clear();
                var formularioAgregar = new FormularioAgregarProducto
                {
                    Dock = DockStyle.Fill // Asegurar que ocupe todo el espacio del panel
                };
                panelDerecho.Controls.Add(formularioAgregar);
            };
            panelIzquierdo.Controls.Add(btnAgregarProductos);

            // Crear el botón "Productos"
            Button btnProductos = new Button
            {
                Text = "Productos",
                Dock = DockStyle.Top,
                Height = 50
            };
            btnProductos.Click += (s, e) =>
            {
                // Mostrar "Chau" en el panel derecho
                panelDerecho.Controls.Clear();
                Label lblChau = new Label
                {
                    Text = "Chau",
                    Dock = DockStyle.Fill,
                    Font = new System.Drawing.Font("Arial", 20),
                    TextAlign = System.Drawing.ContentAlignment.MiddleCenter
                };
                panelDerecho.Controls.Add(lblChau);
            };
            panelIzquierdo.Controls.Add(btnProductos);

            // Crear el encabezado
            Label encabezado = new Label
            {
                Text = "Gestión",
                Dock = DockStyle.Top,
                Font = new System.Drawing.Font("Arial", 16, System.Drawing.FontStyle.Bold),
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Height = 50
            };
            panelIzquierdo.Controls.Add(encabezado);
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            // Este código se ejecutará cuando el formulario se cargue
            MessageBox.Show("Formulario cargado correctamente");
        }
    }
}
