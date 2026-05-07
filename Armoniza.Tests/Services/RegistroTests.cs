using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;

namespace Armoniza.Tests.Services
{
    public class RegistroTests
    {
        [Fact]
        public void CrearInstrumento_Exitoso()
        {
            var baseUrl = Environment.GetEnvironmentVariable("ARMONIZA_BASEURL") ?? "http://localhost:5258";
            var adminUser = Environment.GetEnvironmentVariable("ARMONIZA_ADMIN_USER") ?? "Admin";
            var adminPass = Environment.GetEnvironmentVariable("ARMONIZA_ADMIN_PASS") ?? "Armoniza1212";

            var options = new ChromeOptions();

            using IWebDriver driver = new ChromeDriver(options);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            driver.Navigate().GoToUrl($"{baseUrl}/Account/Login");

            wait.Until(d => d.FindElement(By.Name("username"))).SendKeys(adminUser);
            driver.FindElement(By.Id("password")).SendKeys(adminPass);
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            wait.Until(d => !d.Url.Contains("/Account/Login", StringComparison.OrdinalIgnoreCase));

            driver.Navigate().GoToUrl($"{baseUrl}/instrumentos/Create");

            var codigo = Random.Shared.Next(100000, 999999);

            wait.Until(d => d.FindElement(By.Id("codigo"))).SendKeys(codigo.ToString());
            driver.FindElement(By.Id("nombre")).SendKeys($"Instrumento Test {codigo}");
            driver.FindElement(By.Id("estuche")).Click();

            var categoriaSelect = new SelectElement(driver.FindElement(By.Id("idCategoria")));
            try
            {
                wait.Until(_ => categoriaSelect.Options.Count > 0);
            }
            catch (WebDriverTimeoutException)
            {
                Assert.Fail("No hay categorías disponibles para seleccionar. Agrega al menos una categoría en la BD antes de ejecutar este test.");
            }

            categoriaSelect.SelectByIndex(0);

            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            wait.Until(d => d.Url.Contains("/instrumentos", StringComparison.OrdinalIgnoreCase));
            Assert.Contains("¡Instrumento creado correctamente!", driver.PageSource);
            Assert.Contains($"Instrumento Test {codigo}", driver.PageSource);
        }
    }

}
