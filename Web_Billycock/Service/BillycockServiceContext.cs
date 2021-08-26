using Billycock.Models;
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
using Web_Billycock.DTO;

namespace Billycock.Data
{
    public class BillycockServiceContext: DbContext
    {
        public BillycockServiceContext(IConfiguration configuration) : base(GetOptions(configuration))
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
                    res = policyWrap.Execute(ct => client.GetAsync(urlBase + "B"), cancellationToken);
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
                            ApplicationName = "Web_Billycock",
                            MultipleActiveResultSets = bd.MultipleActiveResultSets,
                            PersistSecurityInfo = bd.PersistSecurityInfo,
                            Encrypt = bd.PersistSecurityInfo,
                            TrustServerCertificate = bd.PersistSecurityInfo
                        };
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

        public DbSet<Usuario> USUARIO { get; set; }
        public DbSet<Plataforma> PLATAFORMA { get; set; }
        public DbSet<Estado> ESTADO { get; set; }
        public DbSet<Cuenta> CUENTA { get; set; }
        public DbSet<UsuarioPlataformaCuenta> USUARIOPLATAFORMACUENTA { get; set; }
        public DbSet<PlataformaCuenta> PLATAFORMACUENTA { get; set; }
        public DbSet<Historia> HISTORIA { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
