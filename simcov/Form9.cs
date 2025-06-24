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
    public partial class Form9 : Form
    {
        public Form9()
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

            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10.8f, FontStyle.Bold);
            this.dataGridView1.Font = new Font("Segoe UI", 10.8f, FontStyle.Regular);

            dataGridView2.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10.8f, FontStyle.Bold);
            this.dataGridView2.Font = new Font("Segoe UI", 10.8f, FontStyle.Regular);

            dataGridView1.AlternatingRowsDefaultCellStyle.Font = new Font("Segoe UI", 10.8f);
            dataGridView2.AlternatingRowsDefaultCellStyle.Font = new Font("Segoe UI", 10.8f);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            string nombre = textBox2.Text.Trim();

            if (string.IsNullOrEmpty(nombre))
            {
                MessageBox.Show("Ingresa un nombre de usuario para buscar.");
                return;
            }

            clases.conexion_simcov objConexion = new clases.conexion_simcov();
            SqlConnection conexion = objConexion.establecerConexion();

            try
            {
                string query = "SELECT id_usuario, nombre_usuario, rol, fecha_creacion FROM Usuarios WHERE nombre_usuario LIKE @nombre";
                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@nombre", "%" + nombre + "%");

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar usuario: " + ex.Message);
            }
            finally
            {
                objConexion.cerrarConexion();
            }
        }

        private void Form9_Load(object sender, EventArgs e)
        {
            if (clases.SesionUsuario.Rol != "administrador")
            {
                MessageBox.Show("Acceso denegado. Solo el administrador puede registrar nuevos usuarios.");
                this.Close();
            }

            comboBox1.Items.Add("administrador");
            comboBox1.Items.Add("empleado");
            comboBox1.SelectedIndex = 1;

            comboBox2.Items.Add("Todos");
            comboBox2.Items.Add("administrador");
            comboBox2.Items.Add("empleado");
            comboBox2.SelectedIndex = 0;

            CargarTodosLosUsuarios();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string usuario = textBox2.Text.Trim();
            string contraseña = clases.seguridad.ObtenerHashSha256(textBox3.Text.Trim());
            string rol = comboBox1.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(contraseña) || string.IsNullOrEmpty(rol))
            {
                MessageBox.Show("Todos los campos son obligatorios.");
                return;
            }

            //Validar si el usuario existe
            clases.usuarios objconexion = new clases.usuarios();
            if (objconexion.validarUsuario(textBox2.Text.Trim()))
            {
                MessageBox.Show("El usuario ya existe");
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                comboBox1.SelectedIndex = -1;
                dataGridView1.DataSource = null;
                return;
            }

            clases.conexion_simcov objConexion = new clases.conexion_simcov();
            SqlConnection conexion = objConexion.establecerConexion();

            try
            {
                string query = "INSERT INTO Usuarios (nombre_usuario, contraseña, rol) VALUES (@usuario, @contraseña, @rol)";
                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@usuario", usuario);
                cmd.Parameters.AddWithValue("@contraseña", contraseña);
                cmd.Parameters.AddWithValue("@rol", rol);
                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    MessageBox.Show("Usuario registrado correctamente.");
                    textBox2.Clear();
                    textBox3.Clear();
                    comboBox1.SelectedIndex = 1;
                    CargarTodosLosUsuarios();
                }
                else
                {
                    MessageBox.Show("No se pudo registrar el usuario.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                objConexion.cerrarConexion();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Todos los campos son obligatorios.");
                return;
            }

            int idUsuarioSeleccionado = int.Parse(textBox1.Text);
            int idUsuarioActual = clases.SesionUsuario.IdUsuario;
            string rolActual = clases.SesionUsuario.Rol;

            // Si NO es administrador, no se permite
            if (rolActual != "administrador")
            {
                MessageBox.Show("No está autorizado para modificar a otros usuarios.");
                return;
            }

            // Si es administrador, debe ingresar la contraseña del usuario seleccionado
            Form7 formVal = new Form7(); // Formulario para ingresar usuario y contraseña
            if (formVal.ShowDialog() != DialogResult.OK)
            {
                MessageBox.Show("Validación cancelada.");
                return;
            }

            // Validar credenciales ingresadas con la base de datos
            string usuarioIngresado = formVal.UsuarioIngresado;
            string contraseñaIngresadaHash = clases.seguridad.ObtenerHashSha256(formVal.ContraseñaIngresada);

            // Consultar base de datos si coincide
            using (SqlConnection conn = new clases.conexion_simcov().establecerConexion())
            {
                string query = "SELECT id_usuario FROM Usuarios WHERE id_usuario = @id AND nombre_usuario = @usuario AND contraseña = @password";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", idUsuarioSeleccionado);
                    cmd.Parameters.AddWithValue("@usuario", usuarioIngresado);
                    cmd.Parameters.AddWithValue("@password", contraseñaIngresadaHash);

                    var result = cmd.ExecuteScalar();
                    if (result == null)
                    {
                        MessageBox.Show("Autenticación fallida. No se puede modificar el usuario.");
                        textBox1.Clear();
                        textBox2.Clear();
                        textBox3.Clear();
                        comboBox1.SelectedIndex = -1;
                        dataGridView1.DataSource = null;
                        return;
                    }
                }
            }

            int idUsuario = int.Parse(textBox1.Text.Trim());
            string nombreUsuario = textBox2.Text.Trim();
            string contraseñaHash = clases.seguridad.ObtenerHashSha256(textBox3.Text.Trim());
            string rol = comboBox1.SelectedItem.ToString();

            clases.conexion_simcov objConexion = new clases.conexion_simcov();
            SqlConnection conexion = objConexion.establecerConexion();

            // Prevenir dejar el sistema sin administradores
            if (rol == "empleado" && idUsuario == clases.SesionUsuario.IdUsuario)
            {
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Usuarios WHERE rol = 'administrador'", conexion))
                {
                    int totalAdmins = (int)cmd.ExecuteScalar();

                    if (totalAdmins <= 1)
                    {
                        MessageBox.Show("No puede cambiar su rol a empleado porque es el único administrador. Debe haber al menos un administrador en el sistema.");
                        return;
                    }
                }
            }

            try
            {
                string query = "UPDATE Usuarios SET nombre_usuario = @usuario, contraseña = @contraseña, rol = @rol WHERE id_usuario = @id";
                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@usuario", nombreUsuario);
                cmd.Parameters.AddWithValue("@contraseña", contraseñaHash);
                cmd.Parameters.AddWithValue("@rol", rol);
                cmd.Parameters.AddWithValue("@id", idUsuario);

                int filas = cmd.ExecuteNonQuery();

                if (filas > 0)
                {
                    MessageBox.Show("Usuario actualizado correctamente.");
                    CargarTodosLosUsuarios();
                    textBox3.Clear();
                }
                else
                {
                    MessageBox.Show("No se actualizó ningún usuario.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar: " + ex.Message);
            }
            finally
            {
                objConexion.cerrarConexion();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Seleccione un usuario para eliminar.");
                return;
            }

            int idUsuario = int.Parse(textBox1.Text);

            // Prevenir que se borre a sí mismo
            if (idUsuario == clases.SesionUsuario.IdUsuario)
            {
                MessageBox.Show("No puedes eliminar tu propio usuario mientras estás logueado.");
                return;
            }

            // Confirmación de eliminación
            DialogResult confirmacion = MessageBox.Show(
                "¿Estás seguro de que deseas eliminar este usuario?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmacion != DialogResult.Yes)
            {
                return;
            }

            // Conexión
            clases.conexion_simcov objConexion = new clases.conexion_simcov();
            SqlConnection conexion = objConexion.establecerConexion();

            try
            {
                string rolUsuario = "";

                // Obtener el rol del usuario a eliminar
                using (SqlCommand cmdRol = new SqlCommand("SELECT rol FROM Usuarios WHERE id_usuario = @id", conexion))
                {
                    cmdRol.Parameters.AddWithValue("@id", idUsuario);
                    object result = cmdRol.ExecuteScalar();

                    if (result != null)
                    {
                        rolUsuario = result.ToString();
                    }
                }

                // Si es administrador, verificar cuántos hay
                if (rolUsuario == "administrador")
                {
                    using (SqlCommand cmdContar = new SqlCommand("SELECT COUNT(*) FROM Usuarios WHERE rol = 'administrador'", conexion))
                    {
                        int totalAdmin = (int)cmdContar.ExecuteScalar();

                        if (totalAdmin <= 1)
                        {
                            MessageBox.Show("No puedes eliminar al único administrador del sistema.");
                            return;
                        }
                    }
                }

                // Eliminar
                string query = "DELETE FROM Usuarios WHERE id_usuario = @id";
                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@id", idUsuario);

                int resultado = cmd.ExecuteNonQuery();

                if (resultado > 0)
                {
                    MessageBox.Show("Usuario eliminado correctamente.");
                    textBox1.Clear();
                    textBox2.Clear();
                    textBox3.Clear();
                    comboBox1.SelectedIndex = -1;

                    CargarTodosLosUsuarios();
                }
                else
                {
                    MessageBox.Show("No se eliminó ningún usuario.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar: " + ex.Message);
            }
            finally
            {
                objConexion.cerrarConexion();
            }
        }

        private void CargarTodosLosUsuarios()
        {
            clases.conexion_simcov objConexion = new clases.conexion_simcov();
            SqlConnection conexion = objConexion.establecerConexion();

            try
            {
                string query = "SELECT id_usuario, nombre_usuario, rol, fecha_creacion FROM Usuarios";
                SqlDataAdapter da = new SqlDataAdapter(query, conexion);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView2.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar usuarios: " + ex.Message);
            }
            finally
            {
                objConexion.cerrarConexion();
            }
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dataGridView1.Rows[e.RowIndex];
                textBox1.Text = fila.Cells["id_usuario"].Value.ToString();
                textBox2.Text = fila.Cells["nombre_usuario"].Value.ToString();
                comboBox1.SelectedItem = fila.Cells["rol"].Value.ToString();
                textBox3.Text = "";
            }
        }

        private void dataGridView2_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dataGridView2.Rows[e.RowIndex];
                textBox1.Text = fila.Cells["id_usuario"].Value.ToString();
                textBox2.Text = fila.Cells["nombre_usuario"].Value.ToString();
                comboBox1.SelectedItem = fila.Cells["rol"].Value.ToString();
                textBox3.Text = "";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            comboBox1.SelectedIndex = -1;
            dataGridView1.DataSource = null;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string rolSeleccionado = comboBox2.SelectedItem.ToString();

            clases.conexion_simcov objConexion = new clases.conexion_simcov();
            SqlConnection conexion = objConexion.establecerConexion();

            try
            {
                string query;

                if (rolSeleccionado == "Todos")
                {
                    query = "SELECT id_usuario, nombre_usuario, rol, fecha_creacion FROM Usuarios";
                }
                else
                {
                    query = "SELECT id_usuario, nombre_usuario, rol, fecha_creacion FROM Usuarios WHERE rol = @rol";
                }

                SqlCommand cmd = new SqlCommand(query, conexion);
                if (rolSeleccionado != "Todos")
                    cmd.Parameters.AddWithValue("@rol", rolSeleccionado);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView2.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al filtrar: " + ex.Message);
            }
            finally
            {
                objConexion.cerrarConexion();
            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && dataGridView1.CurrentRow != null)
            {
                DataGridViewRow fila = dataGridView1.CurrentRow;
                textBox1.Text = fila.Cells["id_usuario"].Value.ToString();
                textBox2.Text = fila.Cells["nombre_usuario"].Value.ToString();
                comboBox1.SelectedItem = fila.Cells["rol"].Value.ToString();
                textBox3.Text = "";

                dataGridView1.DataSource = null;
                dataGridView1.Rows.Clear();
                dataGridView1.ClearSelection();

                e.Handled = true;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dataGridView1.Rows[e.RowIndex];
                textBox1.Text = fila.Cells["id_usuario"].Value.ToString();
                textBox2.Text = fila.Cells["nombre_usuario"].Value.ToString();
                comboBox1.SelectedItem = fila.Cells["rol"].Value.ToString();
                textBox3.Text = "";
            }

            dataGridView1.DataSource = null;
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            this.AcceptButton = button6;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox3.PasswordChar = '\0';
            }
            else
            {
                textBox3.PasswordChar = '*';
            }
        }
    }
}
