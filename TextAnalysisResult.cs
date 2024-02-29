namespace AnalizzatoreTestoParallelo;

/// <summary>
/// Represents the result of text analysis.
/// </summary>
public class TextAnalysisResult
{
    /// <summary>
    /// Represents the result of text analysis.
    /// </summary>
    public TextAnalysisResult()
    {
        WordFrequency = new Dictionary<string, int>();
    }

    /// <summary>
    /// Represents the result of text analysis.
    /// </summary>
    public TextAnalysisResult(int wordCount, Dictionary<string, int> wordFrequency, double averageSentenceLength, long elapsedMilliseconds)
    {
        WordCount = wordCount;
        WordFrequency = wordFrequency;
        AverageSentenceLength = averageSentenceLength;
        ElapsedMilliseconds = elapsedMilliseconds;
    }

    /// <summary>
    /// Represents the word count of a text analysis result.
    /// </summary>
    public int WordCount { get; set; }

    /// <summary>
    /// Represents the word frequency analysis result.
    /// </summary>
    public Dictionary<string, int> WordFrequency { get; set; }

    /// <summary>
    /// Represents the average length of a sentence in a text analysis result.
    /// </summary>
    public double AverageSentenceLength { get; set; }

    /// <summary>
    /// Gets or sets the number of milliseconds it took to perform the analysis.
    /// </summary>
    public long ElapsedMilliseconds { get; set; }
}