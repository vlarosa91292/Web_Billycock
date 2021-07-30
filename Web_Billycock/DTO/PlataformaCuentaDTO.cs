using Billycock.Models;
using System.Text.Json.Serialization;

namespace Billycock.DTO
{
    public class PlataformaCuentaDTO:PlataformaCuenta
    {
        [JsonIgnore]
        public string idPlataformaCuenta { get; set; }
    }
}
