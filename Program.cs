using System.Diagnostics;
using AnalizzatoreTestoParallelo;

/// <summary>
/// Represents the main class of the program.
/// </summary>
public class Program
{
    /// <summary>
    /// Entry point of the program.
    /// </summary>
    /// <param name="args">Command line arguments.</param>
    public static void Main(string[] args)
    {
        Console.WriteLine("Benvenuto nell'analizzatore di testo!");

        while (true)
        {
            // Ottieni tutti i file disponibili nella cartella "Resources"
            var files = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources"), "*.txt")
                .Select(Path.GetFileName)
                .ToArray();

            if (files.Length == 0)
            {
                Console.WriteLine("Nessun file di testo disponibile nella cartella 'Resources'.");
                return;
            }

            // Mostra all'utente i file disponibili
            Console.WriteLine("File disponibili:");

            for (var i = 0; i < files.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {files[i]}");
            }

            Console.WriteLine("Inserisci il numero del file di testo da analizzare (o 'exit' per uscire):");
            var input = Console.ReadLine();

            if (input?.ToLower() == "exit")
            {
                break;
            }

            if (!int.TryParse(input, out var selectedIndex) || selectedIndex < 1 || selectedIndex > files.Length)
            {
                Console.WriteLine("Input non valido. Inserisci un numero valido o 'exit' per uscire.");
                continue;
            }

            var fileName = files[selectedIndex - 1]!;
            var filePath = Path.Combine("Resources", fileName);

            Console.WriteLine($"Analisi del file {fileName} in corso...");

            // Avvia l'analisi sia in modo seriale che parallelo
            TextAnalysisResult parallelResult = AnalyzeTextParallel(filePath);
            TextAnalysisResult nonParallelResult = AnalyzeTextNonParallel(filePath);

            // Visualizza i risultati
            Console.WriteLine("Risultati analisi:");
            Console.WriteLine($"Numero totale di parole: {nonParallelResult.WordCount}");
            Console.WriteLine($"Frequenza delle parole più comuni:");

            foreach (var pair in nonParallelResult.WordFrequency.OrderByDescending(kv => kv.Value).Take(10))
            {
                Console.WriteLine($"{pair.Key}: {pair.Value}");
            }

            Console.WriteLine($"Lunghezza media delle frasi: {nonParallelResult.AverageSentenceLength} parole");
            Console.WriteLine();

            // Confronta le prestazioni
            Console.WriteLine($"Prestazioni:");
            Console.WriteLine($"Tempo impiegato in modalità parallela: {parallelResult.ElapsedMilliseconds} ms");
            Console.WriteLine($"Tempo impiegato in modalità non-parallela: {nonParallelResult.ElapsedMilliseconds} ms");
            Console.WriteLine($"Speedup ottenuto: {nonParallelResult.ElapsedMilliseconds / (double)parallelResult.ElapsedMilliseconds}");

            Console.WriteLine("Premi INVIO per continuare...");
            Console.ReadLine();
        }
    }

    /// <summary>
    /// Represents the separator characters used for text analysis.
    /// </summary>
    private static readonly char[] Separator = [' ', '\n', '\r', '\t'];

    /// <summary>
    /// Represents the characters used to separate sentences in text analysis.
    /// </summary>
    private static readonly char[] AverageSentenceSeparator = ['.', '!', '?'];

    /// <summary>
    /// Analyzes the text found in the specified file path using a non-parallel approach.
    /// </summary>
    /// <param name="filePath">The path of the file to analyze.</param>
    /// <returns>A TextAnalysisResult object containing the analysis results.</returns>
    private static TextAnalysisResult AnalyzeTextNonParallel(string filePath)
    {
        var result = new TextAnalysisResult();

        // Legge il testo dal file
        var text = File.ReadAllText(filePath);

        // Avvia il cronometro per misurare il tempo di esecuzione
        var stopwatch = Stopwatch.StartNew();

        // Effettua l'analisi
        result.WordCount = text.Split(Separator, StringSplitOptions.RemoveEmptyEntries).Length;

        result.WordFrequency = text.Split(Separator, StringSplitOptions.RemoveEmptyEntries)
            .GroupBy(word => word.ToLower())
            .ToDictionary(group => group.Key, group => group.Count());

        result.AverageSentenceLength = text.Split(AverageSentenceSeparator, StringSplitOptions.RemoveEmptyEntries)
            .Select(sentence => sentence.Split(Separator, StringSplitOptions.RemoveEmptyEntries).Length)
            .DefaultIfEmpty(0)
            .Average();

        // Ferma il cronometro e registra il tempo trascorso
        stopwatch.Stop();
        result.ElapsedMilliseconds = stopwatch.ElapsedMilliseconds;

        return result;
    }

    /// <summary>
    /// Analyzes the text in parallel.
    /// </summary>
    /// <param name="filePath">The path of the file to be analyzed.</param>
    /// <returns>The result of the text analysis.</returns>
    private static TextAnalysisResult AnalyzeTextParallel(string filePath)
    {

        // Legge il testo dal file
        var text = File.ReadAllText(filePath);

        // Avvia il cronometro per misurare il tempo di esecuzione
        var stopwatch = Stopwatch.StartNew();

        // Effettua l'analisi in parallelo
        var wordCountTask = Task.Run(() => text.Split(Separator, StringSplitOptions.RemoveEmptyEntries).Length);

        var wordFrequencyTask = Task.Run(() => text.Split(Separator, StringSplitOptions.RemoveEmptyEntries)
            .AsParallel()
            .GroupBy(word => word.ToLower())
            .ToDictionary(group => group.Key, group => group.Count()));

        var sentenceLengthsTask = Task.Run(() => text.Split(AverageSentenceSeparator, StringSplitOptions.RemoveEmptyEntries)
            .AsParallel()
            .Select(sentence => sentence.Split(Separator, StringSplitOptions.RemoveEmptyEntries).Length)
            .DefaultIfEmpty(0)
            .Average());

        Task.WaitAll(wordCountTask, wordFrequencyTask, sentenceLengthsTask);
        stopwatch.Stop();

        return new TextAnalysisResult(wordCountTask.Result, wordFrequencyTask.Result, sentenceLengthsTask.Result, stopwatch.ElapsedMilliseconds);
    }
}