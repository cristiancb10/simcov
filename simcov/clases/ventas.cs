using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data;
using System.Globalization;
using static simcov.Form3;

namespace simcov.clases
{
    internal class ventas
    {
        clases.conexion_simcov objconexion = new clases.conexion_simcov();

        public void seleccionarProducto(DataGridView t, TextBox id_producto, TextBox nombre, TextBox precio)
        {
            try
            {
                id_producto.Text = t.CurrentRow.Cells[0].Value.ToString();
                nombre.Text = t.CurrentRow.Cells[1].Value.ToString();
                precio.Text = t.CurrentRow.Cells[2].Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se logro selecccionar registros. Error: " + ex.ToString());
            }
        }

        public void buscarClienteNombre(DataGridView g, TextBox nombre)
        {
            clases.conexion_simcov objconexion = new clases.conexion_simcov();
            try
            {
                g.DataSource = null;
                string consulta =
                       @"SELECT clientes.id_cliente, clientes.nombre
                        FROM clientes
                        WHERE clientes.nombre LIKE '%' + @nombre + '%'";
                
                SqlDataAdapter adaptador = new SqlDataAdapter(consulta, objconexion.obtenerConexionExistente());

                adaptador.SelectCommand.Parameters.AddWithValue("@nombre", nombre.Text ?? "");

                DataTable dt = new DataTable();
                adaptador.Fill(dt);
                g.DataSource = dt;
                objconexion.cerrarConexion();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se puede visualizar los datos.." + ex.ToString());
            }
        }

        public void buscarClienteId(DataGridView g, TextBox id_cliente)
        {
            clases.conexion_simcov objconexion = new clases.conexion_simcov();
            try
            {
                g.DataSource = null;
                string consulta =
                       @"SELECT clientes.id_cliente, clientes.nombre
                        FROM clientes
                        WHERE CAST(clientes.id_cliente AS VARCHAR) LIKE '%' + @id_cliente + '%'";

                SqlDataAdapter adaptador = new SqlDataAdapter(consulta, objconexion.obtenerConexionExistente());

                adaptador.SelectCommand.Parameters.AddWithValue("@id_cliente", id_cliente.Text ?? "");

                DataTable dt = new DataTable();
                adaptador.Fill(dt);
                g.DataSource = dt;
                objconexion.cerrarConexion();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se puede visualizar los datos.." + ex.ToString());
            }
        }

        public void buscarProductoNombre(DataGridView g, TextBox nombre)
        {
            clases.conexion_simcov objconexion = new clases.conexion_simcov();
            try
            {
                g.DataSource = null;
                string consulta =
                       @"SELECT productos.id_producto, productos.nombre, productos.precio, productos.stock
                        FROM productos
                        WHERE productos.nombre LIKE '%' + @nombre + '%'";

                SqlDataAdapter adaptador = new SqlDataAdapter(consulta, objconexion.obtenerConexionExistente());

                adaptador.SelectCommand.Parameters.AddWithValue("@nombre", nombre.Text ?? "");

                DataTable dt = new DataTable();
                adaptador.Fill(dt);
                g.DataSource = dt;
                objconexion.cerrarConexion();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se puede visualizar los datos.." + ex.ToString());
            }
        }

        public void buscarProductoId(DataGridView g, TextBox id_producto)
        {
            clases.conexion_simcov objconexion = new clases.conexion_simcov();
            try
            {
                g.DataSource = null;
                string consulta =
                       @"SELECT productos.id_producto, productos.nombre, productos.precio, productos.stock
                        FROM productos
                        WHERE CAST(productos.id_producto AS VARCHAR) LIKE '%' + @id_producto + '%'";

                SqlDataAdapter adaptador = new SqlDataAdapter(consulta, objconexion.obtenerConexionExistente());

                adaptador.SelectCommand.Parameters.AddWithValue("@id_producto", id_producto.Text ?? "");

                DataTable dt = new DataTable();
                adaptador.Fill(dt);
                g.DataSource = dt;
                objconexion.cerrarConexion();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se puede visualizar los datos.." + ex.ToString());
            }
        }

