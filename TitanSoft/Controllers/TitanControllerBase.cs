using System.Dynamic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TitanSoft.Controllers
{
    /// <summary>
    /// Base Controller class used to standardized api responses and implement any
    /// shared functionality 
    /// </summary>
    public abstract class TitanControllerBase : ControllerBase
    {
        /// <summary>
        /// Returns a successful HTTP response(200) with a common response. This method returns no data/payload
        /// with the request
        /// object ApiResponse
        /// </summary>
        /// <returns>Status Code 200</returns>
        /// <param name="message">The message to return</param>
        protected OkObjectResult Ok(string message = "success") => Ok(null, message);

        /// <summary>
        /// Returns a successful HTTP respoonse (200) with a common response
        /// object ApiResponse
        /// </summary>
        /// <returns>ApiResponse wrapped in an OkObjectResult</returns>
        /// <param name="message">The message to be returned</param>
        /// <param name="value">any data associated with the request</param>
        protected OkObjectResult Ok(object value, string message = "success")
        {
            dynamic response = new ExpandoObject();

            if(message != null)
                response.message = message;
            if (value != null)
                response.data = value;

            return new OkObjectResult(response)
                {
                    StatusCode = StatusCodes.Status200OK
                };
        }

        /// <summary>
        /// Returns a bad request HTTP response (400) with a common response
        /// object ApiResponse
        /// </summary>
        /// <returns>ApiResponse wrapped in an BadRequestObjectResult</returns>
        /// <param name="message">The message to be returned with from the request</param>
        /// <param name="error">Any data associated w/ the request.</param>
        protected BadRequestObjectResult BadRequest(string message, object error = null)
        {
            dynamic response = new ExpandoObject();

            if(message != null)
                response.message = message;
            if (error != null)
                response.data = error;

            return new BadRequestObjectResult(response)
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
        }
    }
}
