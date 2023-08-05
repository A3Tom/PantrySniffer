using PS.Common.Domain.Enums;

namespace PS.Common.Domain.DTOs;
public class CreateIngredientDto
{
    public CreateIngredientDto(string name, IngredientType type, string imageUrl = null)
    {
        Name = name;
        Type = type;
        ImageUrl = imageUrl;
    }

    public string Name { get; init; }
    public IngredientType Type { get; init; }
    public string ImageUrl { get; init; }
}
