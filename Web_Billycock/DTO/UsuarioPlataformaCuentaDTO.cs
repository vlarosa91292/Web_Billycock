using Billycock.Models;
using System.Text.Json.Serialization;

namespace Billycock.DTO
{
    public class UsuarioPlataformaCuentaDTO:UsuarioPlataformaCuenta
    {
        [JsonIgnore]
        public string idUsuarioPlataformaCuenta { get; set; }
    }
}
