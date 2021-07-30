using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Billycock.Models
{
    public class PlataformaCuenta
    {
        public int? usuariosdisponibles { get; set; }
        public string fechaPago { get; set; }
        public string clave { get; set; }
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
