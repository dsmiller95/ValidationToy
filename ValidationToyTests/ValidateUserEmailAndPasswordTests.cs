using ValidationToy;
using ValidationToy.Requests;
using ValidationToy.Validated;
using static ValidationToyTests.TestUtilities;

namespace ValidationToyTests;

public class ValidateUserEmailAndPasswordTests
{
    [SetUp]
    public void Setup()
    {
        
    }

    [Test]
    public void ValidateUser_WithMissingEmail_Fails()
    {
        var createUser = GetValidCreateUser();
        createUser.Email = null;

        var validator = GetValidator();
        var validationResult = validator.Validate(createUser);
        
        AssertFailedWithMessage(validationResult, "Email is missing.");
    }

    [Test]
    public void ValidateUser_WithValidEmail_Succeed()
    {
        var createUser = GetValidCreateUser();
        createUser.Email = "Someemal@home.com";

        var validator = GetValidator();
        var validationResult = validator.Validate(createUser);
        
        AssertSuccess(validationResult, succ =>
        {
            Assert.AreEqual(createUser.Email, succ.Email.Value);
        });
    }
    
    
    [Test]
    public void ValidateUser_WithMissingPassword_Fails()
    {
        var createUser = GetValidCreateUser();
        createUser.Password = null;

        var validator = GetValidator();
        var validationResult = validator.Validate(createUser);
        
        AssertFailedWithMessage(validationResult, "Password is missing.");
    }

    private DemoValidator GetValidator()
    {
        return new DemoValidator();
    }
}