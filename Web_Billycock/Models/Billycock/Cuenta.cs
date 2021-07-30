using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Billycock.Models
{
    public class Cuenta
    {
        [Key]
        [JsonIgnore]
        public int idCuenta { get; set; }
        public string correo { get; set; }
        public string diminutivo { get; set; }
        [JsonIgnore]
        public int idEstado { get; set; }
        [JsonIgnore]
        public List<UsuarioPlataformaCuenta> usuarioPlataformaCuentas { get; set; }
        [JsonIgnore]
        public List<PlataformaCuenta> plataformaCuentas { get; set; }
    }
}
