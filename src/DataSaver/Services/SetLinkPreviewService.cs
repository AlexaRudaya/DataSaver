namespace DataSaver.Services
{
    public sealed class SetLinkPreviewService : ISetLinkPreviewService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiKey;
        private readonly ILogger<SetLinkPreviewService> _logger;

        public SetLinkPreviewService(IHttpClientFactory httpClientFactory,
            IOptions<LinkPreviewOptions> options,
            ILogger<SetLinkPreviewService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _apiKey = options.Value.ApiKey!;
            _logger = logger;
        }

        /// <summary>
        /// Sets the preview image and title for a given link using external LinkPreview API.
        /// </summary>
        /// <param name="link">The link for which to set a preview.</param>
        /// <returns>The updated view model of a link with preview data.</returns>
        /// <exception cref="InvalidOperationException">If the API request fails after 10 attempts.</exception>
        /// <remarks>
        /// If the request to API is successful, response is deserialized => updates the provided link with the preview data.
        /// If the request fails, it retries up to 10 times.
        /// If the request fails after 10 attempts, throws an InvalidOperationException.
        /// </remarks>>
        public async Task SetLinkPreviewAsync(LinkViewModel link)
        {
            var httpClient = _httpClientFactory.CreateClient();

            int attempts = default;

            while (true)
            {
                var response = await httpClient.GetAsync($"{_apiKey}{link.UrlLink}");

                attempts++;

                if (attempts == 10)
                {
                    throw new InvalidOperationException("The amount of attempts is 10, please, reload a page");
                }
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<LinkPreview>(responseContent);

                    link.PreviewTitle = result.Title!;
                    link.PreviewImage = result.Image!;

                    _logger.LogInformation($"{attempts} times loaded");
                    break;
                }

                _logger.LogError(response.StatusCode.ToString());
            }
        }
    }
}
