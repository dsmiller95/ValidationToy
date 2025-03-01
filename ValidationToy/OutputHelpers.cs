namespace ValidationToy;

public static class OutputHelpers
{
    public static IEnumerable<string> Indent(this IEnumerable<string> source, int indentation)
    {
        return source.Select(s => new string(' ', indentation) + s);
    }
    public static string LinesIndented(this IEnumerable<string> source, int indentation)
    {
        var indented = source.Select(s => new string(' ', indentation) + s);
        return string.Join(Environment.NewLine, indented);
    }
    
    public static bool ErrorsMatch(string[] expected, string[] actual)
    {
        if (expected.Length != actual.Length)
        {
            return false;
        }

        var expectedList = new List<string>(expected);
        foreach (string actualError in actual)
        {
            var matchingExpected = expectedList.FirstOrDefault(e => ErrorMatches(e, actualError));
            if (matchingExpected != null)
            {
                expectedList.Remove(matchingExpected);
            }
            else
            {
                return false;
            }
        }

        return expectedList.Count == 0;
    }

    private static bool ErrorMatches(string expectedError, string actualError)
    {
        return actualError.ToLower().Contains(expectedError.ToLower());
    } 
}