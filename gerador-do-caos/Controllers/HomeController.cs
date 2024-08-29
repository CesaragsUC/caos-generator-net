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
        public IActionResult HighCpuUsage()
        {
            Task.Run(() =>
            {
                var watch = Stopwatch.StartNew();
                while (watch.ElapsedMilliseconds < 10000) // Estressar CPU por 10 segundos
                {
                    // Opera��o pesada na CPU
                    Math.Pow(2, 20);
                }
                watch.Stop();
            });

            return RedirectToAction("Index");
        }

        // Simular alto consumo de mem�ria
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

                // Manter a mem�ria alocada por menos tempo antes de liber�-la
                Thread.Sleep(3000); // Manter por 3 segundos


                lists.Clear(); // Libera a mem�ria
                GC.Collect(); // For�a a coleta de lixo para liberar mem�ria
                GC.WaitForPendingFinalizers();
                GC.Collect();
            });

            return RedirectToAction("Index");
        }

        // Simular quebra da aplica��o
        [HttpPost]
        public IActionResult CrashApplication()
        {
            Environment.FailFast("Simulando quebra da aplica��o");
            return RedirectToAction("Index");
        }

        // Simular indisponibilidade tempor�ria
        [HttpPost]
        public IActionResult TemporaryUnavailability()
        {
            Task.Run(() =>
            {
                Thread.Sleep(10000); // Tornar a aplica��o indispon�vel por 10 segundos
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
