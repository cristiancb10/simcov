using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data;

namespace simcov.clases
{
    internal class inventario
    {
        public bool insertar(TextBox nombre, TextBox precio, TextBox costo, TextBox stock, TextBox stock_minimo)
        {
            clases.conexion_simcov objconexion = new clases.conexion_simcov();

            if (string.IsNullOrWhiteSpace(nombre.Text) ||
                 string.IsNullOrWhiteSpace(precio.Text) ||
                 string.IsNullOrWhiteSpace(costo.Text)) 
            {
                MessageBox.Show("Los campos nombre, precio, costo son obligatorios");
                return false;
            }

            if (!EsNumeroDecimalValido(precio.Text, out decimal precioValue))
            {
                MessageBox.Show("El precio solo debe contener números positivo y/o punto decimal");
                return false;
            }

            if (!EsNumeroDecimalValido(costo.Text, out decimal costoValue))
            {
                MessageBox.Show("El costo solo debe contener números positivo y/o punto decimal");
                return false;
            }

            decimal precioDecimal = decimal.Parse(precio.Text);
            decimal costoDecimal = decimal.Parse(costo.Text);

            if (precioDecimal < costoDecimal)
            {
                MessageBox.Show("El precio de venta no puede ser menor al costo");
                return false;
            }

            if (!decimal.TryParse(precio.Text, out decimal precioValue1) || precioValue1 <= 0)
            {
                MessageBox.Show("Ingrese un precio válido (mayor que 0)");
                return false;
            }

            if (!string.IsNullOrWhiteSpace(costo.Text) && !decimal.TryParse(costo.Text, out decimal costoValue1))
            {
                MessageBox.Show("El costo debe ser un número válido (ej: 15.50) o dejarse vacío");
                return false;
            }

            if (!int.TryParse(stock.Text, out int stockValue) || stockValue < 0)
            {
                MessageBox.Show("Ingrese una cantidad de stock válida (número entero)");
                return false;
            }

            if (!string.IsNullOrWhiteSpace(stock_minimo.Text))
            {
                if (!int.TryParse(stock_minimo.Text, out int stockMinimoValue) || stockMinimoValue < 0)
                {
                    MessageBox.Show("Ingrese un stock mínimo válido (número entero positivo) o deje vacío");
                    stock_minimo.Focus();
                    return false;
                }
            }

            try
            {
                string query = "INSERT INTO productos (nombre, precio, costo, stock, stock_minimo) " + " VALUES ('" + nombre.Text + "', '" + precio.Text + "', '" + costo.Text + "', '" + stock.Text + "', '" + stock_minimo.Text + "')";

                SqlCommand micomando = new SqlCommand(query, objconexion.obtenerConexionExistente());
                SqlDataReader mireader;
                mireader = micomando.ExecuteReader();
                while (mireader.Read())
                { }
                MessageBox.Show("El dato se guardo correctamente");
                objconexion.cerrarConexion();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar. Asegúrese que:\n" +
                      "- Precio es un número mayor a 0\n" +
                      "- Costo y Stock son números válidos\n" +
                      "Error técnico: " + ex.Message);
                return false;
            }
            finally
            {
                objconexion.cerrarConexion();
            }
        }

