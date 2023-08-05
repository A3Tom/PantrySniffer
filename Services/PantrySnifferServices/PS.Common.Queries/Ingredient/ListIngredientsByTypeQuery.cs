using PS.Common.Domain.Enums;
using PS.Common.Domain.ViewModels;

namespace PS.Common.Queries.Ingredient;
public class ListIngredientsByTypeQuery : IRequest<IEnumerable<IngredientVM>>
{
    public IngredientType IngredientType { get; init; }

    public ListIngredientsByTypeQuery(IngredientType ingredientType)
    {
        IngredientType = ingredientType;
    }
}
