using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace simcov
{
    public partial class Form4 : Form
    {
        public Form4()
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

            dataGridView2.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10.8f, FontStyle.Bold);
            this.dataGridView2.Font = new Font("Segoe UI", 10.8f, FontStyle.Regular);

            clases.inventario objconexion = new clases.inventario();
            objconexion.mostrar(dataGridView2);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            dataGridView1.DataSource = null;
        }


        private void dataGridView2_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            clases.inventario objconexion = new clases.inventario();
            objconexion.seleccionar(dataGridView2, textBox1, textBox2, textBox3, textBox4, textBox5, textBox6);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            clases.inventario objconexion = new clases.inventario();
            bool actualizado = objconexion.actualizar(textBox1, textBox2, textBox3, textBox4, textBox5, textBox6);

            if (actualizado)
            {
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();
                textBox6.Clear();
            }

            objconexion.mostrar(dataGridView2);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            clases.inventario objconexion = new clases.inventario();
            if (!objconexion.validarProducto(textBox2.Text.Trim()))
            {
                bool exito = objconexion.insertar(textBox2, textBox3, textBox4, textBox5, textBox6);

                if (exito)
                {
                    textBox1.Clear();
                    textBox2.Clear();
                    textBox3.Clear();
                    textBox4.Clear();
                    textBox5.Clear();
                    textBox6.Clear();

                    objconexion.mostrar(dataGridView2);
                }
            }

            else
            {
                MessageBox.Show("El producto ya existe");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            clases.inventario objconexion = new clases.inventario();
            objconexion.eliminar(textBox1);
            objconexion.mostrar(dataGridView2);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("No a digitado ningun producto para buscar");
                return;
            }
            else
            {
                dataGridView1.ClearSelection();
                clases.inventario objconexion = new clases.inventario();
                objconexion.buscarProducto(dataGridView1, textBox2);
            }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            clases.inventario objconexion = new clases.inventario();
            objconexion.seleccionarProducto(dataGridView1, textBox1, textBox2, textBox3, textBox4, textBox5, textBox6);
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            dataGridView1.ClearSelection();
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            this.AcceptButton = button6;
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            if (clases.SesionUsuario.Rol != "administrador")
            {
                MessageBox.Show("Acceso denegado.");
                this.Close();
            }

            this.dataGridView1.Font = new Font("Segoe UI", 10.8f, FontStyle.Regular);
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10.8f, FontStyle.Bold);
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && dataGridView1.CurrentRow != null)
            {
                DataGridViewRow row = dataGridView1.CurrentRow;
                textBox1.Text = row.Cells["id_producto"].Value.ToString();
                textBox2.Text = row.Cells["nombre"].Value.ToString();
                textBox3.Text = row.Cells["precio"].Value.ToString();
                textBox4.Text = row.Cells["costo"].Value.ToString();
                textBox5.Text = row.Cells["stock"].Value.ToString();
                textBox6.Text = row.Cells["stock_minimo"].Value.ToString();

                dataGridView1.DataSource = null;
                dataGridView1.Rows.Clear();
                dataGridView1.ClearSelection();

                e.Handled = true;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textBox1.Text, out int idProducto) || idProducto <= 0)
            {
                MessageBox.Show("Selecciona un producto válido.");
                return;
            }

            if (!int.TryParse(textBox7.Text, out int cantidadASumar) || cantidadASumar <= 0)
            {
                MessageBox.Show("Ingresa una cantidad válida a sumar.");
                return;
            }

            clases.inventario objconexion = new clases.inventario();
            objconexion.sumarStock(idProducto, cantidadASumar);
            objconexion.mostrar(dataGridView2);
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            dataGridView1.DataSource = null;
            textBox7.Clear();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                textBox5.ReadOnly = false;
            }
            else
                textBox5.ReadOnly= true;
        }
    }
}
