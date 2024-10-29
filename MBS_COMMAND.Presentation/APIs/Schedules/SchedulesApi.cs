using MBS_COMMAND.Application.Abstractions;
using MBS_COMMAND.Contract.Services.Schedule;
using MBS_COMMAND.Presentation.Abstractions;
using MBS_COMMAND.Presentation.Constrants;
using Microsoft.AspNetCore.Authentication;

namespace MBS_COMMAND.Presentation.APIs.Schedules;
public class SchedulesApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/schedules";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var gr1 = app.NewVersionedApi("Schedules")
            .MapGroup(BaseUrl).HasApiVersion(1);

        gr1.MapPost(String.Empty, CreateSchedules)
            .RequireAuthorization(RoleNames.Student);

        gr1.MapPut(String.Empty, UpdateSchedules)
            .RequireAuthorization(RoleNames.Student);

        gr1.MapDelete(String.Empty, DeleteSchedules)
            .RequireAuthorization(RoleNames.Student);
    }
    private static async Task<IResult> UpdateStats(Isender sender, Command.AcceptScheduleCommand command)
    {
        var result = await sender.Send(command);
        return result.IsFailure ? HandlerFailure(result) : Results.Ok(result);


}

    private static async Task<IResult> AcceptSchedule(ISender sender, Command.AcceptScheduleCommand command)
    {
        var result = await sender.Send(command);
        return result.IsFailure ? HandlerFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> CreateSchedules(ISender sender, HttpContext context,
        IJwtTokenService jwtTokenService,
        [FromBody] Command.CreateScheduleCommand command)
    {
        var accessToken = await context.GetTokenAsync("access_token");
        var (claimPrincipal, _) = jwtTokenService.GetPrincipalFromExpiredToken(accessToken!);
        var userId = claimPrincipal.Claims.FirstOrDefault(c => c.Type == "UserId")!.Value;

        command.UserId = new Guid(userId);

        var result = await sender.Send(command);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    public static async Task<IResult> UpdateSchedules(ISender sender, HttpContext context,
        IJwtTokenService jwtTokenService,
        [FromBody] Command.UpdateScheduleCommand command)
    {
        var accessToken = await context.GetTokenAsync("access_token");
        var (claimPrincipal, _) = jwtTokenService.GetPrincipalFromExpiredToken(accessToken!);
        var userId = claimPrincipal.Claims.FirstOrDefault(c => c.Type == "UserId")!.Value;

        command.UserId = new Guid(userId);

        var result = await sender.Send(command);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    public static async Task<IResult> DeleteSchedules(ISender sender, HttpContext context,
        IJwtTokenService jwtTokenService,
        [FromBody] Command.DeleteScheduleCommand command)
    {
        var accessToken = await context.GetTokenAsync("access_token");
        var (claimPrincipal, _) = jwtTokenService.GetPrincipalFromExpiredToken(accessToken!);
        var userId = claimPrincipal.Claims.FirstOrDefault(c => c.Type == "UserId")!.Value;

        command.UserId = new Guid(userId);

        var result = await sender.Send(command);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }
}