using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    static void Main()
    {
        Console.WriteLine("Benvenuto nell'analizzatore di testo!");

        while (true)
        {
            Console.WriteLine("Inserisci il percorso del file di testo da analizzare (o 'exit' per uscire):");
            string filePath = Console.ReadLine();

            if (filePath.ToLower() == "exit")
                break;

            if (!File.Exists(filePath))
            {
                Console.WriteLine("Il file specificato non esiste. Riprova.");
                continue;
            }

            Console.WriteLine("Analisi in corso...");
        }
    }
}