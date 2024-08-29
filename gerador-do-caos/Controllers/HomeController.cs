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

            // Obtém o nome do nó Kubernetes (host) da variável de ambiente
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("HOSTNAME")))
            {
                // Se não estiver no Kubernetes, retorna o nome da máquina local
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
                // Operação pesada na CPU
                Math.Pow(2, 20);
            }
            watch.Stop();

            return RedirectToAction("Index");
        }

        // Simular alto consumo de memória
        [HttpPost]
        public async Task<IActionResult> HighMemoryUsage()
        {
            int maxIterations = 2; // Ajuste o número de iterações conforme necessário

            for (int i = 0; i < maxIterations; i++)
            {
                using (var largeObject = new LargeObject())
                {
                    // Simulando uma operação que utiliza o objeto
                    largeObject.DoSomething(i);

                    Thread.Sleep(1000); // Aguarda 1 segundo
                }

                _logger.LogWarning($"Iteração {i} concluída.");

            }

            GC.Collect(); 
            GC.WaitForPendingFinalizers(); 
            GC.Collect(); 

            return RedirectToAction("Index");
        }

        // Simular quebra da aplicação
        [HttpPost]
        public IActionResult CrashApplication()
        {
            Environment.FailFast("Simulando quebra da aplicação");
            return RedirectToAction("Index");
        }

    }
}
