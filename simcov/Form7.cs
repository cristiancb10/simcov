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
    public partial class Form7 : Form
    {
        public string UsuarioIngresado { get; private set; }
        public string ContraseñaIngresada { get; private set; }

        public Form7()
        {
            InitializeComponent();
            // Registrar esta instancia para poder cerrarla desde fuera (por ejemplo al cerrar sesión)
            clases.SesionUsuario.FormularioValidacion = this;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Validar con el usuario logueado
            string usuario = textBox1.Text.Trim();
            string contraseña = textBox2.Text.Trim();

            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(contraseña))
            {
                MessageBox.Show("Debe ingresar el usuario y la contraseña.");
                return;
            }

            UsuarioIngresado = usuario;
            ContraseñaIngresada = contraseña;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            // Limpiar la referencia cuando se cierre el formulario
            clases.SesionUsuario.FormularioValidacion = null;
            base.OnFormClosed(e);
        }
    }
}
