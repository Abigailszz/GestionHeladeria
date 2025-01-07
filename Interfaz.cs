using System;
 using System.Data;
        using System.Windows.Forms;

namespace MiAplicacion
{
    public static class Interfaz
    {
        public static void ConfigurarInterfaz(Form form, Panel panelDerecho)
        {
            // Crear el panel izquierdo (menú)
            Panel panelIzquierdo = new Panel
            {
                Dock = DockStyle.Left,
                Width = 200,
                BackColor = System.Drawing.Color.LightGray
            };
            form.Controls.Add(panelIzquierdo);

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

            // Crear el botón "Agregar Productos"
            Button btnAgregarProductos = new Button
            {
                Text = "Agregar Productos",
                Dock = DockStyle.Top,
                Height = 50
            };
            btnAgregarProductos.Click += (s, e) =>
            {
                panelDerecho.Controls.Clear();
                var formularioAgregar = new FormularioAgregarProducto
                {
                    Dock = DockStyle.Fill
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
                panelDerecho.Controls.Clear(); // Limpia el panel derecho
                AgregarBuscador(panelDerecho); // Agrega el buscador
                MostrarTablaProductos(panelDerecho); // Muestra la tabla de productos
            };
            panelIzquierdo.Controls.Add(btnProductos);
        }

        public static void MostrarTablaProductos(Panel panelDerecho)
        {
            DataGridView tablaProductos = new DataGridView
            {
                Top = 100,
                Height = panelDerecho.Height - 100,
                Width = panelDerecho.Width,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            try
            {
                ProductosInfo productoData = new ProductosInfo();
                DataTable datosProductos = productoData.ObtenerProductos();
                tablaProductos.DataSource = datosProductos;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener los datos: " + ex.Message);
            }

            panelDerecho.Controls.Add(tablaProductos);
        }

        public static void AgregarBuscador(Panel panelDerecho)
        {
            Panel panelBuscador = new Panel
            {
                Top = 50,
                Left = 50,
                Height = 40,
                Width = panelDerecho.Width,
                BackColor = System.Drawing.Color.LightGray
            };
            panelDerecho.Controls.Add(panelBuscador);

            TextBox txtBuscarID = new TextBox
            {
                Width = 200,
                Margin = new Padding(10)
            };
            panelBuscador.Controls.Add(txtBuscarID);

            Button btnBuscar = new Button
            {
                Text = "Buscar",
                Width = 80,
                Margin = new Padding(10),
                Left = txtBuscarID.Right + 10
            };
            btnBuscar.Click += (s, e) =>
            {
                if (int.TryParse(txtBuscarID.Text, out int idProducto))
                {
                    MostrarFormularioEdicion(idProducto, panelDerecho);
                }
                else
                {
                    MessageBox.Show("Por favor, ingrese un ID válido.");
                }
            };
            panelBuscador.Controls.Add(btnBuscar);
        }

        public static void MostrarFormularioEdicion(int idProducto, Panel panelDerecho)
        {
           
                ProductosInfo productoInfo = new ProductosInfo();
                DataRow producto = productoInfo.ObtenerProductoPorID(idProducto);

                if (producto == null)
                {
                    MessageBox.Show("Producto no encontrado.");
                    return;
                }
                else
                {
                    MessageBox.Show("Producto encontrado: " + producto["descripcion"].ToString());
                }

                // Crear un formulario para editar
                Form formularioEdicion = new Form
                {
                    Text = "Editar Producto",
                    Width = 400,
                    Height = 300
                };

                Label lblDescripcion = new Label
                {
                    Text = "Descripción:",
                    Top = 20,
                    Left = 20
                };
                formularioEdicion.Controls.Add(lblDescripcion);

                TextBox txtDescripcion = new TextBox
                {
                    Text = producto["descripcion"].ToString(),
                    Top = 50,
                    Left = 20,
                    Width = 300
                };
                formularioEdicion.Controls.Add(txtDescripcion);

                Label lblCantidad = new Label
                {
                    Text = "Cantidad:",
                    Top = 100,
                    Left = 20
                };
                formularioEdicion.Controls.Add(lblCantidad);

                NumericUpDown nudCantidad = new NumericUpDown
                {
                    Value = Convert.ToDecimal(producto["cantidad"]),
                    Top = 130,
                    Left = 20,
                    Width = 100
                };
                formularioEdicion.Controls.Add(nudCantidad);

                Button btnGuardar = new Button
                {
                    Text = "Guardar",
                    Top = 200,
                    Left = 20
                };
                btnGuardar.Click += (s, e) =>
                {
                    string nuevaDescripcion = txtDescripcion.Text;
                    int nuevaCantidad = (int)nudCantidad.Value;

                    productoInfo.ActualizarProducto(idProducto, nuevaDescripcion, nuevaCantidad);
                    MessageBox.Show("Producto actualizado correctamente.");

                    // Recargar la tabla de productos
                    panelDerecho.Controls.Clear(); // Limpia el panel derecho
                    AgregarBuscador(panelDerecho); // Asegura que el buscador esté presente
                    MostrarTablaProductos(panelDerecho); // Muestra la tabla con los datos actualizados

                    formularioEdicion.Close();
                };
                formularioEdicion.Controls.Add(btnGuardar);

                formularioEdicion.ShowDialog();

            }
       
    }


	}

