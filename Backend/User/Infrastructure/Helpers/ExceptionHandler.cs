using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace PhAppUser.Infrastructure.Helpers
{
    /// <summary>
    /// Clase para centralizar el manejo de excepciones en la capa de infraestructura.
    /// </summary>
    public static class ExceptionHandler
    {
        /// <summary>
        /// Maneja excepciones de métodos síncronos y registra errores en el logger.
        /// </summary>
        /// <typeparam name="T">Tipo del resultado esperado.</typeparam>
        /// <param name="action">Función a ejecutar.</param>
        /// <param name="logger">Instancia del logger para registrar errores.</param>
        /// <param name="message">Mensaje descriptivo para el error.</param>
        /// <returns>Resultado de la función ejecutada.</returns>
        public static T Handle<T>(Func<T> action, ILogger logger, string message)
        {
            try
            {
                return action();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{Message}", message);
                throw new Exception(message, ex);
            }
        }

        /// <summary>
        /// Maneja excepciones de métodos asíncronos y registra errores en el logger.
        /// </summary>
        /// <typeparam name="T">Tipo del resultado esperado.</typeparam>
        /// <param name="action">Función asíncrona a ejecutar.</param>
        /// <param name="logger">Instancia del logger para registrar errores.</param>
        /// <param name="message">Mensaje descriptivo para el error.</param>
        /// <returns>Resultado de la tarea ejecutada.</returns>
        public static async Task<T> HandleAsync<T>(Func<Task<T>> action, ILogger logger, string message)
        {
            try
            {
                return await action();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{Message}", message);
                throw new Exception(message, ex);
            }
        }

        /// <summary>
        /// Maneja excepciones de métodos asíncronos sin retorno.
        /// </summary>
        /// <param name="action">Acción asíncrona a ejecutar.</param>
        /// <param name="logger">Instancia del logger para registrar errores.</param>
        /// <param name="message">Mensaje descriptivo para el error.</param>
        /// <returns>Tarea completada.</returns>
        public static async Task HandleAsync(Func<Task> action, ILogger logger, string message)
        {
            try
            {
                await action();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{Message}", message);
                throw new Exception(message, ex);
            }
        }
    }
}

