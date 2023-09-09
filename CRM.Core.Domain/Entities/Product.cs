namespace CRM.Core.Domain.Entities;

public class Product: BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Logo { get; set;} = string.Empty;
    public string Description { get; set; } = string.Empty;

    public virtual ICollection<Prospect>? Prospections { get; set; }
    public virtual IEnumerable<ProductStage> Stages { get; set; } = new List<ProductStage>();

    public Product()
    {
    }

    public Product(string name, string? logo, string description, User? creator): base(creator)
    {
        Name = name;
        Logo = logo ?? DefaultParams.defaultProduct;
        Description = description;
    }
}