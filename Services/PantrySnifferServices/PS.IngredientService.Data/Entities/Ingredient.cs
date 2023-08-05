using PS.Common.Domain.Abstract;
using PS.Common.Domain.Constants;
using PS.Common.Domain.DTOs;
using PS.Common.Domain.Enums;
using PS.Common.Domain.ViewModels;

namespace PS.IngredientService.Data.Entities;
public class Ingredient : IIdentityEntity, IHasPartitionKey, INamedEntity
{
    public Guid id { get; init; }
    public string Name { get; set; } = string.Empty;
    public IngredientType Type { get; set; }
    public string ImageUrl { get; set; }
    public string PartitionKey => EntityConstants.DEFAULT_PARTITION_KEY;

    public Ingredient()
    {
        
    }

    public Ingredient(CreateIngredientDto dto)
    {
        id = Guid.NewGuid();
        Name = dto.Name;
        Type = dto.Type;
        ImageUrl = dto.ImageUrl;
    }
    public Ingredient(UpdateIngredientDto dto)
    {
        id = dto.Id;
        Name = dto.Name;
        Type = dto.Type;
        ImageUrl = dto.ImageUrl;
    }

    public IngredientVM ToDto() => new(id, Name, Type, ImageUrl);

    public static explicit operator IngredientVM(Ingredient entity) => entity.ToDto();
}
