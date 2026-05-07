using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Armoniza.Application.Common.Interfaces.Repositories;
using Armoniza.Application.Common.Interfaces.Services;
using Armoniza.Infrastructure.Services;
using Moq;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;


namespace Armoniza.Tests.Services
{
    public class CiclodeVidaGuitarra : IDisposable
    {

        private readonly IWebDriver _webDriver;

        private readonly Mock<IUsuarioRepository> _usuarioRepoMock;
        private readonly Mock<ITipoUsuarioService> _tipoUsuarioServiceMock;
        private readonly Mock<IApartadosRepository> _apartadosRepoMock;
        private readonly UsuarioService _sut;

        public CiclodeVidaGuitarra()
        {
            _webDriver = new ChromeDriver();
            _usuarioRepoMock = new Mock<IUsuarioRepository>();
            _tipoUsuarioServiceMock = new Mock<ITipoUsuarioService>();
            _apartadosRepoMock = new Mock<IApartadosRepository>();

            _sut = new UsuarioService(_usuarioRepoMock.Object, _tipoUsuarioServiceMock.Object, _apartadosRepoMock.Object);
        }

       

        [Fact]

        public void CicloDeVidaCompleto()
        {

            _webDriver.Navigate().GoToUrl("http://localhost:5258");

            _webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);

            IWebElement inputUser = _webDriver.FindElement(By.Name("username"));

            IWebElement inputPassword = _webDriver.FindElement(By.Id("password"));

            inputUser.SendKeys("Test");
            inputPassword.SendKeys("markitos");
            inputPassword.SendKeys(Keys.Enter);

            IWebElement moverseAInstrumentos = _webDriver.FindElement(By.CssSelector("a[href*='/instrumentos']"));
            moverseAInstrumentos.Click();



            IWebElement hayGuitarras = _webDriver.FindElement(By.CssSelector("div[onclick*='section2']"));

            hayGuitarras.Click();

            System.Threading.Thread.Sleep(500);

            var contenedorGuitarras = _webDriver.FindElement(By.Id("section2"));
            Assert.True(contenedorGuitarras.Displayed, "No se encontraron guitarras");

            IWebElement moverseAApartados = _webDriver.FindElement(By.CssSelector("a[href*='/apartadoes']"));
            moverseAApartados.Click();

            IWebElement moverseAApartadosCreate = _webDriver.FindElement(By.CssSelector("a[href*='/apartadoes/Create']"));
            moverseAApartadosCreate.Click();

            IWebElement abrirUserSelect = _webDriver.FindElement(By.Id("select2-userSelect-container"));
            abrirUserSelect.Click();

            

            var primeraOpcion = _webDriver.FindElement(By.CssSelector("#select2-userSelect-results li.select2-results__option"));

            primeraOpcion.Click();

            var agregarGuitarra = _webDriver.FindElement(By.XPath("//button[i[contains(@class, 'bi-plus-lg')]]"));

            agregarGuitarra.Click();


            var modalBody = _webDriver.FindElement(By.ClassName("modal-body"));

            var celdaCodigo = modalBody.FindElement(By.CssSelector("button.select-inst-btn[data-inst-id='66']"));

            celdaCodigo.Click();

            System.Threading.Thread.Sleep(500);

