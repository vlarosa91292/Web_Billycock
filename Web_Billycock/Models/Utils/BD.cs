using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_Billycock.Repositories.Utils
{
    public class BD
    {
        //Server=myServerName\myInstanceName;Database=myDataBase;User Id=myUsername;Password=myPassword;
        public string Server { get; set; }
        public string Database { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public string Others { get; set; }
    }
}
