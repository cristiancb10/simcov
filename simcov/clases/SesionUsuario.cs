using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace simcov.clases
{
    public class SesionUsuario
    {
        public static int IdUsuario { get; set; }
        public static string NombreUsuario { get; set; }
        public static string Rol { get; set; }
        public static string ContraseñaHash { get; set; }
        public static Form FormularioValidacion { get; set; }
    }
}
