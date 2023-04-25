namespace DataSaver.Services
{
    public sealed class SetLinkPreviewService : ISetLinkPreviewService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiKey;
        private readonly string _apiUrl;
        private readonly ILogger<SetLinkPreviewService> _logger;

        public SetLinkPreviewService(IHttpClientFactory httpClientFactory,
            IOptions<LinkPreviewOptions> options,
            ILogger<SetLinkPreviewService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _apiKey = options.Value.ApiKey!;
            _apiUrl = options.Value.ApiUrl!;
            _logger = logger;
        }

        /// <summary>
        /// Sets the preview image and title for a given link using external LinkPreview API.
        /// </summary>
        /// <param name="link">The link for which to set a preview.</param>
        /// <returns>The Task.</returns>
        /// <remarks>
        /// If the request to API is successful, response is deserialized => updates the provided link with the preview data.
        /// </remarks>>
        public async Task SetLinkPreviewAsync(LinkViewModel link)
        {
            var httpClient = _httpClientFactory.CreateClient();

            var response = await httpClient.GetAsync($"{_apiUrl}?key={_apiKey}&q={link.UrlLink}");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<LinkPreview>(responseContent);

                link.PreviewTitle = result.Title!;
                link.PreviewImage = result.Image!;
            }
            else
            {
                _logger.LogError($"Couldn't create a preview for the link: {link.UrlLink}. Status code:" + response.StatusCode.ToString());
            }
        }
    }
}
