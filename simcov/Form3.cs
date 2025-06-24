using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace simcov
{
    public partial class Form3 : Form
    {
        public Form3()
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

            configurarDataGridView3();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();

            //Si ambos datos de Id Y Nombre de productos estan vacios
            if (string.IsNullOrWhiteSpace(textBox1.Text) &&
                string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("No a digitado ningun producto o su ID para buscar");
                return;
            }
            //Si ambos estan llenados 
            if (!string.IsNullOrWhiteSpace(textBox1.Text) &&
                !string.IsNullOrWhiteSpace(textBox2.Text))
            {
                //Si el foco esta en id_cliente
                if (textBox2.Focused)
                {
                    dataGridView1.ClearSelection();
                    textBox1.Clear();
                    clases.ventas objconexion = new clases.ventas();
                    objconexion.buscarProductoNombre(dataGridView1, textBox2);
                    dataGridView1.Focus();
                }

                else if (textBox1.Focused)
                {
                    //Validar que el Id_cliente sea numerico
                    if (!textBox1.Text.All(char.IsDigit))
                    {
                        MessageBox.Show("El ID debe ser un valor numérico entero");
                        textBox1.Focus();
                    }
                    else
                    {
                        dataGridView1.ClearSelection();
                        textBox2.Clear();
                        clases.ventas objconexion = new clases.ventas();
                        objconexion.buscarProductoId(dataGridView1, textBox1);
                        dataGridView1.Focus();
                    }
                }
            }

            //Dato de Id_producto vacio -> busca por nombre
            if (string.IsNullOrWhiteSpace(textBox1.Text) &&
                !string.IsNullOrWhiteSpace(textBox2.Text))
            {
                dataGridView1.ClearSelection();
                clases.ventas objconexion = new clases.ventas();
                objconexion.buscarProductoNombre(dataGridView1, textBox2);
                return;
            }

            //Dato de nombre vacio -> busca por id_producto
            if (string.IsNullOrWhiteSpace(textBox2.Text) &&
                !string.IsNullOrWhiteSpace(textBox1.Text))
            {
                if (!textBox1.Text.All(char.IsDigit))
                {
                    MessageBox.Show("El ID debe ser un valor numérico entero");
                    textBox1.Focus();
                }
                else
                {
                    dataGridView1.ClearSelection();
                    textBox2.Clear();
                    clases.ventas objconexion = new clases.ventas();
                    objconexion.buscarProductoId(dataGridView1, textBox1);
                    dataGridView1.Focus();
                }
            }
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            clases.ventas objconexion = new clases.ventas();
            objconexion.seleccionarProducto(dataGridView1, textBox1, textBox2, textBox4);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
            textBox9.Clear();
            textBox10.Clear();
	        label14.Visible = false;
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            dataGridView1.ClearSelection();
            dataGridView2.DataSource = null;
            dataGridView2.Rows.Clear();
            dataGridView2.ClearSelection();
            dataGridView3.DataSource = null;
            dataGridView3.Rows.Clear();
            dataGridView3.ClearSelection();
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            button6.Enabled = false;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox3.Text) || string.IsNullOrEmpty(textBox4.Text))
            {
                return; 
            }

            try
            {
                decimal precio = decimal.Parse(textBox3.Text);
                decimal cantidad = decimal.Parse(textBox4.Text); 

                decimal total = precio * cantidad;

                textBox5.Text = total.ToString("N2"); 
            }
            catch (FormatException)
            {
                MessageBox.Show("Por favor, ingrese valores numéricos válidos.");
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                textBox8.ReadOnly = false;
                textBox10.ReadOnly = false;
                button6.Enabled = true;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                textBox8.ReadOnly = true;
                textBox10.ReadOnly = true;
                button6.Enabled = false;
                textBox8.BackColor = Color.FromArgb(70, 74, 78);
                textBox10.BackColor = Color.FromArgb(70, 74, 78);
                textBox8.Text = "";
                textBox10.Text = "";

                dataGridView2.DataSource = null;
                dataGridView2.Rows.Clear();
                dataGridView2.ClearSelection();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            dataGridView2.ClearSelection();

            // Ambos datos de Id y nombre vacios
            if (string.IsNullOrWhiteSpace(textBox8.Text) &&
                string.IsNullOrWhiteSpace(textBox10.Text))
            {
                MessageBox.Show("No a digitado ningun nombre o su ID para buscar");
                return;
            }

            //Si ambos estan llenados 
            if (!string.IsNullOrWhiteSpace(textBox8.Text) &&
                !string.IsNullOrWhiteSpace(textBox10.Text))
            {
                //Si el foco esta en id_cliente
                if (textBox8.Focused)
                {
                    dataGridView2.ClearSelection();
                    textBox10.Clear();
                    clases.ventas objconexion = new clases.ventas();
                    objconexion.buscarClienteNombre(dataGridView2, textBox8);
                    dataGridView2.Focus();
                }

                else if (textBox10.Focused)
                {
                    //Validar que el Id_cliente sea numerico
                    if (!textBox10.Text.All(char.IsDigit))
                    {
                        MessageBox.Show("El ID debe ser un valor numérico entero");
                        textBox10.Focus();
                    }
                    else
                    {
                        dataGridView2.ClearSelection();
                        textBox8.Clear();
                        clases.ventas objconexion = new clases.ventas();
                        objconexion.buscarClienteId(dataGridView2, textBox10);
                        dataGridView2.Focus();
                    }
                }
            }

            //Dato de Id_cliente vacio -> busca por nombre
            if (string.IsNullOrWhiteSpace(textBox10.Text) &&
                !string.IsNullOrWhiteSpace(textBox8.Text))
            {
                dataGridView2.ClearSelection();
                clases.ventas objconexion = new clases.ventas();
                objconexion.buscarClienteNombre(dataGridView2, textBox8);
                return;
            }

            //Dato de nombre vacio -> busca por id_cliente
            if (string.IsNullOrWhiteSpace(textBox8.Text) &&
                !string.IsNullOrWhiteSpace(textBox10.Text))
            {
                if (!textBox10.Text.All(char.IsDigit))
                {
                    MessageBox.Show("El ID debe ser un valor numérico entero");
                    textBox10.Focus();
                }
                else
                {
                    dataGridView2.ClearSelection();
                    textBox8.Clear();
                    clases.ventas objconexion = new clases.ventas();
                    objconexion.buscarClienteId(dataGridView2, textBox10);
                    dataGridView2.Focus();
                }
            }    
        }

        private void dataGridView2_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            clases.ventas objconexion = new clases.ventas();
            objconexion.seleccionarCliente(dataGridView2, textBox8, textBox10);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox2.Text) ||
                string.IsNullOrEmpty(textBox4.Text) ||
                string.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show("Por favor complete todos los campos del producto");
                return;
            }

            try
            {
                string nombreProducto = textBox2.Text;
                decimal precioUnitario = decimal.Parse(textBox4.Text);
                int cantidad = int.Parse(textBox3.Text);

                if (cantidad <= 0)
                {
                    MessageBox.Show($"Digite una cantidad positiva");
                    textBox3.Clear();
                    textBox3.Focus();
                    return;
                }

                clases.ventas obj = new clases.ventas();
                int stockDisponible = obj.obtenerStock(nombreProducto);

                if (cantidad > stockDisponible)
                {
                    MessageBox.Show($"No hay suficiente stock. Stock disponible: {stockDisponible}");
                    return;
                }

                bool productoExistente = false;

                foreach (DataGridViewRow row in dataGridView3.Rows)
                {
                    if (row.Cells["Producto"].Value?.ToString() == nombreProducto)
                    {
                        // Sumar a la cantidad existente
                        int cantidadActual = Convert.ToInt32(row.Cells["Cantidad"].Value);
                        if (cantidadActual + cantidad > stockDisponible)
                        {
                            MessageBox.Show($"No hay suficiente stock para agregar más unidades. Stock disponible: {stockDisponible}");
                            return;
                        }

                        row.Cells["Cantidad"].Value = cantidadActual + cantidad;
                        row.Cells["Subtotal"].Value = (cantidadActual + cantidad) * Convert.ToDecimal(row.Cells["PrecioUnitario"].Value);
                        productoExistente = true;
                        break;
                    }
                }

                decimal costoActual = obj.obtenerCosto(nombreProducto);

                if (!productoExistente)
                {
                    dataGridView3.Rows.Add(nombreProducto, precioUnitario, costoActual, cantidad, precioUnitario * cantidad);
                }
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();

                dataGridView1.DataSource = null;
                dataGridView1.Rows.Clear();
                dataGridView1.ClearSelection();
            }
            catch (FormatException)
            {
                MessageBox.Show("Por favor ingrese valores numéricos válidos para precio y cantidad");
            }
        }

        private void configurarDataGridView3()
        {
            dataGridView3.Columns.Clear();
            dataGridView3.Columns.Add("Producto", "Producto");
            dataGridView3.Columns.Add("PrecioUnitario", "Precio Unitario");
            dataGridView3.Columns.Add("CostoUnitario", "Costo Unitario");
            dataGridView3.Columns.Add("Cantidad", "Cantidad");
            dataGridView3.Columns.Add("Subtotal", "Subtotal");

            dataGridView3.Columns["PrecioUnitario"].DefaultCellStyle.Format = "N2";
            dataGridView3.Columns["CostoUnitario"].DefaultCellStyle.Format = "N2";
            dataGridView3.Columns["Subtotal"].DefaultCellStyle.Format = "N2";

            dataGridView3.Columns["PrecioUnitario"].ReadOnly = true;
            dataGridView3.Columns["CostoUnitario"].ReadOnly = true;
            dataGridView3.Columns["Subtotal"].ReadOnly = true;
            dataGridView3.Columns["Cantidad"].ReadOnly = false;

            dataGridView3.CellEndEdit += (sender, e) =>
            {
                // Verificar que estamos editando la columna de Cantidad y la fila es válida
                if (e.ColumnIndex == dataGridView3.Columns["Cantidad"].Index && e.RowIndex >= 0)
                {
                    DataGridViewRow row = dataGridView3.Rows[e.RowIndex];

                    // Validar que la celda no esté vacía
                    if (row.Cells["Cantidad"].Value == null || string.IsNullOrEmpty(row.Cells["Cantidad"].Value.ToString()))
                    {
                        MessageBox.Show("La cantidad no puede estar vacía");
                        row.Cells["Cantidad"].Value = 1;
                        return;
                    }

                    try
                    {
                        int nuevaCantidad = Convert.ToInt32(row.Cells["Cantidad"].Value);
                        string nombreProducto = row.Cells["Producto"].Value.ToString();
                        decimal precio = Convert.ToDecimal(row.Cells["PrecioUnitario"].Value);

                        // Validar stock 
                        clases.ventas obj = new clases.ventas();
                        int stockDisponible = obj.obtenerStock(nombreProducto);

                        // Validaciones combinadas
                        if (nuevaCantidad <= 0)
                        {
                            MessageBox.Show("La cantidad debe ser mayor a cero");
                            row.Cells["Cantidad"].Value = 1;
                            nuevaCantidad = 1;
                        }
                        else if (nuevaCantidad > stockDisponible)
                        {
                            MessageBox.Show($"No hay suficiente stock. Máximo disponible: {stockDisponible}");
                            row.Cells["Cantidad"].Value = stockDisponible;
                            nuevaCantidad = stockDisponible;
                        }

                        // Calcular nuevo subtotal
                        row.Cells["Subtotal"].Value = nuevaCantidad * precio;

                    }
                    catch (FormatException)
                    {
                        MessageBox.Show("Ingrese un valor numérico válido");
                        row.Cells["Cantidad"].Value = 1;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error inesperado: {ex.Message}");
                        row.Cells["Cantidad"].Value = 1;
                    }
                }
            };
        }

        private void button2_Click(object sender, EventArgs e)
        {
            decimal total = 0;

            foreach (DataGridViewRow row in dataGridView3.Rows)
            {
                if (row.Cells["Subtotal"].Value != null)
                {
                    total += Convert.ToDecimal(row.Cells["Subtotal"].Value);
                }
            }

            textBox9.Text = total.ToString("N2");
        }
        
        private void button8_Click(object sender, EventArgs e)
        {
            if (dataGridView3.SelectedRows.Count > 0)
            {
                DialogResult result = MessageBox.Show("¿Está seguro de eliminar el producto seleccionado?",
                                                   "Confirmar eliminación",
                                                   MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    foreach (DataGridViewRow row in dataGridView3.SelectedRows)
                    {
                        dataGridView3.Rows.Remove(row);
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor seleccione al menos un producto para eliminar");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Validar que el total mostrado coincida con la suma actual de productos
            decimal totalActual = CalcularTotalActual();
            decimal totalMostrado = string.IsNullOrEmpty(textBox9.Text) ? 0 : decimal.Parse(textBox9.Text);

            if (totalActual != totalMostrado)
            {
                MessageBox.Show("Debe calcular el total actualizado antes de registrar la venta.",
                              "Total no actualizado",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Warning);
                return;
            }

            clases.ventas objVentas = new clases.ventas();

            // Validaciones básicas
            if (dataGridView3.Rows.Count == 0)
            {
                MessageBox.Show("No hay productos para registrar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(textBox9.Text))
            {
                MessageBox.Show("Debe calcular el total primero", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Validación del cliente (solo si está en modo cliente registrado)
                if (!radioButton1.Checked && !radioButton2.Checked)
                {
                    MessageBox.Show("Debe seleccionar un tipo de cliente", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int? idCliente = null;

                if (radioButton2.Checked)
                {
                    idCliente = null;
                }

                if (radioButton1.Checked)
                {
                    if (string.IsNullOrWhiteSpace(textBox10.Text))
                    {
                        MessageBox.Show("Debe buscar al cliente", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    else if (!int.TryParse(textBox10.Text, out int tempId))
                    {
                        MessageBox.Show("El ID del cliente no es válido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else if (!objVentas.ValidarCliente(tempId, textBox8.Text.Trim()))
                    {
                        MessageBox.Show("El nombre y el ID del cliente no coinciden.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    else
                    {
                        idCliente = tempId;
                    }
                }

                // Validación del total
                if (!decimal.TryParse(textBox9.Text, out decimal totalVenta))
                {
                    MessageBox.Show("El total no es válido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Validación del pago
                if (!decimal.TryParse(textBox6.Text, out decimal efectivo) || efectivo < totalVenta)
                {
                    MessageBox.Show("El efectivo ingresado es menor al total de la venta", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Crear lista de productos para la venta
                List<DetalleVentaTemp> detalles = new List<DetalleVentaTemp>();

                foreach (DataGridViewRow row in dataGridView3.Rows)
                {
                    // Validar que la fila no sea vacía
                    if (row.IsNewRow) 
			            continue;

                    // Validar que todos los campos necesarios tengan valores
                    if (row.Cells["Producto"].Value == null ||
                        row.Cells["PrecioUnitario"].Value == null ||
                        row.Cells["Cantidad"].Value == null)
                    {
                        MessageBox.Show("Hay productos con datos incompletos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    try
                    {
                        detalles.Add(new DetalleVentaTemp
                        {
                            NombreProducto = row.Cells["Producto"].Value.ToString(),
                            PrecioUnitario = Convert.ToDecimal(row.Cells["PrecioUnitario"].Value),
                            CostoUnitario = Convert.ToDecimal(row.Cells["CostoUnitario"].Value),
                            Cantidad = Convert.ToInt32(row.Cells["Cantidad"].Value),
                            Subtotal = Convert.ToDecimal(row.Cells["Subtotal"].Value)
                        });
                    }
                    catch (FormatException)
                    {
                        MessageBox.Show($"Los datos del producto '{row.Cells["Producto"].Value}' no son válidos",
                                      "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // Confirmación antes de registrar
                DialogResult confirmacion = MessageBox.Show($"¿Confirmar venta por un total de Bs. {totalVenta:N2}?",
                                                         "Confirmar Venta",
                                                         MessageBoxButtons.YesNo,
                                                         MessageBoxIcon.Question);

                if (confirmacion != DialogResult.Yes) 
                    return;

                // Registrar la venta
                int idUsuario = clases.SesionUsuario.IdUsuario;
                bool resultado = objVentas.RegistrarVentaCompleta(idCliente, detalles, totalVenta, idUsuario);

                if (resultado)
                {
                    MessageBox.Show("Venta registrada exitosamente!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Limpiar formulario
                    button7_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al registrar venta: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public class DetalleVentaTemp
        {
            public string NombreProducto { get; set; }
            public decimal PrecioUnitario { get; set; }
            public decimal CostoUnitario { get; set; }
            public int Cantidad { get; set; }
            public decimal Subtotal { get; set; }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            this.AcceptButton = button5;
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            this.AcceptButton = button5;
        }

        private void textBox8_Enter(object sender, EventArgs e)
        {
            this.AcceptButton = button6;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            this.dataGridView1.Font = new Font("Segoe UI", 10.8f, FontStyle.Regular);
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10.8f, FontStyle.Bold);
            
            this.dataGridView2.Font = new Font("Segoe UI", 10.8f, FontStyle.Regular);
            dataGridView2.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10.8f, FontStyle.Bold);

            this.dataGridView3.Font = new Font("Segoe UI", 10.8f, FontStyle.Regular);
            dataGridView3.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10.8f, FontStyle.Bold);

        }

        private void dataGridView2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && dataGridView2.CurrentRow != null)
            {
                DataGridViewRow row = dataGridView2.CurrentRow;
                textBox10.Text = row.Cells["id_cliente"].Value.ToString();  
                textBox8.Text = row.Cells["nombre"].Value.ToString();

                button2.Focus();

                e.Handled = true; 
            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && dataGridView1.CurrentRow != null)
            {
                DataGridViewRow row = dataGridView1.CurrentRow;
                textBox1.Text = row.Cells["id_producto"].Value.ToString();
                textBox2.Text = row.Cells["nombre"].Value.ToString();
                textBox4.Text = row.Cells["precio"].Value.ToString();

                textBox3.Focus();

                e.Handled = true;
            }
        }

        private decimal CalcularTotalActual()
        {
            decimal totalActual = 0;

            foreach (DataGridViewRow row in dataGridView3.Rows)
            {
                if (row.Cells["Subtotal"].Value != null && !row.IsNewRow)
                {
                    totalActual += Convert.ToDecimal(row.Cells["Subtotal"].Value);
                }
            }

            return totalActual;
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            ValidarCambioYTotal();
        }

        private void ValidarCambioYTotal()
        {
            decimal totalActual = CalcularTotalActual();

            // Obtener y validar el total mostrado
            bool totalOk = decimal.TryParse(textBox9.Text, out decimal totalMostrado);
            bool efectivoOk = decimal.TryParse(textBox6.Text, out decimal efectivo);

            // Si el total no coincide con el calculado, mostrar advertencia
            if (!totalOk || totalActual != totalMostrado)
            {
                label14.Visible = true;
                textBox7.Text = "";
                return;
            }

            // Si no se ingresó efectivo o es inválido
            if (!efectivoOk || string.IsNullOrWhiteSpace(textBox6.Text))
            {
                label14.Visible = false;
                textBox7.Text = "";
                return;
            }

            // Calcular y mostrar el cambio
            decimal cambio = efectivo - totalMostrado;
            textBox7.Text = cambio.ToString("N2");

            // Mostrar advertencia si el efectivo es menor al total
            if (cambio < 0)
            {
                label14.Visible = true;
            }
            else
            {
                label14.Visible = false;
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            ValidarCambioYTotal();
        }
    }
}
