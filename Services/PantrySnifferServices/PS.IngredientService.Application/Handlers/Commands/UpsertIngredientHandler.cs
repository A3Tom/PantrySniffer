using PS.Common.Domain.ViewModels;
using PS.Common.Queries.Ingredient;
using PS.IngredientService.Data.Repositories;

namespace PS.IngredientService.Application.Handlers.Commands;
public class UpsertIngredientHandler : IRequestHandler<UpsertIngredientCommand, IngredientVM>
{
    private readonly IIngredientsRepository _ingredientsRepository;

    public UpsertIngredientHandler(IIngredientsRepository ingredientsRepository)
    {
        _ingredientsRepository = ingredientsRepository;
    }

    public async Task<IngredientVM> Handle(UpsertIngredientCommand request, CancellationToken cancellationToken) 
        => await EntityAlreadyExists(request.Name, request.Id) ?
            await _ingredientsRepository.EditIngredient(new(request.Id!.Value, request.Name, request.IngredientType, request.ImageUrl)) :
            await _ingredientsRepository.CreateIngredient(new(request.Name, request.IngredientType, request.ImageUrl));

    private async Task<bool> EntityAlreadyExists(string entityName, Guid? entityId)
        => entityId.HasValue ?
        (await _ingredientsRepository.GetIngredientById(entityId.Value)) != null :
        (await _ingredientsRepository.SearchIngredients(entityName)).Any(x => x.Name == entityName);
}
