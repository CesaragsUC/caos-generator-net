using gerador_do_caos.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace gerador_do_caos.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            string hostName = string.Empty;

            // Obt�m o nome do n� Kubernetes (host) da vari�vel de ambiente
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("HOSTNAME")))
            {
                // Se n�o estiver no Kubernetes, retorna o nome da m�quina local
                hostName = Environment.MachineName;
            }
            else
            {
                hostName = Environment.GetEnvironmentVariable("HOSTNAME");
            }

            ViewBag.HostName = hostName;

            return View();
        }

        // Simular alto uso de CPU
        [HttpPost]
        public async Task<IActionResult> HighCpuUsage()
        {
            var watch = Stopwatch.StartNew();
            while (watch.ElapsedMilliseconds < 10000) // Estressar CPU por 10 segundos
            {
                // Opera��o pesada na CPU
                Math.Pow(2, 20);
            }
            watch.Stop();

            return RedirectToAction("Index");
        }

        // Simular alto consumo de mem�ria
        [HttpPost]
        public async Task<IActionResult> HighMemoryUsage()
        {
            int maxIterations = 2; // Ajuste o n�mero de itera��es conforme necess�rio

            for (int i = 0; i < maxIterations; i++)
            {
                using (var largeObject = new LargeObject())
                {
                    // Simulando uma opera��o que utiliza o objeto
                    largeObject.DoSomething(i);

                    Thread.Sleep(1000); // Aguarda 1 segundo
                }

                _logger.LogWarning($"Itera��o {i} conclu�da.");

            }

            GC.Collect(); 
            GC.WaitForPendingFinalizers(); 
            GC.Collect(); 

            return RedirectToAction("Index");
        }

        // Simular quebra da aplica��o
        [HttpPost]
        public IActionResult CrashApplication()
        {
            Environment.FailFast("Simulando quebra da aplica��o");
            return RedirectToAction("Index");
        }

    }
}
