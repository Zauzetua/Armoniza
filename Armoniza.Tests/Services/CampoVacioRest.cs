using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Armoniza.Tests.Services
{
    public class CampoVacioRest
    {
        [Fact]
        public void CrearInstrumento_CampoVacio()
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

            driver.FindElement(By.Id("estuche")).Click();

            var categoria = new SelectElement(driver.FindElement(By.Id("idCategoria")));
            categoria.SelectByIndex(0);

            Assert.Contains("/instrumentos/Create", driver.Url);

            // Validar mensaje
            Assert.Contains("nombre", driver.PageSource, StringComparison.OrdinalIgnoreCase);
        }
    }

}
