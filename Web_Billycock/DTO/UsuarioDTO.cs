using Billycock.Models;
using System.Text.Json.Serialization;

namespace Billycock.DTO
{
    public class UsuarioDTO:Usuario
    {
        public int netflix { get; set; }
        public int amazon { get; set; }
        public int disney { get; set; }
        public int hbo { get; set; }
        public int youtube { get; set; }
        public int spotify { get; set; }
        [JsonIgnore]
        public string descEstado { get; set; }
    }
}
