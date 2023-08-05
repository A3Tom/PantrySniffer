using PS.Common.Domain.DTOs;
using PS.Common.Domain.Enums;
using PS.Common.Domain.ViewModels;

namespace PS.IngredientService.Data.Repositories;
public interface IIngredientsRepository
{
    Task<IngredientVM> CreateIngredient(CreateIngredientDto dto);
    Task<IngredientVM> EditIngredient(UpdateIngredientDto dto);
    Task<IEnumerable<IngredientVM>> ListIngredientByType(IngredientType type);
    Task<IEnumerable<IngredientVM>> SearchIngredients(string searchTerm);
    Task<IngredientVM?> GetIngredientById(Guid entityId);
}
