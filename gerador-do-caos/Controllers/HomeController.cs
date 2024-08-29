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

        public IActionResult Index()
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
        public IActionResult HighCpuUsage()
        {
            Task.Run(() =>
            {
                var watch = Stopwatch.StartNew();
                while (watch.ElapsedMilliseconds < 10000) // Estressar CPU por 10 segundos
                {
                    // Operação pesada na CPU
                    Math.Pow(2, 20);
                }
                watch.Stop();
            });

            return RedirectToAction("Index");
        }

        // Simular alto consumo de memória
        [HttpPost]
        public IActionResult HighMemoryUsage()
        {
            Task.Run(() =>
            {
                var lists = new List<byte[]>();
                for (int i = 0; i < 2; i++)
                {
                    lists.Add(new byte[1024 * 1024 * 500]);
                    Thread.Sleep(500); // Reduzir o delay
                }

                // Manter a memória alocada por menos tempo antes de liberá-la
                Thread.Sleep(3000); // Manter por 3 segundos


                lists.Clear(); // Libera a memória
                GC.Collect(); // Força a coleta de lixo para liberar memória
                GC.WaitForPendingFinalizers();
                GC.Collect();
            });

            return RedirectToAction("Index");
        }

        // Simular quebra da aplicação
        [HttpPost]
        public IActionResult CrashApplication()
        {
            Environment.FailFast("Simulando quebra da aplicação");
            return RedirectToAction("Index");
        }

        // Simular indisponibilidade temporária
        [HttpPost]
        public IActionResult TemporaryUnavailability()
        {
            Task.Run(() =>
            {
                Thread.Sleep(10000); // Tornar a aplicação indisponível por 10 segundos
            });

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
