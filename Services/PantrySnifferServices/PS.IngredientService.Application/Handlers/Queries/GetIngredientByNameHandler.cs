using PS.Common.Domain.ViewModels;
using PS.Common.Queries.Ingredient;
using PS.IngredientService.Data.Repositories;

namespace PS.IngredientService.Application.Handlers.Queries;
public class GetIngredientByNameHandler : IRequestHandler<GetIngredientByNameQuery, IngredientVM?>
{
    private readonly IIngredientsRepository _ingredientsRepository;

    public GetIngredientByNameHandler(IIngredientsRepository ingredientsRepository)
    {
        _ingredientsRepository = ingredientsRepository;
    }

    public async Task<IngredientVM?> Handle(GetIngredientByNameQuery request, CancellationToken cancellationToken)
    {
        var potentialMatches = await _ingredientsRepository.SearchIngredients(request.IngredientName);

        return potentialMatches.FirstOrDefault(x => x.Name == request.IngredientName);
    }
}
