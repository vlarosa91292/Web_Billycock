using Billycock.Models;
using System.Text.Json.Serialization;

namespace Billycock.DTO
{
    public class CuentaDTO:Cuenta
    {
        public bool netflix { get; set; }
        public bool amazon { get; set; }
        public bool disney { get; set; }
        public bool hbo { get; set; }
        public bool youtube { get; set; }
        public bool spotify { get; set; }
        [JsonIgnore]
        public string descEstado { get; set; }
    }
}
