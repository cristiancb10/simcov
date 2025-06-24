using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace simcov
{
    public partial class Form10 : Form
    {
        public Form10()
        {
            InitializeComponent();
            this.SuspendLayout();

            this.AutoScroll = false;
            this.HorizontalScroll.Enabled = false;
            this.VerticalScroll.Enabled = false;
            this.AutoScrollMinSize = new Size(0, 0);
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Location = new Point(0, 0);

            this.ResumeLayout(false);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox2.PasswordChar = '\0';
            }
            else
            {
                textBox2.PasswordChar = '*';
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Debe ingresar la nueva contraseña.");
                return;
            }

            Form7 formVal = new Form7(); // Formulario para ingresar usuario y contraseña iniciales
            if (formVal.ShowDialog() != DialogResult.OK)
            {
                MessageBox.Show("Validación cancelada.");
                return;
            }

            string usuarioIngresado = formVal.UsuarioIngresado;
            string contraseñaIngresadaHash = clases.seguridad.ObtenerHashSha256(formVal.ContraseñaIngresada);

            // Verificar credenciales del usuario actual
            using (SqlConnection conn = new clases.conexion_simcov().establecerConexion())
            {
                string query = "SELECT id_usuario FROM Usuarios WHERE nombre_usuario = @usuario AND contraseña = @password";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@usuario", usuarioIngresado);
                    cmd.Parameters.AddWithValue("@password", contraseñaIngresadaHash);

                    object result = cmd.ExecuteScalar();
                    if (result == null || Convert.ToInt32(result) != clases.SesionUsuario.IdUsuario)
                    {
                        MessageBox.Show("Autenticación fallida. No se puede modificar la contraseña.");
                        textBox2.Clear();
                        return;
                    }
                }

                // Cambiar la contraseña del usuario actual
                try
                {
                    string nuevaContraseñaHash = clases.seguridad.ObtenerHashSha256(textBox2.Text.Trim());

                    string updateQuery = "UPDATE Usuarios SET contraseña = @contraseña WHERE id_usuario = @id";
                    using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
                    {
                        updateCmd.Parameters.AddWithValue("@contraseña", nuevaContraseñaHash);
                        updateCmd.Parameters.AddWithValue("@id", clases.SesionUsuario.IdUsuario);

                        int filas = updateCmd.ExecuteNonQuery();
                        if (filas > 0)
                        {
                            MessageBox.Show("Contraseña actualizada correctamente.");
                            textBox2.Clear();
                        }
                        else
                        {
                            MessageBox.Show("No se actualizó la contraseña.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al actualizar: " + ex.Message);
                }
            }
        }

        private void Form10_Load(object sender, EventArgs e)
        {
            textBox1.Text = clases.SesionUsuario.NombreUsuario;

        }
    }
}
