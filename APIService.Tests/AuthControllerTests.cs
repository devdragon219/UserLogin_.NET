using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;
using APIService.Controllers;
using APIService.Models;
using System.Threading.Tasks;

public class AuthControllerTests
{
    [Fact]
    public async Task Test_Login_Returns_NotFound_On_UserNotFound()
    {
        var mockUserManager = new Mock<UserManager<IdentityUser>>(
            new Mock<IUserStore<IdentityUser>>().Object,
            null, null, null, null, null, null, null, null);

        var mockSignInManager = new Mock<SignInManager<IdentityUser>>(
            mockUserManager.Object,
            new Mock<IHttpContextAccessor>().Object,
            new Mock<IUserClaimsPrincipalFactory<IdentityUser>>().Object,
            null, null, null, null);

        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x[It.IsAny<string>()]).Returns("SuperSecretSecurityKeyThatIs32Chars");

        var controller = new AuthController(
            mockUserManager.Object,
            mockSignInManager.Object,
            mockConfiguration.Object);

        mockUserManager
            .Setup(x => x.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((IdentityUser)null);

        var result = await controller.Login(new LoginRequest { Username = "test", Password = "somepassword" });

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("{ Message = User not found. }", notFoundResult.Value.ToString());
    }

    [Fact]
    public async Task Test_Login_Returns_Unauthorized_On_InvalidPassword()
    {
        var mockUserManager = new Mock<UserManager<IdentityUser>>(
            new Mock<IUserStore<IdentityUser>>().Object,
            null, null, null, null, null, null, null, null);

        var mockSignInManager = new Mock<SignInManager<IdentityUser>>(
            mockUserManager.Object,
            new Mock<IHttpContextAccessor>().Object,
            new Mock<IUserClaimsPrincipalFactory<IdentityUser>>().Object,
            null, null, null, null);

        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x[It.IsAny<string>()]).Returns("SuperSecretSecurityKeyThatIs32Chars");

        var controller = new AuthController(
            mockUserManager.Object,
            mockSignInManager.Object,
            mockConfiguration.Object);

        var user = new IdentityUser { UserName = "admin" };
        mockUserManager
            .Setup(x => x.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        mockUserManager
            .Setup(x => x.CheckPasswordAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
            .ReturnsAsync(false);

        var result = await controller.Login(new LoginRequest { Username = "admin", Password = "wrongpassword" });

        var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
        Assert.Equal("{ Message = Invalid Password. }", unauthorizedResult.Value.ToString());
    }

    [Fact]
    public async Task Test_Login_Returns_Ok_On_SuccessfulLogin()
    {
        var mockUserManager = new Mock<UserManager<IdentityUser>>(
            new Mock<IUserStore<IdentityUser>>().Object,
            null, null, null, null, null, null, null, null);

        var mockSignInManager = new Mock<SignInManager<IdentityUser>>(
            mockUserManager.Object,
            new Mock<IHttpContextAccessor>().Object,
            new Mock<IUserClaimsPrincipalFactory<IdentityUser>>().Object,
            null, null, null, null);

        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x[It.IsAny<string>()]).Returns("SuperSecretSecurityKeyThatIs32Chars");

        var controller = new AuthController(
            mockUserManager.Object,
            mockSignInManager.Object,
            mockConfiguration.Object);

        var user = new IdentityUser { UserName = "admin", Email = "admin@example.com", PhoneNumber = "123456789" };
        mockUserManager
            .Setup(x => x.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        mockUserManager
            .Setup(x => x.CheckPasswordAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
            .ReturnsAsync(true);

        var result = await controller.Login(new LoginRequest { Username = "admin", Password = "Admin@123" });

        var okResult = Assert.IsType<OkObjectResult>(result);

        // Parse the result into a concrete type to avoid dynamic binding issues
        var resultObject = okResult.Value.GetType().GetProperty("Token").GetValue(okResult.Value, null);
        var profile = okResult.Value.GetType().GetProperty("Profile").GetValue(okResult.Value, null);

        var username = profile.GetType().GetProperty("UserName").GetValue(profile, null).ToString();
        var email = profile.GetType().GetProperty("Email").GetValue(profile, null).ToString();
        var phoneNumber = profile.GetType().GetProperty("PhoneNumber").GetValue(profile, null).ToString();

        // Assertions
        Assert.NotNull(resultObject);
        Assert.Equal("admin", username);
        Assert.Equal("admin@example.com", email);
        Assert.Equal("123456789", phoneNumber);
    }
}
