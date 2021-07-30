using Billycock.Models;
using System.Text.Json.Serialization;

namespace Billycock.DTO
{
    public class PlataformaDTO:Plataforma
    {
        [JsonIgnore]
        public string descEstado { get; set; }
    }
}
