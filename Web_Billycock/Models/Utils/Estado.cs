using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Billycock.Models
{
    public class Estado
    {
        [Key]
        public int idEstado { get; set; }
        public string descripcion { get; set; }
    }
}
