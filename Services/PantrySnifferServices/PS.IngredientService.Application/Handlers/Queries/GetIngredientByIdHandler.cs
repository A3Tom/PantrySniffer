using PS.Common.Domain.ViewModels;
using PS.Common.Queries.Ingredient;
using PS.IngredientService.Data.Repositories;

namespace PS.IngredientService.Application.Handlers.Queries;
internal class GetIngredientByIdHandler : IRequestHandler<GetIngredientByIdQuery, IngredientVM?>
{
    private readonly IIngredientsRepository _ingredientsRepository;

    public GetIngredientByIdHandler(IIngredientsRepository ingredientsRepository)
    {
        _ingredientsRepository = ingredientsRepository;
    }

    public async Task<IngredientVM?> Handle(GetIngredientByIdQuery request, CancellationToken cancellationToken)
    {
        return await _ingredientsRepository.GetIngredientById(request.Id);
    }
}