        public bool ValidarCliente(int id_cliente, string nombre)
        {
            clases.conexion_simcov objconexion = new clases.conexion_simcov();
            SqlConnection conexion = null;
            try
            {
                conexion = objconexion.obtenerConexionExistente();

                string consulta =
                    @"SELECT COUNT(*) 
                      FROM clientes 
                      WHERE clientes.id_cliente = @id AND clientes.nombre = @nombre";

                SqlCommand cmd = new SqlCommand(consulta, conexion);
                cmd.Parameters.AddWithValue("@id", id_cliente);
                cmd.Parameters.AddWithValue("@nombre", nombre);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error validando cliente: " + ex.Message);
                return false;
            }
            finally
            {
                if (conexion != null && conexion.State == System.Data.ConnectionState.Open)
                {
                    objconexion.cerrarConexion();
                }
            }
        }


        public void seleccionarCliente(DataGridView t, TextBox id_cliente, TextBox nombre)
        {
            try
            {
                nombre.Text = t.CurrentRow.Cells[0].Value.ToString();
                id_cliente.Text = t.CurrentRow.Cells[1].Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se logro selecccionar registros. Error: " + ex.ToString());
            }
        }

        public int obtenerStock(string nombreProducto)
        {
            clases.conexion_simcov objconexion = new clases.conexion_simcov();
            SqlConnection conexion = objconexion.obtenerConexionExistente();

            SqlCommand cmd = new SqlCommand("SELECT stock FROM productos WHERE nombre = '" + nombreProducto + "'", conexion);
            int total = Convert.ToInt32(cmd.ExecuteScalar());

            objconexion.cerrarConexion();
            return total;
        }

