using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Billycock.Models
{
    public class UsuarioPlataformaCuenta
    {
        public int? cantidad { get; set; }
        //Usuario
        [ForeignKey("Usuario")]
        public int idUsuario { get; set; }
        [JsonIgnore]
        public Usuario Usuario { get; set; }
        //PLATAFORMA
        [ForeignKey("Plataforma")]
        public int idPlataforma { get; set; }
        [JsonIgnore]
        public Plataforma Plataforma { get; set; }
        //CUENTA
        [ForeignKey("Cuenta")]
        public int idCuenta { get; set; }
        [JsonIgnore]
        public Cuenta Cuenta { get; set; }
    }
}
