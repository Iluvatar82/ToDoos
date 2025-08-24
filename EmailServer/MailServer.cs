using SmtpServer;
using SmtpServer.ComponentModel;
using System.Security.Cryptography.X509Certificates;

namespace EmailServer
{
    public class MailServer
    {
        private SmtpServer.SmtpServer _smtpServer {  get; set; }

        public MailServer()
        {
            var options = new SmtpServerOptionsBuilder()
                .ServerName("smtp.todoos.net")
                .Endpoint(builder =>
                    builder
                        .Port(9025, true)
                        .AllowUnsecureAuthentication(false)
                        .Certificate(CreateCertificate()))
                .Build();

            _smtpServer = new SmtpServer.SmtpServer(options, ServiceProvider.Default);
            //Start().Wait();
        }

        static X509Certificate2 CreateCertificate()
        {
            var certificate = File.ReadAllBytes(@"..\EmailServer\Certificate\todoos.net.pfx");
            return new X509Certificate2(certificate, "*");
        }

        public async Task Start()
        {
            //Task.Run(async () => await _smtpServer.StartAsync(CancellationToken.None));
            //await _smtpServer.StartAsync(CancellationToken.None);
        }
    }
}
