using gerador_do_caos.Controllers;

namespace gerador_do_caos;

public class LargeObject : IDisposable
{
    private byte[] data;


    public LargeObject()
    {
        data = new byte[1024 * 1024 * 500];
      
    }

    public void DoSomething(int i)
    {
        // mostrar no console log
        Console.WriteLine($"Iteração {i}");

    }

    public void Dispose()
    {
        // Liberando recursos não gerenciados
        data = null;
        GC.Collect(); // Forçando a coleta de lixo (use com cautela)
    }
}