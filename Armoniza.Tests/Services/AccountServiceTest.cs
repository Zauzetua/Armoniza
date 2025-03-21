using Armoniza.Application.Services;
using Armoniza.Infrastructure.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace Armoniza.Tests.Services
{
    //Clase para probar el servicio de cuenta
    public class AccountServiceTest
    {
        //Mock para el acceso al contexto HTTP
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        //Mock para el contexto de la base de datos
        private readonly Mock<ApplicationDbContext> _contextMock;
        //Servicio de cuenta
        private readonly AccountService _accountService;

        //Constructor de la clase
        public AccountServiceTest()
        {
            //Inicialización de los mocks
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _contextMock = new Mock<ApplicationDbContext>();
            _accountService = new AccountService(_httpContextAccessorMock.Object, _contextMock.Object);
        }

        //Método para crear un mock de un DbSet
        //Esta cosa es un poco complicada, pero básicamente estamos simulando un DbSet de Entity Framework
        private Mock<DbSet<Admin>> CreateMockDbSet(IQueryable<Admin> data)
        {
            var mockSet = new Mock<DbSet<Admin>>();
            mockSet.As<IQueryable<Admin>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Admin>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Admin>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Admin>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mockSet;
        }

        //Método para probar el inicio de sesión
        //Este método debería devolver verdadero si las credenciales son válidas
        [Fact]
        public async Task Login_ShouldReturnTrue_WhenCredentialsAreValid()
        {
            //Valores de prueba
            var username = "admin";
            var password = "password";
            //Contraseña encriptada
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            //Datos de prueba
            var adminData = new List<Admin>
            {
                new Admin { username = username, password = hashedPassword }
            }.AsQueryable();

            //Mock del DbSet
            var mockSet = CreateMockDbSet(adminData);
            _contextMock.Setup(c => c.Admin).Returns(mockSet.Object);

            //Mock del contexto HTTP
            //Aquí simulamos el inicio de sesión
            var httpContextMock = new Mock<HttpContext>();
            var authenticationServiceMock = new Mock<IAuthenticationService>();
            httpContextMock.Setup(h => h.RequestServices.GetService(typeof(IAuthenticationService)))
                           .Returns(authenticationServiceMock.Object);
            _httpContextAccessorMock.Setup(a => a.HttpContext).Returns(httpContextMock.Object);
            authenticationServiceMock
                .Setup(a => a.SignInAsync(httpContextMock.Object, CookieAuthenticationDefaults.AuthenticationScheme, 
                                          It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()))
                .Returns(Task.CompletedTask);

            //Aqui se llama al método de inicio de sesión
            var result = await _accountService.Login(username, password);

            // Assert
            Assert.True(result);
            authenticationServiceMock.Verify(a => a.SignInAsync(httpContextMock.Object, CookieAuthenticationDefaults.AuthenticationScheme, 
                                                                It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()), Times.Once);
        }

        [Fact]
        public async Task Login_ShouldReturnFalse_WhenCredentialsAreInvalid()
        {
            // Valores de prueba
            var username = "admin";
            var password = "wrongpassword"; // Contraseña incorrecta
            var correctPassword = "password";
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(correctPassword);

            // Datos de prueba
            var adminData = new List<Admin>
            {
                new Admin { username = username, password = hashedPassword }
            }.AsQueryable();

            // Mock del DbSet
            var mockSet = CreateMockDbSet(adminData);
            _contextMock.Setup(c => c.Admin).Returns(mockSet.Object);

            // Act
            var result = await _accountService.Login(username, password);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task LogOut_ShouldSignOutUser_WhenHttpContextIsValid()
        {
            // Arrange
            var httpContextMock = new Mock<HttpContext>();
            var authenticationServiceMock = new Mock<IAuthenticationService>();
            httpContextMock.Setup(h => h.RequestServices.GetService(typeof(IAuthenticationService)))
                           .Returns(authenticationServiceMock.Object);
            _httpContextAccessorMock.Setup(a => a.HttpContext).Returns(httpContextMock.Object);

            authenticationServiceMock
                .Setup(a => a.SignOutAsync(httpContextMock.Object, CookieAuthenticationDefaults.AuthenticationScheme, null))
                .Returns(Task.CompletedTask);

            // Act
            _accountService.LogOut();

            // Assert
            authenticationServiceMock.Verify(a => a.SignOutAsync(httpContextMock.Object, CookieAuthenticationDefaults.AuthenticationScheme, null), Times.Once);
        }


    }
}
