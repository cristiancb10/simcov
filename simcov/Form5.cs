using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace simcov
{
    public partial class Form5 : Form
    {
        public Form5()
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

            clases.clientes objconexion = new clases.clientes();
            objconexion.mostrar(dataGridView2);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            dataGridView1.DataSource = null;
            dataGridView2.DataSource = null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();  
            
            clases.clientes objconexion = new clases.clientes();
            if (!objconexion.validarCliente(textBox2.Text.Trim()))
            {
                bool exito = objconexion.insertar(textBox2, textBox3, textBox4);

                if (exito)
                {
                    textBox1.Clear();
                    textBox2.Clear();
                    textBox3.Clear();
                    textBox4.Clear();

                    objconexion.mostrar(dataGridView2);
                }
            }

            else
            {
                MessageBox.Show("El nombre de cliente ya existe");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("No a digitado ningun nombre para buscar");
                return;
            }
            else
            {
                dataGridView1.ClearSelection();
                clases.clientes objconexion = new clases.clientes();
                objconexion.buscarCliente(dataGridView1, textBox2);
            }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            clases.clientes objconexion = new clases.clientes();
            objconexion.seleccionarCliente(dataGridView1, textBox1, textBox2, textBox3, textBox4);
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            dataGridView1.ClearSelection();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            clases.clientes objconexion = new clases.clientes();
            bool actualizado = objconexion.actualizar(textBox1, textBox2, textBox3, textBox4);

            if (actualizado)
            {
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
            }

            objconexion.mostrar(dataGridView2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (clases.SesionUsuario.Rol != "administrador")
            {
                MessageBox.Show("Solo los administradores pueden eliminar clientes.", "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            clases.clientes objconexion = new clases.clientes();
            objconexion.eliminar(textBox1);
            objconexion.mostrar(dataGridView2);
        }

        private void dataGridView2_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            clases.clientes objconexion = new clases.clientes();
            objconexion.seleccionar(dataGridView2, textBox1, textBox2, textBox3, textBox4);

        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            this.AcceptButton = button6;
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            this.AcceptButton = button6;
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            this.dataGridView1.Font = new Font("Segoe UI", 10.8f, FontStyle.Regular);
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10.8f, FontStyle.Bold);
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && dataGridView1.CurrentRow != null)
            {
                DataGridViewRow row = dataGridView1.CurrentRow;
                textBox1.Text = row.Cells["id_cliente"].Value.ToString();
                textBox2.Text = row.Cells["nombre"].Value.ToString();
                textBox3.Text = row.Cells["telefono"].Value.ToString();
                textBox4.Text = row.Cells["direccion"].Value.ToString();

                dataGridView1.DataSource = null;
                dataGridView1.Rows.Clear();
                dataGridView1.ClearSelection();

                e.Handled = true;
            }
        }
    }
}
