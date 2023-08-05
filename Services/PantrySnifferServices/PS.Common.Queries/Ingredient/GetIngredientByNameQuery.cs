using PS.Common.Domain.ViewModels;

namespace PS.Common.Queries.Ingredient;
public class GetIngredientByNameQuery : IRequest<IngredientVM?>
{
    public string IngredientName { get; init; }

    public GetIngredientByNameQuery(string ingredientName)
    {
        IngredientName = ingredientName;
    }
}
