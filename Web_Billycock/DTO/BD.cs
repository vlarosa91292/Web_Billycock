using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_Billycock.DTO
{
    public class BD
    {
        public string Server { get; set; }
        public string Database { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public bool MultipleActiveResultSets { get; set; }
        public bool PersistSecurityInfo { get; set; }
        public bool Encrypt { get; set; }
        public bool TrustServerCertificate { get; set; }
    }
}
