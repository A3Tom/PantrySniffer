using PS.Common.Domain.Enums;

namespace PS.Common.Domain.ViewModels;
public class IngredientVM
{
    public IngredientVM() { }

    public IngredientVM(Guid id, string name, IngredientType type, string imageUrl)
    {
        Id = id;
        Name = name;
        Type = type;
        ImageUrl = imageUrl;
    }

    public Guid Id { get; init; }
    public string Name { get; init; }
    public IngredientType Type { get; init; }
    public string ImageUrl { get; init; }
}
