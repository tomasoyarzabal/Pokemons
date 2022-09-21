using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace dominio1
{
   public class Pokemon
    {
        public int Id { get; set; }
        [DisplayName("Numero")]
        public int Numero { get; set; }
        public string Nombre { get; set; }
        [DisplayName("Descripcion")]
        public string Descripcion { get; set; }
        public string UrlImagen { get; set; }
        public Elemento Tipo { get; set; }
        public Elemento Debilidad { get; set; }
    }
}
