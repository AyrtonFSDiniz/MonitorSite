using Monitor.Helpers;
using System.Net;
using System.Net.Mail;
using static Monitor.EnvioEmailService;

namespace Monitor;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly Sites _sites;

    public Worker(ILogger<Worker> logger, IConfiguration _conf)
    {
        _logger = logger;
        _sites = _conf.GetSection("Sites").Get<Sites>();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            int timerDeVerificacao = 60000;
            
            HttpStatusCode status = await Requesters.GetStatusFromUrl(_sites.Url);
            Console.WriteLine("######## =================== ########");
            Console.WriteLine("######## O site está rodando ########");
            Console.WriteLine("######## =================== ########");

            if (status != HttpStatusCode.OK)
            {
                string nameFile = string.Format("logfile_{0}.txt", DateTime.Now.ToString("ddMMyyyy"));
                string path = Path.Combine(@"C:\Users\ayrton.diniz\Desktop\Treino .NET\MonitorSite", nameFile);
                StreamWriter logFile = new StreamWriter(path, true);
                logFile.WriteLine(string.Format("O Site {0} ficou fora do ar em {1}.", _sites.Url, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")));
                logFile.Close();

                _logger.LogInformation("Worker rodando em: {time}", DateTimeOffset.Now);


                Console.WriteLine("Enviando e-mail...");
                EmailService email = new();
                EmailMessage msg = new();

                msg.Body = @$"<table align='center'>
                           <tr>
        
                            <th bgcolor = 'RoyalBlue'><font color='white'><b> Vendedor </font></b></th>
                            </table>";

                msg.IsHtml = true;
                msg.Subject = "Seu site está fora do ar!";
                msg.ToEmail = "ayrtondefreitassilva@gmail.com";
                email.SendEmailMessage(msg);
            }

            await Task.Delay(timerDeVerificacao, stoppingToken);
        }
    }
}
