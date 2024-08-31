namespace Ecommerce.Startup.Api.Requests
{
    public class UpdateStartupRequest
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public byte[]? Logo { get; set; }

        public Guid UserId { get; set; }

        public Guid? PortfolioId { get; set; }
        public Guid CategoryId { get; set; }

        public DateTime FoundationYear { get; set; }
        public string? WebAddress { get; set; }

        public byte[]? UploadedFile { get; set; }

        public string? UploadedFileName { get; set; }

        public bool IsActive { get; set; }
    }
}
