using PS.Common.Domain.Enums;

namespace PS.Common.Domain.DTOs;
public class UpdateIngredientDto
{
    public UpdateIngredientDto(Guid id, string name, IngredientType type, string imageUrl = null)
    {
        Id = id;
        Name = name;
        Type = type;
        ImageUrl = ImageUrl;
    }

    public Guid Id { get; init; }
    public string Name { get; set; }
    public IngredientType Type { get; set; }
    public string ImageUrl { get; set; }
}
