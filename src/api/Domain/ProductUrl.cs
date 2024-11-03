namespace api.Domain
{
    // Supporting domain classes
    public class ProductUrl
    {
        public string Domain { get; private set; }
        public string Url { get; private set; }
        public DateTime? LastScrapedAt { get; private set; }

        private ProductUrl()
        {
            Domain = string.Empty;
            Url = string.Empty;
        }

        public ProductUrl(string domain, string url)
        {
            if (string.IsNullOrEmpty(domain))
                throw new ArgumentException("Domain cannot be empty");
            if (string.IsNullOrEmpty(url))
                throw new ArgumentException("URL cannot be empty");

            Domain = domain;
            Url = url;
        }

        public void UpdateLastScrapedAt(DateTime scrapedAt)
        {
            LastScrapedAt = scrapedAt;
        }
    }
}