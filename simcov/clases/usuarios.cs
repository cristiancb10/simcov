using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace simcov.clases
{
    internal class usuarios
    {
        public bool validarUsuario(string nombre)
        {
            clases.conexion_simcov objconexion = new clases.conexion_simcov();
            SqlConnection conexion = null;
            try
            {
                conexion = objconexion.obtenerConexionExistente();

                string consulta = @"SELECT COUNT(*) 
                                  FROM Usuarios 
                                  WHERE usuarios.nombre_usuario = @nombre";
                SqlCommand cmd = new SqlCommand(consulta, conexion);
                cmd.Parameters.AddWithValue("@nombre", nombre);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al validar usuario: " + ex.Message);
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
