using ValidationToy.Requests;

namespace ValidationToy;

public class ValidationTestCaseExamplesFile
{
    public required ValidationTestCaseExample[] Cases { get; set; }
}

public class ValidationTestCaseExample
{
    public required string Name { get; set; }
    public required CreateUser Input { get; set; }
    public required Expected Expected { get; set; }
}

public class Expected
{
    public required string[] Errors { get; set; }
}