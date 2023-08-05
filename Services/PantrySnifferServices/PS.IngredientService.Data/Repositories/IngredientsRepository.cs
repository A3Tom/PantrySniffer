using PS.Common.Domain.Base;
using PS.Common.Domain.DTOs;
using PS.Common.Domain.Enums;
using PS.Common.Domain.Settings.Abstract;
using PS.Common.Domain.ViewModels;
using PS.IngredientService.Data.Entities;

namespace PS.IngredientService.Data.Repositories;
public class IngredientsRepository : CosmosDbRepository, IIngredientsRepository
{
    public IngredientsRepository(ICosmosDbSettings cosmosSettings) : base(cosmosSettings)
    {
    }

    public async Task<IngredientVM> CreateIngredient(CreateIngredientDto dto)
    {
        var similarlyNamedEntities = await SearchEntitiesByName<Ingredient>(dto.Name);
        var exactNameMatch = similarlyNamedEntities.FirstOrDefault(x => x.Name == dto.Name);

        if(exactNameMatch != null)
            return (IngredientVM)exactNameMatch;
        
        var entity = await InsertEntityAsync(new Ingredient(dto));
        return (IngredientVM)entity;
    }

    public async Task<IngredientVM> EditIngredient(UpdateIngredientDto dto)
    {
        var entity = await UpdateEntityAsync(new Ingredient(dto));
        return (IngredientVM)entity;
    }

    public async Task<IEnumerable<IngredientVM>> ListIngredientByType(IngredientType type)
    {
        var entities = await ListEntitiesByType<Ingredient>((int)type);
        return entities.Select(entity => (IngredientVM)entity);
    }

    public async Task<IEnumerable<IngredientVM>> SearchIngredients(string searchTerm)
    {
        var entities = await SearchEntitiesByName<Ingredient>(searchTerm);
        return entities.Select(entity => (IngredientVM)entity);
    }

    public async Task<IngredientVM?> GetIngredientById(Guid entityId)
    {
        var entities = await SearchEntitiesById<Ingredient>(entityId);
        return entities.FirstOrDefault()?.ToDto();
    }
}
