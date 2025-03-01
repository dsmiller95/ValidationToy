// See https://aka.ms/new-console-template for more information

using System.Text;
using System.Text.Json;
using DmansValidator;
using ValidationToy;

Console.WriteLine("Hello, World!");

var jsonSettings = new JsonSerializerOptions(JsonSerializerDefaults.Web);
var testCases = JsonSerializer.Deserialize<ValidationTestCaseExamplesFile>(
    File.ReadAllText("ValidationExamples.json"), jsonSettings);

if (testCases == null)
{
    Console.Error.WriteLine("Failed to parse test cases.");
    return;
}

var validationService = new ValidationService();
var validator = new CreateUserValidator(validationService);

var divider = "==================================";
foreach (var testCase in testCases.Cases)
{
    Console.WriteLine(divider);
    Console.WriteLine(testCase.Name);
    Console.WriteLine(divider + "\n");
    Console.WriteLine("input:\n" + testCase.Input + "\n");
    
    var result = validator.Validate(testCase.Input);
    if (result.IsSuccess)
    {
        if (testCase.Expected.Errors.Length == 0)
        {
            Console.WriteLine($"Got no errors, expected no errors.");
        }
        else
        {
            Console.WriteLine($"Got not errors, but expected errors:\n{testCase.Expected.Errors.LinesIndented(4)}");
        }
    }
    else
    {
        var errors = result.Error.Select(e => e.Message).ToArray();
        if (ErrorsMatch(testCase.Expected.Errors, errors))
        {
            Console.WriteLine($"Got errors:\n{errors.LinesIndented(4)}");
        }
        else
        {
            Console.WriteLine($"Got errors:\n{errors.LinesIndented(4)}\nbut expected errors:\n{testCase.Expected.Errors.LinesIndented(4)}");
        }
    }
    Console.WriteLine("\n");
}

bool ErrorsMatch(string[] expected, string[] actual)
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

bool ErrorMatches(string expectedError, string actualError)
{
    return actualError.ToLower().Contains(expectedError.ToLower());
}