        public bool RegistrarVentaCompleta(int? idCliente, List<DetalleVentaTemp> detalles, decimal total, int idUsuario)
        {
            SqlConnection conexion = null;
            SqlTransaction transaccion = null;

            try
            {
                //Con beginTransaction para realizar mas de una consulta
                conexion = objconexion.obtenerConexionExistente();
                transaccion = conexion.BeginTransaction();

                // 1. Registrar la venta principal
                string query = "INSERT INTO Ventas (id_cliente, total, id_usuario) VALUES (@cliente, @total, @usuario); " +
                                "SELECT SCOPE_IDENTITY();";

                SqlCommand cmd = new SqlCommand(query, conexion, transaccion);
                cmd.Parameters.AddWithValue("@cliente", (object)idCliente ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@total", total);
                cmd.Parameters.AddWithValue("@usuario", idUsuario);

                int idVenta = Convert.ToInt32(cmd.ExecuteScalar());

                // 2. Registrar cada detalle de venta
                foreach (var detalle in detalles)
                {   
                    // Obtener id_producto
                    string queryProducto = "SELECT id_producto FROM Productos WHERE nombre = @nombre";
                    SqlCommand cmdProducto = new SqlCommand(queryProducto, conexion, transaccion);
                    cmdProducto.Parameters.AddWithValue("@nombre", detalle.NombreProducto);
                    int idProducto = Convert.ToInt32(cmdProducto.ExecuteScalar());

                    // Verificar stock
                    string queryStock = "SELECT stock FROM Productos WHERE id_producto = @idProducto";
                    SqlCommand cmdStock = new SqlCommand(queryStock, conexion, transaccion);
                    cmdStock.Parameters.AddWithValue("@idProducto", idProducto);
                    int stock = Convert.ToInt32(cmdStock.ExecuteScalar());

                    if (detalle.Cantidad > stock)
                    {
                        throw new Exception($"Stock insuficiente para {detalle.NombreProducto}. Disponible: {stock}");
                    }

                    // Registrar en DetalleVenta
                    string queryDetalle = @"INSERT INTO DetalleVenta 
                                 (id_venta, id_producto, cantidad, precio_unitario, costo_historico, subtotal) 
                                 VALUES 
                                 (@idVenta, @idProducto, @cantidad, @precioUnitario, @costoHistorico, @subtotal)";

                    SqlCommand cmdDetalle = new SqlCommand(queryDetalle, conexion, transaccion);
                    cmdDetalle.Parameters.AddWithValue("@idVenta", idVenta);
                    cmdDetalle.Parameters.AddWithValue("@idProducto", idProducto);
                    cmdDetalle.Parameters.AddWithValue("@cantidad", detalle.Cantidad);
                    cmdDetalle.Parameters.AddWithValue("@precioUnitario", detalle.PrecioUnitario);
                    cmdDetalle.Parameters.AddWithValue("@costoHistorico", detalle.CostoUnitario);
                    cmdDetalle.Parameters.AddWithValue("@subtotal", detalle.Subtotal);
                    cmdDetalle.ExecuteNonQuery();

                    // Actualizar stock
                    string updateStock = "UPDATE Productos SET stock = stock - @cantidad WHERE id_producto = @idProducto";
                    SqlCommand cmdUpdateStock = new SqlCommand(updateStock, conexion, transaccion);
                    cmdUpdateStock.Parameters.AddWithValue("@cantidad", detalle.Cantidad);
                    cmdUpdateStock.Parameters.AddWithValue("@idProducto", idProducto);
                    cmdUpdateStock.ExecuteNonQuery();
                }

                transaccion.Commit();
                return true;
            }
            catch (Exception ex)
            {
                transaccion?.Rollback();
                MessageBox.Show($"Error al registrar venta: {ex.Message}");
                return false;
            }
        }

        public decimal obtenerCosto(string nombreProducto)
        {
            using (SqlConnection conexion = objconexion.establecerConexion())
            {
                string query = "SELECT costo FROM Productos WHERE nombre = @nombre";
                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@nombre", nombreProducto);

                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToDecimal(result) : 0;
            }
        }

        public bool AnularVenta(int idVenta)
        {
            SqlConnection conexion = objconexion.obtenerConexionExistente();
            SqlTransaction transaccion = null;

            try
            {
                transaccion = conexion.BeginTransaction();

                // Verificar si ya está anulada
                string checkQuery = "SELECT anulada FROM Ventas WHERE id_venta = @id";
                SqlCommand checkCmd = new SqlCommand(checkQuery, conexion, transaccion);
                checkCmd.Parameters.AddWithValue("@id", idVenta);
                bool yaAnulada = Convert.ToBoolean(checkCmd.ExecuteScalar());

                if (yaAnulada)
                {
                    MessageBox.Show("La venta ya fue anulada.");
                    transaccion.Rollback();
                    return false;
                }

                // 1. Recuperar los productos vendidos
                string detalleQuery = @"SELECT id_producto, cantidad 
                                        FROM DetalleVenta 
                                        WHERE id_venta = @id";
                SqlCommand detalleCmd = new SqlCommand(detalleQuery, conexion, transaccion);
                detalleCmd.Parameters.AddWithValue("@id", idVenta);
                SqlDataReader reader = detalleCmd.ExecuteReader();

                List<(int idProducto, int cantidad)> productos = new List<(int, int)>();

                while (reader.Read())
                {
                    productos.Add((reader.GetInt32(0), reader.GetInt32(1)));
                }
                reader.Close();

                // 2. Restaurar stock
                foreach (var item in productos)
                {
                    string updateStock = "UPDATE Productos SET stock = stock + @cantidad WHERE id_producto = @idProducto";
                    SqlCommand cmd = new SqlCommand(updateStock, conexion, transaccion);
                    cmd.Parameters.AddWithValue("@cantidad", item.cantidad);
                    cmd.Parameters.AddWithValue("@idProducto", item.idProducto);
                    cmd.ExecuteNonQuery();
                }

                // 3. Marcar la venta como anulada
                string anularVenta = "UPDATE Ventas SET anulada = 1 WHERE id_venta = @id";
                SqlCommand cmdAnular = new SqlCommand(anularVenta, conexion, transaccion);
                cmdAnular.Parameters.AddWithValue("@id", idVenta);
                cmdAnular.ExecuteNonQuery();

                transaccion.Commit();
                return true;
            }
            catch (Exception ex)
            {
                transaccion?.Rollback();
                MessageBox.Show("Error al anular venta: " + ex.Message);
                return false;
            }
        }
    }
}
