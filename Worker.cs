using Monitor.Helpers;
using System.Net;

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

            }


            await Task.Delay(60000, stoppingToken); //para rodar a cada minuto
        }
    }
}
