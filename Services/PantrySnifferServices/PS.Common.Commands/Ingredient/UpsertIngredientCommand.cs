using PS.Common.Domain.Enums;
using PS.Common.Domain.ViewModels;

namespace PS.Common.Queries.Ingredient;
public class UpsertIngredientCommand : IRequest<IngredientVM>
{
    public UpsertIngredientCommand()
    {
            
    }
    public UpsertIngredientCommand(string name, int type, Guid? id = null, string imageUrl = null)
    {
        Name = name;
        Type = type;
        Id = id;
        ImageUrl = imageUrl;
    }

    public Guid? Id { get; init; }
    public string Name { get; init; }
    public int Type { get; init; }
    public IngredientType IngredientType => (IngredientType)Type;
    public string ImageUrl { get; init; }
}
