using Billycock.Models;
using Billycock.Models.Hilario;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ModernHttpClient;
using Newtonsoft.Json;
using Polly;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Web_Billycock.Repositories.Utils;

namespace Billycock.Data
{
    public class HilarioServiceContext : DbContext
    {
        public HilarioServiceContext(IConfiguration configuration) : base(GetOptions(configuration))
        {
        }

        private static DbContextOptions GetOptions(IConfiguration configuration)
        {
            string urlBase = configuration["url_Base"];
            BD bd = new BD();
            HttpClient client;
            Task<HttpResponseMessage> res;
            CancellationToken cancellationToken = CancellationToken.None;
            var policyRetry = Policy.Handle<HttpRequestException>().Retry(6);
            var policyTimeout = Policy.Timeout(15);
            var policyWrap = Policy.Wrap(policyRetry, policyTimeout);
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            try
            {
                using (client = new HttpClient(new NativeMessageHandler()))
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));
                    res = policyWrap.Execute(ct => client.GetAsync(urlBase + "H"), cancellationToken);
                    res.Wait();
                    if (res.IsCompleted)
                    {
                        var responseContent = res.Result.Content;

                        // by calling .Result you are synchronously reading the result
                        string responseString = responseContent.ReadAsStringAsync().Result;
                        bd = JsonConvert.DeserializeObject<BD>(responseString);

                        builder = new SqlConnectionStringBuilder()
                        {
                            DataSource = bd.Server,
                            InitialCatalog = bd.Database,
                            UserID = bd.UserId,
                            Password = bd.Password,
                            ApplicationName = "Web_Billycock"
                        };
                        if (builder.UserID != "sa")
                        {
                            builder.MultipleActiveResultSets = true;
                            builder.PersistSecurityInfo = false;
                            builder.Encrypt = true;
                            builder.TrustServerCertificate = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                /*res.Result.ReasonPhrase = ex.Message;
                Console.WriteLine("Error " + res + " Error " +
                ex.ToString());*/
            }

            return SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder(), builder.ConnectionString).Options;
        }

        public DbSet<Producto> PRODUCTO { get; set; }
        public DbSet<Proveedor> PROVEEDOR { get; set; }
        public DbSet<Oferta> OFERTA { get; set; }
        public DbSet<Linea> LINEA { get; set; }
        //public DbSet<Venta.Cabecera> VENTA_CABECERA { get; set; }
        //public DbSet<Venta.Detalle> VENTA_DETALLE { get; set; }

        public DbSet<Historia> HISTORIA { get; set; }
        public DbSet<Estado> ESTADO { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}
