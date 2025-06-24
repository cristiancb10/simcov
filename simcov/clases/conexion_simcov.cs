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
    internal class conexion_simcov
    {

        SqlConnection conex;

        private readonly string cadenaConexion = "Data Source=DESKTOP-T6ME3UQ;Initial Catalog=simcov_db;User Id=simcov_db;Password=MateriaInf430";


        public SqlConnection establecerConexion()
        {
            try
            {
   
                conex = new SqlConnection(cadenaConexion);
                conex.Open();

            }
            catch(Exception) 
            {
                MessageBox.Show("No se pudo conectar a la base de datos.");
            }
            return conex;
        }

        public SqlConnection obtenerConexionExistente()
        {
            if (conex != null && conex.State == ConnectionState.Open)
            {
                return conex;
            }
            else
            {
                return establecerConexion();
            }
        }

        public void cerrarConexion()
        {
            conex.Close();
        }

    }
}
