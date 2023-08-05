using PS.Common.Domain.Enums;
using PS.Common.Domain.ViewModels;
using PS.Common.Queries.Ingredient;
using PS.IngredientService.Data.Repositories;

namespace PS.IngredientService.Application.Handlers.Queries;
public class ListIngredientByTypeHandler : IRequestHandler<ListIngredientsByTypeQuery, IEnumerable<IngredientVM>>
{
    public readonly IIngredientsRepository _ingredientsRepository;

    public ListIngredientByTypeHandler(IIngredientsRepository ingredientsRepository)
    {
        _ingredientsRepository = ingredientsRepository;
    }

    public async Task<IEnumerable<IngredientVM>> Handle(ListIngredientsByTypeQuery request, CancellationToken cancellationToken)
    {
        var ingredientType = !string.IsNullOrEmpty(Enum.GetName(request.IngredientType)) ?
            request.IngredientType :
            IngredientType.Unknown;

        return await _ingredientsRepository.ListIngredientByType(ingredientType);
    }
}