            WebDriverWait wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10));


            var cerrarModal = _webDriver.FindElement(By.CssSelector("#instrumentModal .btn-close"));
            cerrarModal.Click();

            wait.Until(driver =>
            {
                var modals = driver.FindElements(By.Id("instrumentModal"));
                return modals.Count == 0 || !modals[0].Displayed;
            });



            var inputFecha = _webDriver.FindElement(By.Id("apartado_fecharegreso"));
            inputFecha.Click();

            //No funciona si es el ultimo dia del mes
            var diaSiguiente = _webDriver.FindElement(By.XPath("//span[contains(@class, 'today')]/following-sibling::span[not(contains(@class, 'flatpickr-disabled'))]"));

            diaSiguiente.Click();


            WebDriverWait wait2 = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10));

            inputFecha.SendKeys(Keys.Escape);

            // Espera a que el calendario desaparezca
            wait2.Until(driver =>
            {
                var calendars = driver.FindElements(By.ClassName("flatpickr-calendar"));
                return calendars.All(c => !c.Displayed);
            });



            var crearApartado = _webDriver.FindElement(By.XPath("//button[contains(., 'Crear')]"));
            crearApartado.Click();

            wait.Until(driver => driver.Url.Contains("apartadoes"));

            
            _webDriver.Navigate().GoToUrl("http://localhost:5258/reportes");

            bool encontrado = false;

            
            var paginas = _webDriver.FindElements(By.CssSelector(".pagination .page-link")).Count;

            for (int i = 1; i <= paginas; i++)
            {
                
                var paginaBtn = wait.Until(driver =>
                {
                    var elems = driver.FindElements(By.CssSelector(".pagination .page-link"));
                    return elems.Count >= i ? elems[i - 1] : null;
                });

                paginaBtn.Click();

                 
                wait.Until(driver =>
                {
                    var tabla = driver.FindElement(By.TagName("table"));
                    return tabla.Displayed;
                });

                // Buscar al Aboytia
                var filaUsuario = _webDriver.FindElements(
                    By.XPath("//tr[td[contains(text(), 'Aboytia')]]")
                );

                if (filaUsuario.Count > 0)
                {
                    encontrado = true;
                    break;
                }
            }

            Assert.True(encontrado, "No se encontró el usuario en ninguna página");



        }

        [Fact]

        public void ValidarInstrumentoNoDisponible()
        {

            _webDriver.Navigate().GoToUrl("http://localhost:5258");

            _webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);

            IWebElement inputUser = _webDriver.FindElement(By.Name("username"));

            IWebElement inputPassword = _webDriver.FindElement(By.Id("password"));

            inputUser.SendKeys("Test");
            inputPassword.SendKeys("markitos");
            inputPassword.SendKeys(Keys.Enter);

            IWebElement moverseAInstrumentos = _webDriver.FindElement(By.CssSelector("a[href*='/instrumentos']"));
            moverseAInstrumentos.Click();



            IWebElement hayGuitarras = _webDriver.FindElement(By.CssSelector("div[onclick*='section2']"));

            hayGuitarras.Click();

            System.Threading.Thread.Sleep(500);

            var contenedorGuitarras = _webDriver.FindElement(By.Id("section2"));
            Assert.True(contenedorGuitarras.Displayed, "No se encontraron guitarras");

            IWebElement moverseAApartados = _webDriver.FindElement(By.CssSelector("a[href*='/apartadoes']"));
            moverseAApartados.Click();

            IWebElement moverseAApartadosCreate = _webDriver.FindElement(By.CssSelector("a[href*='/apartadoes/Create']"));
            moverseAApartadosCreate.Click();

            IWebElement abrirUserSelect = _webDriver.FindElement(By.Id("select2-userSelect-container"));
            abrirUserSelect.Click();



            var segundaOpcion = _webDriver.FindElements(By.CssSelector("#select2-userSelect-results li.select2-results__option"));

            segundaOpcion[1].Click();

            var agregarGuitarra = _webDriver.FindElement(By.XPath("//button[i[contains(@class, 'bi-plus-lg')]]"));

            agregarGuitarra.Click();


            var modalBody = _webDriver.FindElement(By.ClassName("modal-body"));

            var celdaCodigo = modalBody.FindElements(By.CssSelector("button.select-inst-btn[data-inst-id='66']"));

            Assert.Empty(celdaCodigo);
        }
        [Fact]
        public void DevolverInstrumento()
        {
            _webDriver.Navigate().GoToUrl("http://localhost:5258");

            _webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);

            IWebElement inputUser = _webDriver.FindElement(By.Name("username"));

            IWebElement inputPassword = _webDriver.FindElement(By.Id("password"));

            inputUser.SendKeys("Test");
            inputPassword.SendKeys("markitos");
            inputPassword.SendKeys(Keys.Enter);


            System.Threading.Thread.Sleep(500);


            _webDriver.Navigate().GoToUrl("http://localhost:5258/reportes");

            WebDriverWait wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(2));

            bool encontrado = false;


            var paginas = _webDriver.FindElements(By.CssSelector(".pagination .page-link")).Count;

            for (int i = 1; i <= paginas; i++)
            {
                // Click a la página i
                var paginaBtn = wait.Until(driver =>
                {
                    var elems = driver.FindElements(By.CssSelector(".pagination .page-link"));
                    return elems.Count >= i ? elems[i - 1] : null;
                });

                paginaBtn.Click();


                wait.Until(driver =>
                {
                    var tabla = driver.FindElement(By.TagName("table"));
                    return tabla.Displayed;
                });

                // Buscar el usuario
                var filaUsuario = _webDriver.FindElements(
                    By.XPath("//tr[td[contains(text(), 'Pendiente')]]")
                );

                if (filaUsuario.Count > 0)
                {
                    encontrado = true;
                    break;
                }
            }

            Assert.True(encontrado, "No se encontro en pendiente el instrumento");

            IWebElement moverseAApartados = _webDriver.FindElement(By.CssSelector("a[href*='/apartadoes']"));
            moverseAApartados.Click();

            var filas = _webDriver.FindElements(By.CssSelector("#apartadosTableBody tr"));

            var ultimaFila = filas.Last();

            var liberarBtn = ultimaFila.FindElement(By.CssSelector("a[href*='LiberarApartado']"));

            liberarBtn.Click();

            IWebElement confirmarLiberacion = _webDriver.FindElement(By.CssSelector("button[type='submit']"));
            confirmarLiberacion.Click();


            wait.Until(driver => driver.Url.Contains("apartadoes"));

            _webDriver.Navigate().GoToUrl("http://localhost:5258/reportes");

            bool siguePendiente = false;

            var paginas2 = _webDriver.FindElements(By.CssSelector(".pagination .page-link")).Count;

            for (int i = 1; i <= paginas2; i++)
            {
                var paginaBtn = wait.Until(driver =>
                {
                    var elems = driver.FindElements(By.CssSelector(".pagination .page-link"));
                    return elems.Count >= i ? elems[i - 1] : null;
                });

                paginaBtn.Click();

                wait.Until(driver =>
                {
                    var filas = driver.FindElements(By.CssSelector("table tbody tr"));
                    return filas.Count > 0;
                });

                var pendientes = _webDriver.FindElements(
                    By.XPath("//tr[td[contains(text(), 'Pendiente')]]")
                );

                if (pendientes.Count > 0)
                {
                    siguePendiente = true;
                    break;
                }
            }


            Assert.False(siguePendiente, "El instrumento sigue en estado Pendiente");

        }

        public void Dispose()
        {
            _webDriver.Quit();
            _webDriver.Dispose();
        }
    }
}
