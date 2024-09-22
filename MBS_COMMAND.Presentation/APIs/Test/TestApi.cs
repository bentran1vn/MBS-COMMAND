using Carter;
using MBS_COMMAND.Contract.Abstractions.Shared;
using MBS_COMMAND.Presentation.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MBS_COMMAND.Presentation.APIs.Test;


public class TestApi: ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/tests";
    
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group1 = app.NewVersionedApi("Tests")
            .MapGroup(BaseUrl).HasApiVersion(1);
        
        group1.MapGet("kakak", Test);
    }

    #region ====== version 1 ======

    public static async Task<IResult> Test()
    {
        var result = Result.Success("Xin chào Tôi là Command");
        
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    #endregion
}