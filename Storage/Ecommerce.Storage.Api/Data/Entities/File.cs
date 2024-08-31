using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Ecommerce.Storage.Api.Data.Entities;

[Table(nameof(File), Schema = StorageDbContext.Schema)]
public class File
{
    [Key]
    public long Id { get; set; }

    public string Path { get; set; }

    public DateTime Created { get; set; }
}