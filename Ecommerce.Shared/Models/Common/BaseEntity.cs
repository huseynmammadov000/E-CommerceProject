namespace Ecommerce.Shared.Models.Common;

public class BaseEntity
{
    public Guid Id { get; set; }

    public DateTime CreatedTime { get; set; }

    virtual public DateTime UpdateTime { get; set; }
}