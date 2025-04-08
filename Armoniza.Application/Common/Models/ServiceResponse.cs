using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Armoniza.Application.Common.Models
{
    public class ServiceResponse<T>
    {
        /// <summary>
        /// Objeto con el que se esta trabajando.
        /// </summary>
        public T Data { get; set; }
        /// <summary>
        /// Si salio bien o no la operacion.
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// Mensaje, util para saber que paso.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Metodo que permite crear una respuesta exitosa.
        /// </summary>
        /// <param name="data">El objeto</param>
        /// <param name="message">Mensaje de exito</param>
        /// <returns>Una respuesta</returns>
        public static ServiceResponse<T> Ok(T data, string message = "Operación exitosa") =>
       new() { Success = true, Message = message, Data = data };


        /// <summary>
        /// Metodo que permite crear una respuesta fallida.
        /// </summary>
        /// <param name="message">Objeto</param>
        /// <returns>Respuesta fallida</returns>
        public static ServiceResponse<T> Fail(string message) =>
            new() { Success = false, Message = message };
    }
}
