using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace MiAplicacion
{
    public class FormularioAgregarProducto : UserControl
    {
        public FormularioAgregarProducto()
        {
            // Configuración del formulario
            this.Dock = DockStyle.Fill;
            ConfigurarFormulario();
        }

        private void ConfigurarFormulario()
        {
            // Título del formulario
            var lblTitulo = new Label
            {
                Text = "Agregar Producto",
                Font = new System.Drawing.Font("Arial", 16, System.Drawing.FontStyle.Bold),
                Top = 10,
                Left = 20,
                Width = 300
            };

            // Crear campos de entrada
            var lblNombre = CrearLabel("Nombre:", 50);
            var txtNombre = CrearTextBox(50);

            var lblCantidad = CrearLabel("Cantidad:", 90);
            var txtCantidad = CrearTextBox(90);

            var lblMarca = CrearLabel("Marca:", 130);
            var txtMarca = CrearTextBox(130);

            var lblTipo = CrearLabel("Tipo:", 170);
            var txtTipo = CrearTextBox(170);

            var lblCantMin = CrearLabel("Cantidad Mínima:", 210, 120);
            var txtCantMin = CrearTextBox(210);

            var lblFechaVencimiento = CrearLabel("Fecha de Vencimiento:", 250, 150);
            var dtpFechaVencimiento = new DateTimePicker
            {
                Top = 250,
                Left = 180,
                Width = 200
            };

            var lblDescripcion = CrearLabel("Descripción (Opcional):", 290, 150);
            var txtDescripcion = new TextBox
            {
                Top = 290,
                Left = 180,
                Width = 200,
                Height = 60,
                Multiline = true
            };

            var btnGuardar = new Button
            {
                Text = "Guardar",
                Top = 370,
                Left = 130,
                Width = 100
            };
            btnGuardar.Click += (s, e) =>
            {
                GuardarProducto(
                    txtNombre.Text,
                    txtCantidad.Text,
                    txtMarca.Text,
                    txtTipo.Text,
                    txtCantMin.Text,
                    dtpFechaVencimiento.Value,
                    txtDescripcion.Text
                );
            };

            // Agregar controles al formulario
            this.Controls.Add(lblTitulo);
            this.Controls.Add(lblNombre);
            this.Controls.Add(txtNombre);
            this.Controls.Add(lblCantidad);
            this.Controls.Add(txtCantidad);
            this.Controls.Add(lblMarca);
            this.Controls.Add(txtMarca);
            this.Controls.Add(lblTipo);
            this.Controls.Add(txtTipo);
            this.Controls.Add(lblCantMin);
            this.Controls.Add(txtCantMin);
            this.Controls.Add(lblFechaVencimiento);
            this.Controls.Add(dtpFechaVencimiento);
            this.Controls.Add(lblDescripcion);
            this.Controls.Add(txtDescripcion);
            this.Controls.Add(btnGuardar);
        }

        private Label CrearLabel(string texto, int top, int width = 100)
        {
            return new Label
            {
                Text = texto,
                Top = top,
                Left = 20,
                Width = width
            };
        }

        private TextBox CrearTextBox(int top)
        {
            return new TextBox
            {
                Top = top,
                Left = 130,
                Width = 200
            };
        }

        private void GuardarProducto(string nombre, string cantidad, string marca, string tipo, string cantMin, DateTime fechaVencimiento, string descripcion)
        {
            string connectionString = "Server=localhost;Database=gestion_heladeria;Uid=root;Pwd=nueva_contraseña;"; // Cambia por tu conexión
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Verificar o insertar la marca
                    string queryMarca = "INSERT INTO marcas (nombre) SELECT @marca WHERE NOT EXISTS (SELECT 1 FROM marcas WHERE nombre = @marca)";
                    using (var cmdMarca = new MySqlCommand(queryMarca, connection))
                    {
                        cmdMarca.Parameters.AddWithValue("@marca", marca);
                        cmdMarca.ExecuteNonQuery();
                    }

                    // Obtener el ID de la marca
                    int marcaId = ObtenerId(connection, "marcas", marca);

                    // Verificar o insertar el tipo
                    string queryTipo = "INSERT INTO tipos (nombre) SELECT @tipo WHERE NOT EXISTS (SELECT 1 FROM tipos WHERE nombre = @tipo)";
                    using (var cmdTipo = new MySqlCommand(queryTipo, connection))
                    {
                        cmdTipo.Parameters.AddWithValue("@tipo", tipo);
                        cmdTipo.ExecuteNonQuery();
                    }

                    // Obtener el ID del tipo
                    int tipoId = ObtenerId(connection, "tipos", tipo);

                    // Insertar el producto
                    string queryProducto = @"
                        INSERT INTO productos (nombre, cantidad, marca_id, tipo_id, cantidad_minima, fecha_vencimiento, descripcion)
                        VALUES (@nombre, @cantidad, @marcaId, @tipoId, @cantMin, @fechaVencimiento, @descripcion)";
                    using (var cmdProducto = new MySqlCommand(queryProducto, connection))
                    {
                        cmdProducto.Parameters.AddWithValue("@nombre", nombre);
                        cmdProducto.Parameters.AddWithValue("@cantidad", cantidad);
                        cmdProducto.Parameters.AddWithValue("@marcaId", marcaId);
                        cmdProducto.Parameters.AddWithValue("@tipoId", tipoId);
                        cmdProducto.Parameters.AddWithValue("@cantMin", cantMin);
                        cmdProducto.Parameters.AddWithValue("@fechaVencimiento", fechaVencimiento);
                        cmdProducto.Parameters.AddWithValue("@descripcion", string.IsNullOrEmpty(descripcion) ? (object)DBNull.Value : descripcion);

                        cmdProducto.ExecuteNonQuery();
                    }

                    MessageBox.Show("Producto agregado correctamente.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private int ObtenerId(MySqlConnection connection, string tabla, string valor)
        {
            string query = $"SELECT id FROM {tabla} WHERE nombre = @valor";
            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@valor", valor);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
    }
}
