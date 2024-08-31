using System.ComponentModel.DataAnnotations;

using Ecommerce.Startup.Api.Models;


namespace Ecommerce.Startup.Api.Requests;

public class CreateStartupRequest
{
    public required string Name { get; set; }

    [MaxLength(100)]
    public required string Description { get; set; }

    public byte[]? Logo { get; set; }

    //public Guid? PortfolioId { get; set; }
    public string CategoryName { get; set; }
    public string? WebAddress { get; set; }

    public DateTime FoundationYear { get; set; }  = DateTime.UtcNow;
    public bool IsActive { get; set; }
}