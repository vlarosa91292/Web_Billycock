using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Billycock.Models.Hilario
{
    public class Proveedor
    {
        [Key]
        public int idProveedor { get; set; }
        public string descripcion { get; set; }
        public List<Producto> productos { get; set; }
    }
}
