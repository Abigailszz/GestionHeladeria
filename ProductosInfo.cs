using System;
using System.Data;
using MySql.Data.MySqlClient; // Asegúrate de instalar el paquete MySql.Data para usar MySQL.

namespace MiAplicacion
{
    public class ProductosInfo
    {
        private readonly string connectionString = "Server=localhost;Database=gestion_heladeria;Uid=root;Pwd=nueva_contraseña;";

        public DataTable ObtenerProductos()
        {
            string consulta = @"
                SELECT 
                    p.id AS producto_id, 
                    p.nombre AS producto_nombre, 
                    p.cantidad, 
                    p.fecha_vencimiento, 
                    p.cantidad_minima, 
                    p.descripcion, 
                    m.nombre AS marca, 
                    t.nombre AS tipo
                FROM 
                    productos p
                LEFT JOIN 
                    marcas m ON p.marca_id = m.id
                LEFT JOIN 
                    tipos t ON p.tipo_id = t.id";

            try
            {
                using (MySqlConnection conexion = new MySqlConnection(connectionString))
                {
                    conexion.Open();
                    using (MySqlCommand comando = new MySqlCommand(consulta, conexion))
                    {
                        using (MySqlDataAdapter adaptador = new MySqlDataAdapter(comando))
                        {
                            DataTable tabla = new DataTable();
                            adaptador.Fill(tabla);
                            return tabla;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los productos: " + ex.Message);
            }
        }
        public DataRow ObtenerProductoPorID(int idProducto)
        {
            string consulta = "SELECT * FROM productos WHERE id = @id";
            try
            {
                using (MySqlConnection conexion = new MySqlConnection(connectionString))
                {
                    conexion.Open();
                    using (MySqlCommand comando = new MySqlCommand(consulta, conexion))
                    {
                        comando.Parameters.AddWithValue("@id", idProducto);

                        using (MySqlDataAdapter adaptador = new MySqlDataAdapter(comando))
                        {
                            DataTable tabla = new DataTable();
                            adaptador.Fill(tabla);

                            return tabla.Rows.Count > 0 ? tabla.Rows[0] : null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el producto: " + ex.Message);
            }
        }

        public void ActualizarProducto(int idProducto, string descripcion, int cantidad)
        {
            string consulta = "UPDATE productos SET descripcion = @descripcion, cantidad = @cantidad WHERE id = @id";
            try
            {
                using (MySqlConnection conexion = new MySqlConnection(connectionString))
                {
                    conexion.Open();
                    using (MySqlCommand comando = new MySqlCommand(consulta, conexion))
                    {
                        comando.Parameters.AddWithValue("@id", idProducto);
                        comando.Parameters.AddWithValue("@descripcion", descripcion);
                        comando.Parameters.AddWithValue("@cantidad", cantidad);
                        comando.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el producto: " + ex.Message);
            }
        }

    }
}
