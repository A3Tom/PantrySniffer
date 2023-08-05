using PS.Common.Domain.Enums;
using PS.Common.Domain.ViewModels;
using PS.Common.Queries.Ingredient;

namespace PS.IngredientService.Application.Handlers.Queries;
internal class GetIngredientByIdHandler : IRequestHandler<GetIngredientByIdQuery, IngredientVM>
{
    public async Task<IngredientVM> Handle(GetIngredientByIdQuery request, CancellationToken cancellationToken)
    {
        return new()
        {
            Id = request.Id,
            Name = $"Witever {request.Id}",
            Type = IngredientType.Sauce
        };
    }
}
