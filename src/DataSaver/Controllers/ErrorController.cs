namespace DataSaver.Controllers
{
    public sealed class ErrorController : Controller
    {
        /// <summary>
        /// Handles HTTP status codes and returns the corresponding view.
        /// </summary>
        /// <param name="statusCode">The HTTP status code.</param>
        /// <returns>The view corresponding to the HTTP status code.</returns>
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            switch (statusCode)
            {
                default:
                case 404:
                    return View("NotFound");

                case 500:
                    return View("InternalServerError");
            }
        }
    }
}
