using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Billycock.Models
{
    public class Usuario
    {
        [Key]
        public int idUsuario { get; set; }
        public string descripcion { get; set; }
        public DateTime? fechaInscripcion { get; set; }
        [JsonIgnore]
        public int? idEstado { get; set; }
        public string facturacion { get; set; }
        public int? pago { get; set; }
        public string pin { get; set; }
        [JsonIgnore]
        public List<UsuarioPlataformaCuenta> usuarioPlataformaCuentas { get; set; }
    }
}
