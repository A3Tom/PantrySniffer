using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using PS.Common.Domain.Enums;
using PS.Common.Queries.Ingredient;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PS.IngredientService.Functions.Endpoints;

public class IngredientFunctions
{
    private readonly IMediator _mediator;

    public IngredientFunctions(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("Search")]
    [FunctionName(nameof(Search))]
    public async Task<IActionResult> Search(
        [HttpTrigger(AuthorizationLevel.Anonymous, "POST")] SearchIngredientsQuery request,
        ILogger log)
    {
        log.LogInformation($"{nameof(Search)} triggered with {request.SearchTerm}");

        var result = await _mediator.Send(request);

        return new OkObjectResult($"{JsonSerializer.Serialize(result)}");
    }

    [FunctionName(nameof(Get))]
    public async Task<IActionResult> Get(
        [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "{ingredientId}")] HttpRequestMessage request,
        Guid ingredientId,
        ILogger log)
    {
        log.LogInformation($"{nameof(Get)} triggered with {ingredientId}");

        var result = await _mediator.Send(new GetIngredientByIdQuery(ingredientId));

        return new OkObjectResult($"{JsonSerializer.Serialize(result)}");
    }

    [FunctionName(nameof(Create))]
    public async Task<IActionResult> Create(
        [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "")] UpsertIngredientCommand request,
        ILogger log)
    {
        log.LogInformation($"{nameof(Create)} triggered with {request.Id}");

        var entityAlreadyExists = await _mediator.Send(new GetIngredientByNameQuery(request.Name));

        if (entityAlreadyExists != null)
            return new BadRequestObjectResult($"Object already exists with name {request.Name} ({entityAlreadyExists.Id})");

        var result = await _mediator.Send(request);

        return new OkObjectResult($"{JsonSerializer.Serialize(result)}");
    }

    [FunctionName(nameof(Update))]
    public async Task<IActionResult> Update(
        [HttpTrigger(AuthorizationLevel.Anonymous, "PUT", Route = "{ingredientId}")] UpsertIngredientCommand request,
        ILogger log)
    {
        log.LogInformation($"{nameof(Create)} triggered with {request.Id}");

        var result = await _mediator.Send(request);

        return new OkObjectResult($"{JsonSerializer.Serialize(result)}");
    }

    [FunctionName(nameof(ListByType))]
    public async Task<IActionResult> ListByType(
        [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "ListByType/{ingredientTypeInt:int?}")] HttpRequestMessage request,
        int ingredientTypeInt,
        ILogger log)
    {
        var ingredientType = (IngredientType)ingredientTypeInt;
        log.LogInformation($"{nameof(Search)} triggered with {ingredientType} ({Enum.GetName(ingredientType)})");

        var result = await _mediator.Send(new ListIngredientsByTypeQuery(ingredientType));

        return new OkObjectResult($"{JsonSerializer.Serialize(result)}");
    }
}
