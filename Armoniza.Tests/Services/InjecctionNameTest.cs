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
    public class InjecctionNameTest
    {
        [Fact]
        public void CrearInstrumento_Exitoso_InyeccionNombre()
        {
            var baseUrl = Environment.GetEnvironmentVariable("ARMONIZA_BASEURL") ?? "http://localhost:5258";
            var adminUser = Environment.GetEnvironmentVariable("ARMONIZA_ADMIN_USER") ?? "Admin";
            var adminPass = Environment.GetEnvironmentVariable("ARMONIZA_ADMIN_PASS") ?? "Armoniza1212";

            var options = new ChromeOptions();
            var payload = "<script>alert(1)</script>";

            using IWebDriver driver = new ChromeDriver(options);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // LOGIN
            driver.Navigate().GoToUrl($"{baseUrl}/Account/Login");

            wait.Until(d => d.FindElement(By.Name("username"))).SendKeys(adminUser);
            driver.FindElement(By.Id("password")).SendKeys(adminPass);
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            wait.Until(d => !d.Url.Contains("/Account/Login", StringComparison.OrdinalIgnoreCase));

            // IR A CREAR
            driver.Navigate().GoToUrl($"{baseUrl}/instrumentos/Create");

            var code = Random.Shared.Next(100000, 999999);

            wait.Until(d => d.FindElement(By.Id("codigo"))).SendKeys(code.ToString());
            driver.FindElement(By.Id("nombre")).SendKeys(payload);
            driver.FindElement(By.Id("estuche")).Click();

            var categoriaSelect = new SelectElement(driver.FindElement(By.Id("idCategoria")));
            wait.Until(_ => categoriaSelect.Options.Count > 0);
            categoriaSelect.SelectByIndex(0);

            driver.FindElement(By.CssSelector("button[type='submit']")).Click();


            wait.Until(d => d.Url.Contains("/instrumentos", StringComparison.OrdinalIgnoreCase));


            try
            {
                var alert = driver.SwitchTo().Alert();
                Assert.Fail("Se ejecutó un JavaScript alert. Vulnerabilidad XSS detectada.");
            }
            catch (NoAlertPresentException)
            {

            }

            Assert.DoesNotContain(payload, driver.PageSource);
            Assert.Contains("alert(1)", driver.PageSource);

        }
    }

}
