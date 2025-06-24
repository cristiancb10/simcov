using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace simcov
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            this.Opacity = 0;
            this.Shown += Form2_Shown;
            this.BackColor = Color.FromArgb(44, 47, 51);
            this.DoubleBuffered = true;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            foreach (Control control in this.Controls)
            {
                if (control is MdiClient)
                {
                    control.BackColor = Color.FromArgb(44, 47, 51); 
                    break;
                }
            }

            if (clases.SesionUsuario.Rol != "administrador")
            {
                agregarUsuarioToolStripMenuItem.Visible = false;
                inventarioToolStripMenuItem.Visible = false;
                registroDeVentasToolStripMenuItem.Visible = false;
            }
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ventasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 formAbierto3 = this.MdiChildren.OfType<Form3>().FirstOrDefault();

            if (formAbierto3 == null)
            {
                Form3 obj3 = new Form3();
                obj3.MdiParent = this;
                obj3.Show();
            }
            else
            {
                formAbierto3.BringToFront();
            }
        }

        private void inventarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form4 formAbierto4 = this.MdiChildren.OfType<Form4>().FirstOrDefault();

            if (formAbierto4 == null)
            {
                Form4 obj4 = new Form4();
                obj4.MdiParent = this;
                obj4.Show();
            }
            else
            {
                formAbierto4.BringToFront();
            }
        }

        private void clientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form5 formAbierto5 = this.MdiChildren.OfType<Form5>().FirstOrDefault();

            if (formAbierto5 == null)
            {
                Form5 obj5 = new Form5();
                obj5.MdiParent = this;
                obj5.Show();
            }
            else
            {
                formAbierto5.BringToFront();
            }
        }

        private void reportesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form6 formAbierto6 = this.MdiChildren.OfType<Form6>().FirstOrDefault();

            if (formAbierto6 == null)
            {
                Form6 obj6 = new Form6();
                obj6.MdiParent = this;
                obj6.Show();
            }
            else
            {
                formAbierto6.BringToFront();
            }

        }

        private void Form2_Shown(object sender, EventArgs e)
        {
            this.Opacity = 1;
        }

        private void agregarUsuarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form9 formAbierto9 = this.MdiChildren.OfType<Form9>().FirstOrDefault();

            if (formAbierto9 == null)
            {
                Form9 obj9 = new Form9();
                obj9.MdiParent = this;
                obj9.Show();
            }
            else
            {
                formAbierto9.BringToFront();
            }
        }

        private void registroDeVentasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form8 formAbierto8 = this.MdiChildren.OfType<Form8>().FirstOrDefault();

            if (formAbierto8 == null)
            {
                Form8 obj8 = new Form8();
                obj8.MdiParent = this;
                obj8.Show();
            }
            else
            {
                formAbierto8.BringToFront();
            }
        }

        private void cerrarSesiónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clases.SesionUsuario.IdUsuario = 0;
            clases.SesionUsuario.NombreUsuario = null;
            clases.SesionUsuario.Rol = null;

            // Cerrar todos los formularios hijos abiertos si hay
            foreach (Form child in this.MdiChildren)
            {
                child.Close();
            }

            // Cerrar el formulario de validación si estaba abierto
            if (clases.SesionUsuario.FormularioValidacion != null &&
                !clases.SesionUsuario.FormularioValidacion.IsDisposed)
            {
                clases.SesionUsuario.FormularioValidacion.Close();
                clases.SesionUsuario.FormularioValidacion = null;
            }

            // Volver al formulario de login
            if (Application.OpenForms["Form1"] is Form1 login)
            {
                login.LimpiarCamposLogin(); 
                login.Show();
                login.BringToFront();
            }

            // Ocultar el menú principal actual
            this.Hide();
        }

        private void actualizarContraseñaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form10 formAbierto10 = this.MdiChildren.OfType<Form10>().FirstOrDefault();

            if (formAbierto10 == null)
            {
                Form10 obj10 = new Form10();
                obj10.MdiParent = this;
                obj10.Show();
            }
            else
            {
                formAbierto10.BringToFront();
            }
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
