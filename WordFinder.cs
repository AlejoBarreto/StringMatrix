namespace StringMatrix;
public class WordFinder
{
    public char[][] _matrix;
    private IEnumerable<string> HorizontalStrings;
    private IEnumerable<string> VerticalStrings;
    public WordFinder(IEnumerable<string> matrix)
    {
        int rowsSize;
        int columnsSize;
        rowsSize = columnsSize = matrix.Count();
        HorizontalStrings = matrix;

        // Check if collumns and rows have the same size.
        if (rowsSize > 64 || matrix.Any(row => row.Length != rowsSize))
            throw new ArgumentException("Collumns and rows must have the same length with a maximum size of 64x64.");

        //Good way for legibilty but is not the best for performance.
        //_matrix = matrix.Select(row => row.ToCharArray()).ToArray();

        // Initializing matrix
        _matrix = new char[columnsSize][];
        for (int col = 0; col < columnsSize; col++)
        {
            _matrix[col] = new char[rowsSize];
        }

        for (int rowIndex = 0; rowIndex < rowsSize; rowIndex++)
        {
            // ElementAt is expensive, so we get the row and we access to each element with index directly
            var row = matrix.ElementAt(rowIndex);

            // To take adventage of the iteration, the value of the strings are assigned to columns.
            for (int columnIndex = 0; columnIndex < columnsSize; columnIndex++)
            {
                _matrix[columnIndex][rowIndex] = row[columnIndex];
            }
        }

        VerticalStrings = GetVerticalStrings(_matrix);
    }

    public IEnumerable<string> Find(IEnumerable<string> wordStream)
    {
        var wordsCount = new Dictionary<string, int>();

        //Using HashSet to obtain only unique words.
        var uniqueWords = new HashSet<string>(wordStream);

        foreach (var word in uniqueWords)
        {
            var occurrences = FindWordOcurrences(word);
            if (occurrences > 0)
            {
                wordsCount[word] = occurrences;
            }
        }

        if (wordsCount.Count == 0)
            return Enumerable.Empty<string>();

        return wordsCount
            .OrderByDescending(x => x.Value)
            .Take(10) // Take the 10 most repeated words
            .Select(x => x.Key);
    }

    public int FindWordOcurrences(string word)
    {
        var occurrencesCounter = 0;

        foreach (var horizontalString in HorizontalStrings)
        {
            occurrencesCounter += CountOccurrences(horizontalString, word);
        }

        foreach (var verticalString in VerticalStrings)
        {
            occurrencesCounter += CountOccurrences(verticalString, word);
        }

        return occurrencesCounter;
    }

    private int CountOccurrences(string source, string word)
    {
        int count = 0;
        int index = 0;

        // Look for occurrences into string
        while ((index = source.IndexOf(word, index, StringComparison.OrdinalIgnoreCase)) != -1)
        {
            count++;
            index++; // When found, start to search again from the index.
        }

        return count;
    }

    public IEnumerable<string> GetVerticalStrings(char[][] _matrix)
    {
        var verticalStrings = new List<string>();

        foreach (var verticalString in _matrix)
        {
            verticalStrings.Add(new string(verticalString));
        }

        return verticalStrings;
    }
}
