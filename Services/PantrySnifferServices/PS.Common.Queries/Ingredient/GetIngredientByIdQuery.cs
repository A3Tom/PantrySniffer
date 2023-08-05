using PS.Common.Domain.ViewModels;

namespace PS.Common.Queries.Ingredient;
public class GetIngredientByIdQuery : IRequest<IngredientVM>
{
    public Guid Id { get; init; }

    public GetIngredientByIdQuery(Guid id)
    {
        Id = id;
    }
}
