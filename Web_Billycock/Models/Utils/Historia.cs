using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Billycock.Models
{
    public class Historia
    {
        [Key]
        public int idHistory {get;set;}
        public string Request { get; set; }
        public string Response { get; set; }
        public DateTime fecha { get; set; }
        public string integracion { get; set; }
    }
}
