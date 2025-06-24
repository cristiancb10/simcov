using simcov.clases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace simcov
{
    public partial class Form8 : Form
    {
        public Form8()
        {
            InitializeComponent();

            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10.8f, FontStyle.Bold);
            this.dataGridView1.Font = new Font("Segoe UI", 10.8f, FontStyle.Regular);

            dataGridView2.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10.8f, FontStyle.Bold);
            this.dataGridView2.Font = new Font("Segoe UI", 10.8f, FontStyle.Regular);

            dataGridView1.AlternatingRowsDefaultCellStyle.Font = new Font("Segoe UI", 10.8f);
            dataGridView2.AlternatingRowsDefaultCellStyle.Font = new Font("Segoe UI", 10.8f);

            mostrarVentas();

        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView2_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)  // Verificamos que no sea header o fuera de rango
            {
                DataGridViewRow fila = dataGridView2.Rows[e.RowIndex];

                textBox1.Text = fila.Cells["id_venta"].Value?.ToString() ?? "";
                textBox2.Text = fila.Cells["id_cliente"].Value?.ToString() ?? "";
                textBox3.Text = fila.Cells["fecha"].Value?.ToString() ?? "";
                textBox4.Text = fila.Cells["total"].Value?.ToString() ?? "";
                textBox5.Text = fila.Cells["id_usuario"].Value?.ToString() ?? "";
                bool anulada = fila.Cells["anulada"].Value != DBNull.Value && Convert.ToBoolean(fila.Cells["anulada"].Value);
                comboBox1.SelectedItem = anulada ? "Anulada" : "Activa";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Instancia de la clase ventas
            clases.ventas objconexion = new clases.ventas();
            clases.conexion_simcov conexion = new clases.conexion_simcov();

            // Verificar rol de usuario
            if (clases.SesionUsuario.Rol != "administrador")
            {
                MessageBox.Show("Solo un administrador puede anular ventas.");
                return;
            }

            // Verificar que haya fila seleccionada en el DataGridView
            if (dataGridView2.CurrentRow == null)
            {
                MessageBox.Show("Seleccione una venta para anular.");
                return;
            }

            int idVenta = int.Parse(textBox1.Text);

            try
            {
                string query = "SELECT anulada FROM Ventas WHERE id_venta = @id";
                SqlCommand cmd = new SqlCommand(query, conexion.obtenerConexionExistente());
                cmd.Parameters.AddWithValue("@id", idVenta);

                object resultado = cmd.ExecuteScalar();

                if (resultado != null && Convert.ToBoolean(resultado))
                {
                    MessageBox.Show("Esta venta ya fue anulada.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al verificar el estado de la venta: " + ex.Message);
                return;
            }
            finally
            {
                conexion.cerrarConexion();
            }

            // Confirmación para anular
            DialogResult confirmar = MessageBox.Show("¿Está seguro de anular esta venta?", "Confirmar", MessageBoxButtons.YesNo);
            if (confirmar == DialogResult.Yes)
            {
                bool exito = objconexion.AnularVenta(idVenta);
                if (exito)
                {
                    MessageBox.Show("Venta anulada correctamente.");

                    dataGridView1.DataSource = null;
                    textBox1.Clear();
                    textBox2.Clear();
                    textBox3.Clear();
                    textBox4.Clear();
                    textBox5.Clear();
                    comboBox1.SelectedIndex = -1;
                    mostrarVentas(); 
                }
            }
        }

        private void mostrarVentas()
        {
            clases.conexion_simcov objConexion = new clases.conexion_simcov();
            SqlConnection conexion = objConexion.establecerConexion();

            try
            {
                string query = @"SELECT id_venta, id_cliente, fecha, total, id_usuario, anulada 
                         FROM Ventas";
                SqlDataAdapter da = new SqlDataAdapter(query, conexion);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Agregar columna para mostrar "Sí"/"No"
                dt.Columns.Add("AnuladaTexto", typeof(string));

                foreach (DataRow row in dt.Rows)
                {
                    bool anulada = false;
                    if (row["anulada"] != DBNull.Value)
                        anulada = Convert.ToBoolean(row["anulada"]);

                    row["AnuladaTexto"] = anulada ? "Sí" : "No";
                }

                dataGridView2.DataSource = dt;

                // Ocultar columna anulada original si quieres
                dataGridView2.Columns["anulada"].Visible = false;

                // Mostrar la columna amigable
                dataGridView2.Columns["AnuladaTexto"].HeaderText = "Anulada";

                // Otros ajustes de columnas
                dataGridView2.Columns["id_venta"].HeaderText = "ID Venta";
                dataGridView2.Columns["id_cliente"].HeaderText = "ID Cliente";
                dataGridView2.Columns["fecha"].HeaderText = "Fecha";
                dataGridView2.Columns["total"].HeaderText = "Total (Bs)";
                dataGridView2.Columns["id_usuario"].HeaderText = "ID Usuario";

                dataGridView2.Columns["fecha"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar ventas: " + ex.Message);
            }
            finally
            {
                objConexion.cerrarConexion();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            comboBox1.SelectedIndex = -1;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textBox1.Text.Trim(), out int idVenta))
            {
                MessageBox.Show("Ingrese un ID de venta válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            clases.conexion_simcov objConexion = new clases.conexion_simcov();
            SqlConnection conexion = objConexion.establecerConexion();

            try
            {
                // 1. Buscar información de la venta
                string queryVenta = @"SELECT V.id_venta, V.total, V.fecha, V.anulada, 
                                     C.nombre AS cliente, U.nombre_usuario 
                              FROM Ventas V 
                              LEFT JOIN Clientes C ON V.id_cliente = C.id_cliente 
                              INNER JOIN Usuarios U ON V.id_usuario = U.id_usuario 
                              WHERE V.id_venta = @idVenta";

                SqlCommand cmdVenta = new SqlCommand(queryVenta, conexion);
                cmdVenta.Parameters.AddWithValue("@idVenta", idVenta);

                SqlDataReader reader = cmdVenta.ExecuteReader();

                if (reader.Read())
                {
                    textBox2.Text = reader["cliente"]?.ToString() ?? "Cliente no registrado";
                    textBox3.Text = reader["total"].ToString();
                    textBox4.Text = Convert.ToDateTime(reader["fecha"]).ToString("dd/MM/yyyy HH:mm");
                    textBox5.Text = reader["nombre_usuario"].ToString();
                    bool anulada = Convert.ToBoolean(reader["anulada"]);
                    comboBox1.SelectedItem = anulada ? "Anulada" : "Activa";
                }
                else
                {
                    MessageBox.Show("No se encontró ninguna venta con ese ID.", "Sin resultados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                reader.Close();

                // 2. Mostrar detalle de productos vendidos
                string queryDetalle = @"SELECT P.nombre AS Producto, DV.cantidad, DV.precio_unitario, DV.subtotal 
                                FROM DetalleVenta DV 
                                INNER JOIN Productos P ON DV.id_producto = P.id_producto 
                                WHERE DV.id_venta = @idVenta";

                SqlCommand cmdDetalle = new SqlCommand(queryDetalle, conexion);
                cmdDetalle.Parameters.AddWithValue("@idVenta", idVenta);

                SqlDataAdapter adapter = new SqlDataAdapter(cmdDetalle);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar la venta: " + ex.Message);
            }
            finally
            {
                objConexion.cerrarConexion();
            }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            if (e.RowIndex >= 0)  
            {
                DataGridViewRow fila = dataGridView2.Rows[e.RowIndex];

                textBox1.Text = fila.Cells["id_venta"].Value?.ToString() ?? "";
                textBox2.Text = fila.Cells["id_cliente"].Value?.ToString() ?? "";
                textBox3.Text = fila.Cells["fecha"].Value?.ToString() ?? "";
                textBox4.Text = fila.Cells["total"].Value?.ToString() ?? "";
                textBox5.Text = fila.Cells["id_usuario"].Value?.ToString() ?? "";
                bool anulada = fila.Cells["anulada"].Value != DBNull.Value && Convert.ToBoolean(fila.Cells["anulada"].Value);
                comboBox1.SelectedItem = anulada ? "Anulada" : "Activa"; dataGridView1.DataSource = null;
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();
                comboBox1.SelectedIndex = -1;
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            this.AcceptButton = button6;
        }

        private void Form8_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("Activa");    
            comboBox1.Items.Add("Anulada");   
            comboBox1.SelectedIndex = -1;
        }
    }
}
