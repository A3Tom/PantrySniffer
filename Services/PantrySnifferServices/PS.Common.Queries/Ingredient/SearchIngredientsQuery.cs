using PS.Common.Domain.ViewModels;

namespace PS.Common.Queries.Ingredient;
public class SearchIngredientsQuery : IRequest<IEnumerable<IngredientVM>>
{
    public string SearchTerm { get; init; }

    public SearchIngredientsQuery()
    {
            
    }

    public SearchIngredientsQuery(string searchTerm)
    {
        SearchTerm = searchTerm;
    }
}
