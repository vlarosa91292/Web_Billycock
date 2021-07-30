using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Billycock.Models.Hilario
{
    public class Oferta
    {
        [Key]
        public int idOferta { get; set; }
        public string codigoBarra { get; set; }
        [NotMapped]
        public string descProducto { get; set; }
        public string descripcion { get; set; }
        public double precioOferta { get; set; }
        public int cantidad { get; set; }
        public int idEstado { get; set; }
        [NotMapped]
        public string descEstado { get; set; }
    }
}
