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
                for (int i = 0; i < 100; i++) // Ajuste o n�mero de itera��es conforme necess�rio
                {
                    // Aloca 500MB de mem�ria em cada itera��o
                    lists.Add(new byte[1024 * 1024 * 500]);
                    Thread.Sleep(1000); // Espera 1 segundo entre as aloca��es
                }

                // Mant�m a mem�ria alocada por um tempo antes de liber�-la
                Thread.Sleep(30000); // Manter a mem�ria alocada por 30 segundos
                lists.Clear(); // Libera a mem�ria
                GC.Collect(); // For�a a coleta de lixo para liberar mem�ria
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
