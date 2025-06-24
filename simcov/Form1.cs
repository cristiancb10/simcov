using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace simcov
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Shown += Form1_Shown;
            this.Activated += Form1_Activated;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string usuario = textBox1.Text;
            string password = clases.seguridad.ObtenerHashSha256(textBox2.Text);

            clases.conexion_simcov objconexion = new clases.conexion_simcov();
            SqlConnection conexion = objconexion.establecerConexion();

            if (conexion != null && conexion.State == ConnectionState.Open)
            {
                // Consulta a la tabla Usuarios
                string consulta = "SELECT id_usuario, rol FROM Usuarios WHERE nombre_usuario COLLATE Latin1_General_CS_AS = @usuario AND contraseña = @password";
                SqlCommand cmd = new SqlCommand(consulta, conexion);
                cmd.Parameters.AddWithValue("@usuario", usuario);
                cmd.Parameters.AddWithValue("@password", password); 

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    int idUsuario = reader.GetInt32(0);
                    string rol = reader.GetString(1);

                    // Guardar sesión
                    clases.SesionUsuario.IdUsuario = idUsuario;
                    clases.SesionUsuario.NombreUsuario = usuario;
                    clases.SesionUsuario.Rol = rol;

                    // Mostrar menú principal
                    Form2 menuPrincipal = new Form2();
                    menuPrincipal.Show();

                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Usuario o contraseña incorrectos.");
                }
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            LimpiarCamposLogin();
            textBox1.Focus();
        }

        public void LimpiarCamposLogin()
        {
            textBox2.Clear();
            textBox1.Clear();
            textBox1.Focus();
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            textBox1.Focus();
        }
    }
}