        public bool validarProducto(string nombre)
        {
            clases.conexion_simcov objconexion = new clases.conexion_simcov();
            SqlConnection conexion = null;
            try
            {
                conexion = objconexion.obtenerConexionExistente();

                string consulta =
                    @"SELECT COUNT(*) 
                      FROM productos 
                      WHERE productos.nombre = @nombre";

                SqlCommand cmd = new SqlCommand(consulta, conexion);
                cmd.Parameters.AddWithValue("@nombre", nombre);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al validar producto: " + ex.Message);
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

        public void mostrar(DataGridView g)
        {
            clases.conexion_simcov objconexion = new clases.conexion_simcov();
            try
            {
                g.DataSource = null;
                SqlDataAdapter adaptador = new SqlDataAdapter(
                    "SELECT * FROM productos", objconexion.obtenerConexionExistente());

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

        public void seleccionar(DataGridView t, TextBox id_producto, TextBox nombre, TextBox precio, TextBox costo, TextBox stock, TextBox stock_minimo)
        {
            try
            {
                id_producto.Text = t.CurrentRow.Cells[0].Value.ToString();
                nombre.Text = t.CurrentRow.Cells[1].Value.ToString();
                precio.Text = t.CurrentRow.Cells[2].Value.ToString();
                costo.Text = t.CurrentRow.Cells[3].Value.ToString();
                stock.Text = t.CurrentRow.Cells[4].Value.ToString();
                stock_minimo.Text = t.CurrentRow.Cells[5].Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se logro selecccionar registros. Error: " + ex.ToString());
            }
        }

        public bool actualizar(TextBox id_producto, TextBox nombre, TextBox precio, TextBox costo, TextBox stock, TextBox stock_minimo)
        {
            clases.conexion_simcov objconexion = new clases.conexion_simcov();

            // Validaciones
            if (string.IsNullOrWhiteSpace(nombre.Text) ||
                 string.IsNullOrWhiteSpace(precio.Text) ||
                 string.IsNullOrWhiteSpace(costo.Text))
            {
                MessageBox.Show("Los campos nombre, precio, costo son obligatorios");
                return false;
            }

            if (!EsNumeroDecimalValido(precio.Text, out decimal precioValue) ||
                !EsNumeroDecimalValido(costo.Text, out decimal costoValue) ||
                !decimal.TryParse(precio.Text, out precioValue) || precioValue <= 0 ||
                (!string.IsNullOrWhiteSpace(costo.Text) && !decimal.TryParse(costo.Text, out decimal costoVal1)) ||
                !int.TryParse(stock.Text, out int stockValue) || stockValue < 0 ||
                (!string.IsNullOrWhiteSpace(stock_minimo.Text) && (!int.TryParse(stock_minimo.Text, out int stockMinimoValue) || stockMinimoValue < 0))
            )
            {
                MessageBox.Show("Verifique los campos. Hay errores en el ingreso de datos.");
                return false;
            }

            if (precioValue < costoValue)
            {
                MessageBox.Show("El precio de venta no puede ser menor al costo");
                return false;
            }

            try
            {
                string query = "UPDATE productos set productos.nombre = '" + nombre.Text +
                "', productos.precio = '" + precio.Text +
                "', productos.costo = '" + costo.Text +
                "', productos.stock = '" + stock.Text +
                "', productos.stock_minimo = '" + stock_minimo.Text +
                "' WHERE productos.id_producto = '" + id_producto.Text + "';";

                SqlCommand micomando = new SqlCommand(query, objconexion.obtenerConexionExistente());
                SqlDataReader mireader = micomando.ExecuteReader();
                while (mireader.Read()) { }
                MessageBox.Show("Se modificó correctamente.");
                objconexion.cerrarConexion();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se logró modificar registros: " + ex.ToString());
                return false;
            }
        }

        public void eliminar(TextBox id_producto)
        {
            clases.conexion_simcov objconexion = new clases.conexion_simcov();
            try
            {
                String query = "DELETE FROM productos WHERE productos.id_producto = '" + id_producto.Text + "';";
                SqlCommand micomando = new SqlCommand(query, objconexion.obtenerConexionExistente());
                SqlDataReader mireader;
                mireader = micomando.ExecuteReader();
                MessageBox.Show("Se elimino correctamente el registro.");
                while (mireader.Read())
                { }
                objconexion.cerrarConexion();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se logro eliminar el registro. " + ex.ToString());
            }
        }

        private bool EsNumeroDecimalValido(string valor, out decimal resultado)
        {
            foreach (char c in valor)
            {
                if (!char.IsDigit(c) && c != '.')
                {
                    resultado = 0;
                    return false;
                }
            }

            return decimal.TryParse(valor, out resultado);
        }

        public void buscarProducto(DataGridView g, TextBox nombre)
        {
            clases.conexion_simcov objconexion = new clases.conexion_simcov();

            try
            {
                g.DataSource = null;
                SqlDataAdapter adaptador = new SqlDataAdapter(
                    "SELECT id_producto, nombre, precio, costo, stock, stock_minimo FROM productos WHERE productos.nombre LIKE '%" + nombre.Text + "%'", objconexion.obtenerConexionExistente());

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

        public void seleccionarProducto(DataGridView t, TextBox id_producto, TextBox nombre, TextBox precio, TextBox costo, TextBox stock, TextBox stock_minimo)
        {
            try
            {
                id_producto.Text = t.CurrentRow.Cells[0].Value.ToString();
                nombre.Text = t.CurrentRow.Cells[1].Value.ToString();
                precio.Text = t.CurrentRow.Cells[2].Value.ToString();
                costo.Text = t.CurrentRow.Cells[3].Value.ToString();
                stock.Text = t.CurrentRow.Cells[4].Value.ToString();
                stock_minimo.Text = t.CurrentRow.Cells[5].Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se logro selecccionar registros. Error: " + ex.ToString());
            }
        }

        public void sumarStock(int id_producto, int cantidadASumar)
        {
            if (id_producto <= 0 || cantidadASumar <= 0)
            {
                MessageBox.Show("ID de producto o cantidad inválida.");
                return;
            }

            clases.conexion_simcov objconexion = new clases.conexion_simcov();

            try
            {
                string query = "UPDATE productos SET stock = stock + @cantidad WHERE id_producto = @id";
                SqlCommand comando = new SqlCommand(query, objconexion.obtenerConexionExistente());
                comando.Parameters.AddWithValue("@cantidad", cantidadASumar);
                comando.Parameters.AddWithValue("@id", id_producto);

                int filas = comando.ExecuteNonQuery();

                if (filas > 0)
                {
                    MessageBox.Show("Stock actualizado correctamente.");
                }
                else
                {
                    MessageBox.Show("No se encontró el producto.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar el stock: " + ex.Message);
            }
            finally
            {
                objconexion.cerrarConexion();
            }
        }
    }
}

