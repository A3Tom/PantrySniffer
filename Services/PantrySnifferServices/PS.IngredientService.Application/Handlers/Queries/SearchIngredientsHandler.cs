using PS.Common.Domain.Enums;
using PS.Common.Domain.ViewModels;
using PS.Common.Queries.Ingredient;
using PS.IngredientService.Data.Repositories;

namespace PS.IngredientService.Application.Handlers.Queries;
public class SearchIngredientsHandler : IRequestHandler<SearchIngredientsQuery, IEnumerable<IngredientVM>>
{
    private readonly IIngredientsRepository _ingredientsRepository;

    public SearchIngredientsHandler(IIngredientsRepository ingredientsRepository)
    {
        _ingredientsRepository = ingredientsRepository;
    }

    public async Task<IEnumerable<IngredientVM>> Handle(SearchIngredientsQuery request, CancellationToken cancellationToken)
    {
        return await _ingredientsRepository.SearchIngredients(request.SearchTerm);
    }
}
