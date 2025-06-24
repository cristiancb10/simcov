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
    internal class clientes
    {
        public void buscarCliente(DataGridView g, TextBox nombre)
        {
            clases.conexion_simcov objconexion = new clases.conexion_simcov();
            try
            {
                g.DataSource = null;
                SqlDataAdapter adaptador = new SqlDataAdapter(
                    "SELECT id_cliente,nombre,telefono, direccion FROM clientes WHERE clientes.nombre LIKE '%" + nombre.Text + "%'", objconexion.obtenerConexionExistente());

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

        public void seleccionarCliente(DataGridView t, TextBox id_cliente, TextBox nombre, TextBox telefono, TextBox direccion)
        {
            try
            {
                id_cliente.Text = t.CurrentRow.Cells[0].Value.ToString();
                nombre.Text = t.CurrentRow.Cells[1].Value.ToString();
                telefono.Text = t.CurrentRow.Cells[2].Value.ToString();
                direccion.Text = t.CurrentRow.Cells[3].Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se logro selecccionar registros. Error: " + ex.ToString());
            }
        }

        public bool insertar(TextBox nombre, TextBox telefono, TextBox direccion)
        {
            clases.conexion_simcov objconexion = new clases.conexion_simcov();

            if (string.IsNullOrWhiteSpace(nombre.Text) ||
                 string.IsNullOrWhiteSpace(telefono.Text) ||
                 string.IsNullOrWhiteSpace(direccion.Text))
            {
                MessageBox.Show("Los campos nombre, teléfono y dirección son obligatorios");
                return false;
            }

            if (!int.TryParse(telefono.Text, out int telefonoValue) || telefonoValue < 0)
            {
                MessageBox.Show("Ingrese una número de telefono válido");
                return false;
            }

            try
            {
                string query = "INSERT INTO clientes (nombre, telefono, direccion) " + " VALUES ('" + nombre.Text + "', '" + telefono.Text + "', '" + direccion.Text + "')";

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
                MessageBox.Show("Error al guardar. Asegúrese que los datos sean válidos. Error técnico: " + ex.Message);
                return false;
            }
            finally
            {
                objconexion.cerrarConexion();
            }
        }

        public void mostrar(DataGridView g)
        {
            clases.conexion_simcov objconexion = new clases.conexion_simcov();
            try
            {
                g.DataSource = null;
                SqlDataAdapter adaptador = new SqlDataAdapter(
                    "SELECT * FROM clientes", objconexion.obtenerConexionExistente());

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

        public void seleccionar(DataGridView t, TextBox id_cliente, TextBox nombre, TextBox telefono, TextBox direccion)
        {
            try
            {
                id_cliente.Text = t.CurrentRow.Cells[0].Value.ToString();
                nombre.Text = t.CurrentRow.Cells[1].Value.ToString();
                telefono.Text = t.CurrentRow.Cells[2].Value.ToString();
                direccion.Text = t.CurrentRow.Cells[3].Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se logro selecccionar registros. Error: " + ex.ToString());
            }
        }

        public bool actualizar(TextBox id_cliente, TextBox nombre, TextBox telefono, TextBox direccion)
        {
            clases.conexion_simcov objconexion = new clases.conexion_simcov();

            if (string.IsNullOrWhiteSpace(nombre.Text) ||
                 string.IsNullOrWhiteSpace(telefono.Text) ||
                 string.IsNullOrWhiteSpace(direccion.Text))
            {
                MessageBox.Show("Los campos nombre, teléfono y dirección son obligatorios");
                return false;
            }

            if (!int.TryParse(telefono.Text, out int telefonoValue) || telefonoValue < 0)
            {
                MessageBox.Show("Ingrese un número de teléfono válido");
                return false;
            }

            try
            {
                string query = "UPDATE clientes SET nombre = '" + nombre.Text +
                               "', telefono = '" + telefono.Text +
                               "', direccion = '" + direccion.Text +
                               "' WHERE id_cliente = '" + id_cliente.Text + "';";

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

        public void eliminar(TextBox id_cliente)
        {
            clases.conexion_simcov objconexion = new clases.conexion_simcov();
            try
            {
                string query = "DELETE FROM clientes WHERE clientes.id_cliente = @id;";
                SqlCommand micomando = new SqlCommand(query, objconexion.obtenerConexionExistente());
                micomando.Parameters.AddWithValue("@id", id_cliente.Text);
                micomando.ExecuteNonQuery(); 

                MessageBox.Show("Se eliminó correctamente el registro.");
                objconexion.cerrarConexion();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se logro eliminar el registro. " + ex.ToString());
            }
        }

        public bool validarCliente(string nombre)
        {
            clases.conexion_simcov objconexion = new clases.conexion_simcov();
            SqlConnection conexion = null;
            try
            {
                conexion = objconexion.obtenerConexionExistente();

                string consulta = @"SELECT COUNT(*) 
                                  FROM clientes 
                                  WHERE clientes.nombre = @nombre";
                SqlCommand cmd = new SqlCommand(consulta, conexion);
                cmd.Parameters.AddWithValue("@nombre", nombre);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al validar cliente: " + ex.Message);
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
    }
}
