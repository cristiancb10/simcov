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
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
            this.Location = new Point(0, 0);

            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10.8f, FontStyle.Bold);
            this.dataGridView1.Font = new Font("Segoe UI", 10.8f, FontStyle.Regular);
        }

        private void mostrarReporte(string consulta)
        {
            try
            {
                clases.conexion_simcov objconexion = new clases.conexion_simcov();
                SqlDataAdapter adaptador = new SqlDataAdapter(consulta, objconexion.obtenerConexionExistente());
                DataTable dt = new DataTable();
                adaptador.Fill(dt);
                dataGridView1.DataSource = dt;
                objconexion.cerrarConexion();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al mostrar el reporte: " + ex.Message);
            }
        }


        private void button8_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string consulta = @"SELECT 
                                CAST(v.id_venta AS VARCHAR) AS id_venta, 
                                ISNULL(c.nombre, 'Cliente anónimo') AS Cliente, 
                                CONVERT(VARCHAR, v.fecha, 120) AS fecha, 
                                CAST(v.total AS VARCHAR) AS total
                                FROM Ventas v
                                LEFT JOIN Clientes c ON v.id_cliente = c.id_cliente
                                WHERE CONVERT(DATE, v.fecha) = CONVERT(DATE, GETDATE())
                                AND v.anulada = 0

                                UNION ALL

                                SELECT 
                                    'TOTAL VENTAS DE HOY:',' ', '', CAST(SUM(v.total) AS VARCHAR)
                                FROM Ventas v
                                WHERE CONVERT(DATE, v.fecha) = CONVERT(DATE, GETDATE())
                                AND v.anulada = 0";

            mostrarReporte(consulta);
            textBox1.Clear();
            comboBox2.SelectedIndex = -1;
            comboBox1.SelectedIndex = -1;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            textBox1.Clear();
            comboBox2.SelectedIndex = -1;
            comboBox1.SelectedIndex = -1;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            string consulta = @"SELECT id_cliente, nombre, telefono, direccion, fecha_registro
                              FROM Clientes";
            mostrarReporte(consulta);

            textBox1.Clear();
            comboBox2.SelectedIndex = -1;
            comboBox1.SelectedIndex = -1;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            string consulta = @"SELECT id_producto, nombre, precio, costo, stock, stock_minimo, fecha_creacion
                              FROM Productos";
            mostrarReporte(consulta);

            textBox1.Clear();
            comboBox2.SelectedIndex = -1;
            comboBox1.SelectedIndex = -1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string consulta = @"SELECT p.nombre AS Producto, SUM(dv.cantidad) AS TotalUnidades, SUM(dv.subtotal) AS TotalVentas
                              FROM Productos p, DetalleVenta dv
                              WHERE p.id_producto = dv.id_producto
                              GROUP BY p.nombre
                              ORDER BY TotalUnidades DESC";
            mostrarReporte(consulta);

            textBox1.Clear();
            comboBox2.SelectedIndex = -1;
            comboBox1.SelectedIndex = -1;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string consulta = @"SELECT c.nombre AS Cliente, COUNT(v.id_venta) AS CantidadVentas, SUM(v.total) AS MontoTotal
                              FROM Clientes c, Ventas v
                              WHERE c.id_cliente = v.id_cliente
                              GROUP BY c.nombre
                              ORDER BY MontoTotal DESC";
            mostrarReporte(consulta);

            textBox1.Clear();
            comboBox2.SelectedIndex = -1;
            comboBox1.SelectedIndex = -1;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string consulta = @"SELECT CONVERT(DATE, fecha) AS Dia, 
                                       SUM(total) AS Total
                                FROM Ventas
                                WHERE CONVERT(DATE, fecha) = @dia
                                AND ventas.anulada = 0
                                GROUP BY CONVERT(DATE, fecha)
                                ORDER BY Dia DESC";

            clases.conexion_simcov objconexion = new clases.conexion_simcov();
            SqlDataAdapter adaptador = new SqlDataAdapter(consulta, objconexion.obtenerConexionExistente());

            // Usamos la fecha seleccionada en el DateTimePicker (formato: yyyy-MM-dd)
            adaptador.SelectCommand.Parameters.AddWithValue("@dia", dateTimePicker1.Value.Date);

            DataTable dt = new DataTable();
            adaptador.Fill(dt);
            dataGridView1.DataSource = dt;
            objconexion.cerrarConexion();

            textBox1.Clear();
            comboBox2.SelectedIndex = -1;
            comboBox1.SelectedIndex = -1;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string consulta = @"SELECT p.nombre AS Producto, SUM(dv.cantidad) AS TotalVendidos
                                FROM Productos p, DetalleVenta dv
                                WHERE p.id_producto = dv.id_producto
                                GROUP BY p.nombre
                                ORDER BY TotalVendidos DESC";
            mostrarReporte(consulta);

            textBox1.Clear();
            comboBox2.SelectedIndex = -1;
            comboBox1.SelectedIndex = -1;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string consulta = @"SELECT p.nombre AS Producto, SUM(dv.cantidad) AS TotalVendidos
                                FROM Productos p, DetalleVenta dv
                                WHERE p.id_producto = dv.id_producto
                                GROUP BY p.nombre
                                ORDER BY TotalVendidos ASC";
            mostrarReporte(consulta);

            textBox1.Clear();
            comboBox2.SelectedIndex = -1;
            comboBox1.SelectedIndex = -1;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string consulta = @"SELECT nombre, stock, stock_minimo
                                FROM Productos
                                ORDER BY stock ASC";
            mostrarReporte(consulta);

            textBox1.Clear();
            comboBox2.SelectedIndex = -1;
            comboBox1.SelectedIndex = -1;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            string consulta;

            if (string.IsNullOrWhiteSpace(comboBox2.Text))
            {
                consulta = @"SELECT 
                                FORMAT(fecha, 'yyyy-MM') AS Mes, 
                                SUM(total) AS Total
                            FROM Ventas 
                            WHERE ventas.anulada = 0
                            GROUP BY FORMAT(fecha, 'yyyy-MM')
                            ORDER BY Mes DESC";
            }
            else
            {
                consulta = @"SELECT 
                                FORMAT(fecha, 'yyyy-MM') AS Mes, 
                                SUM(total) AS Total
                            FROM Ventas
                            WHERE FORMAT(fecha, 'yyyy-MM') = @mes
                            AND ventas.anulada = 0
                            GROUP BY FORMAT(fecha, 'yyyy-MM')";
            }

            clases.conexion_simcov objconexion = new clases.conexion_simcov();
            SqlDataAdapter adaptador = new SqlDataAdapter(consulta, objconexion.obtenerConexionExistente());

            if (!string.IsNullOrWhiteSpace(comboBox2.Text))
            {
                Dictionary<string, string> meses = new Dictionary<string, string>()
                {
                    {"Enero", "01"},
                    {"Febrero", "02"},
                    {"Marzo", "03"},
                    {"Abril", "04"},
                    {"Mayo", "05"},
                    {"Junio", "06"},
                    {"Julio", "07"},
                    {"Agosto", "08"},
                    {"Septiembre", "09"},
                    {"Octubre", "10"},
                    {"Noviembre", "11"},
                    {"Diciembre", "12"},
                };

                 string mesSeleccionado = comboBox2.Text.Trim();
                 if (meses.ContainsKey(mesSeleccionado))
                 {
                     string anioActual = DateTime.Now.Year.ToString();
                     string valorMes = $"{anioActual}-{meses[mesSeleccionado]}";
                     adaptador.SelectCommand.Parameters.AddWithValue("@mes", valorMes);
                 }
                 else
                 {
                     MessageBox.Show("Seleccione un mes válido del listado.", "Mes inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                     return;
                 }
            }

            DataTable dt = new DataTable();
            adaptador.Fill(dt);
            dataGridView1.DataSource = dt;
            objconexion.cerrarConexion();

            textBox1.Clear();
            comboBox1.SelectedIndex = -1;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            string consulta;

            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                // Mostrar todas las ventas por año
                consulta = @"SELECT 
                             YEAR(fecha) AS Año, 
                             SUM(total) AS Total
                             FROM Ventas
                             WHERE ventas.anulada = 0
                             GROUP BY YEAR(fecha)
                             ORDER BY Año DESC";
            }
            else
            {
                // Mostrar solo el año especificado
                consulta = @"SELECT 
                            YEAR(fecha) AS Año, 
                            SUM(total) AS Total
                            FROM Ventas
                            WHERE YEAR(fecha) = @anio
                            AND ventas.anulada = 0
                            GROUP BY YEAR(fecha)";
            }

            clases.conexion_simcov objconexion = new clases.conexion_simcov();
            SqlDataAdapter adaptador = new SqlDataAdapter(consulta, objconexion.obtenerConexionExistente());

            if (!string.IsNullOrWhiteSpace(textBox1.Text))
            {
                if (int.TryParse(textBox1.Text, out int anio))
                {
                    if (anio <= 0)
                    {
                        MessageBox.Show("El año debe ser un número mayor que cero.", "Año inválido");
                        return;
                    }
                    adaptador.SelectCommand.Parameters.AddWithValue("@anio", anio);
                }
                else
                {
                    MessageBox.Show("Por favor, ingrese un año válido (número).", "Año inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            DataTable dt = new DataTable();
            adaptador.Fill(dt);
            dataGridView1.DataSource = dt;
            objconexion.cerrarConexion();

            comboBox2.SelectedIndex = -1;
            comboBox1.SelectedIndex = -1;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            // Verificar si hay datos en el DataGridView
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("No hay datos para exportar.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Crear un SaveFileDialog para que el usuario elija dónde guardar el archivo
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Archivo de texto (*.txt)|*.txt";
            saveFileDialog.Title = "Guardar reporte como texto";
            saveFileDialog.FileName = "Reporte_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    exportarATxt(saveFileDialog.FileName);
                    MessageBox.Show("Reporte exportado correctamente como archivo TXT.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al exportar el reporte: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void exportarATxt(string filePath)
        {
            using (StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                // Determinar el ancho máximo de cada columna para el formato
                int[] columnWidths = new int[dataGridView1.Columns.Count];
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    columnWidths[i] = dataGridView1.Columns[i].HeaderText.Length;
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.Cells[i].Value != null)
                        {
                            int length = row.Cells[i].Value.ToString().Length;
                            if (length > columnWidths[i])
                            {
                                columnWidths[i] = length;
                            }
                        }
                    }
                    columnWidths[i] += 2; // Agregar un poco de espacio extra
                }

                // Escribir encabezados
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    sw.Write(dataGridView1.Columns[i].HeaderText.PadRight(columnWidths[i]));
                }
                sw.WriteLine();

                // Línea separadora
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    sw.Write(new string('-', columnWidths[i]));
                }
                sw.WriteLine();

                // Escribir datos
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    for (int i = 0; i < dataGridView1.Columns.Count; i++)
                    {
                        string value = row.Cells[i].Value?.ToString() ?? "";
                        sw.Write(value.PadRight(columnWidths[i]));
                    }
                    sw.WriteLine();
                }
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            string consulta = @"SELECT 
                                    CAST(v.id_venta AS VARCHAR) AS id_venta, 
                                    ISNULL(c.nombre, 'Cliente anónimo') AS Cliente, 
                                    CONVERT(VARCHAR, v.fecha, 120) AS fecha, 
                                    CAST(v.total AS VARCHAR) AS total,
                                    CAST(v.id_usuario AS VARCHAR) AS usuario_anulador
                                FROM Ventas v
                                LEFT JOIN Clientes c ON v.id_cliente = c.id_cliente
                                WHERE CONVERT(DATE, v.fecha) = CONVERT(DATE, GETDATE())
                                AND v.anulada = 1

                                UNION ALL

                                SELECT 
                                    'TOTAL VENTAS ANULADAS:','—',  '', CAST(SUM(v.total) AS VARCHAR),''
                                FROM Ventas v
                                WHERE CONVERT(DATE, v.fecha) = CONVERT(DATE, GETDATE())
                                AND v.anulada = 1";

            mostrarReporte(consulta);
            textBox1.Clear();
            comboBox2.SelectedIndex = -1;
            comboBox1.SelectedIndex = -1;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            button12.PerformClick();
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            this.AcceptButton = button13;
        }

        private void button16_Click(object sender, EventArgs e)
        {
            if (clases.SesionUsuario.Rol != "administrador")
            {
                MessageBox.Show("No tienes permiso para realizar esta acción.");
                return;
            }

            string consulta = @"SELECT 
                                    p.nombre AS producto,
                                    SUM(dv.cantidad) AS cantidad_vendida,
                                    CAST(AVG(dv.precio_unitario) AS DECIMAL(10,2)) AS precio_promedio,
                                    CAST(AVG(dv.costo_historico) AS DECIMAL(10,2)) AS costo_promedio,
                                    CAST(SUM(dv.subtotal) AS DECIMAL(10,2)) AS ventas_totales,
                                    CAST(SUM((dv.precio_unitario - dv.costo_historico) * dv.cantidad) AS DECIMAL(10,2)) AS ganancia_total,
                                    0 AS orden  -- Columna adicional para ordenar
                                FROM Ventas v
                                INNER JOIN DetalleVenta dv ON v.id_venta = dv.id_venta
                                INNER JOIN Productos p ON dv.id_producto = p.id_producto
                                WHERE 
                                    CAST(v.fecha AS DATE) = CAST(GETDATE() AS DATE)
                                    AND v.anulada = 0
                                GROUP BY 
                                    p.nombre

                                UNION ALL

                                SELECT 
                                    'TOTAL GENERAL' AS producto,
                                    SUM(dv.cantidad) AS cantidad_vendida,
                                    NULL AS precio_promedio,
                                    NULL AS costo_promedio,
                                    CAST(SUM(dv.subtotal) AS DECIMAL(10,2)) AS ventas_totales,
                                    CAST(SUM((dv.precio_unitario - dv.costo_historico) * dv.cantidad) AS DECIMAL(10,2)) AS ganancia_total,
                                    1 AS orden  -- Columna adicional para ordenar
                                FROM Ventas v
                                INNER JOIN DetalleVenta dv ON v.id_venta = dv.id_venta
                                INNER JOIN Productos p ON dv.id_producto = p.id_producto
                                WHERE 
                                    CAST(v.fecha AS DATE) = CAST(GETDATE() AS DATE)
                                    AND v.anulada = 0
                                ORDER BY 
                                    orden, ganancia_total DESC";
            mostrarReporte(consulta);

            textBox1.Clear();
            comboBox2.SelectedIndex = -1;
            comboBox1.SelectedIndex = -1;
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            if (clases.SesionUsuario.Rol == "administrador")
            {
                button16.Visible = true;
            }
            else
            {
                button18.Visible = false;
                button16.Visible = false;
                button4.Visible = false;
                button12.Visible = false;
                button13.Visible = false;
                button5.Visible = false;
                button6.Visible = false;
                button1.Visible = false;
                button2.Visible = false;
                button3.Visible = false;
                dateTimePicker1.Visible = false;
                comboBox1.Visible = false;
                comboBox2.Visible = false;  
                textBox1.Visible = false;
            }

            cargarUsuariosEnComboBox();
        }

        private void cargarUsuariosEnComboBox()
        {
            try
            {
                using (SqlConnection conn = new clases.conexion_simcov().establecerConexion())
                {
                    string consulta = "SELECT id_usuario, nombre_usuario FROM Usuarios";
                    SqlDataAdapter da = new SqlDataAdapter(consulta, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Agrega una opción "Todos" o vacía si deseas
                    DataRow filaDefault = dt.NewRow();
                    filaDefault["id_usuario"] = DBNull.Value;
                    filaDefault["nombre_usuario"] = " ";
                    dt.Rows.InsertAt(filaDefault, 0);

                    comboBox1.DataSource = dt;
                    comboBox1.DisplayMember = "nombre_usuario";
                    comboBox1.ValueMember = "id_usuario";
                    comboBox1.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar usuarios: " + ex.Message);
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            int idUsuario;

            if (comboBox1.SelectedIndex > 0) // comboBox1 es donde eliges usuario
            {
                idUsuario = Convert.ToInt32(comboBox1.SelectedValue);
            }
            else
            {
                idUsuario = clases.SesionUsuario.IdUsuario;
            }

            string consulta = $@"SELECT 
                                    CAST(v.id_venta AS VARCHAR) AS id_venta, 
                                    ISNULL(c.nombre, 'Cliente anónimo') AS Cliente, 
                                    CONVERT(VARCHAR, v.fecha, 120) AS fecha, 
                                    CAST(v.total AS VARCHAR) AS total
                                FROM Ventas v
                                LEFT JOIN Clientes c ON v.id_cliente = c.id_cliente
                                WHERE v.id_usuario = {idUsuario}
                                AND v.anulada = 0
                                AND CONVERT(DATE, v.fecha) = CONVERT(DATE, GETDATE())

                                UNION ALL

                                SELECT 
                                    'TOTAL DEL USUARIO', '', '', 
                                    CAST(SUM(v.total) AS VARCHAR)
                                FROM Ventas v
                                WHERE v.id_usuario = {idUsuario}
                                AND v.anulada = 0
                                AND CONVERT(DATE, v.fecha) = CONVERT(DATE, GETDATE());";

            mostrarReporte(consulta);
            textBox1.Clear();
            comboBox2.SelectedIndex = -1;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button17.PerformClick();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("No hay datos para exportar.");
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Archivo CSV (*.csv)|*.csv";
            saveFileDialog.Title = "Guardar como archivo Excel (CSV)";
            saveFileDialog.FileName = "Reporte_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    exportarAExcelCSV(saveFileDialog.FileName);
                    MessageBox.Show("Exportado correctamente a CSV.", "Éxito");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al exportar: " + ex.Message, "Error");
                }
            }
        }

        private void exportarAExcelCSV(string filePath)
        {
            using (StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                // Escribir encabezados
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    sw.Write(dataGridView1.Columns[i].HeaderText);
                    if (i < dataGridView1.Columns.Count - 1)
                        sw.Write(",");
                }
                sw.WriteLine();

                // Escribir filas
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    for (int i = 0; i < dataGridView1.Columns.Count; i++)
                    {
                        var valor = row.Cells[i].Value?.ToString().Replace(",", " "); // Evitar conflictos
                        sw.Write(valor);
                        if (i < dataGridView1.Columns.Count - 1)
                            sw.Write(",");
                    }
                    sw.WriteLine();
                }
            }
        }

    }
}
