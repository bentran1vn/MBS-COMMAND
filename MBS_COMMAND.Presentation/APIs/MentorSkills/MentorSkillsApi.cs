using MBS_COMMAND.Application.Abstractions;
using MBS_COMMAND.Presentation.Abstractions;
using MBS_COMMAND.Presentation.Constrants;
using Microsoft.AspNetCore.Authentication;

namespace MBS_COMMAND.Presentation.APIs.MentorSkills;

using CommandV1 = MBS_COMMAND.Contract.Services.MentorSkills.Command;

public class MentorSkillsApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/mentorSkills";
    
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var gr1 = app.NewVersionedApi("MentorSkills")
            .MapGroup(BaseUrl).HasApiVersion(1);
        
        gr1.MapPost("", CreateMentorSkills)
            .RequireAuthorization(RoleNames.Mentor)
            .Accepts<CommandV1.CreateMentorSkillsCommand>("multipart/form-data")
            .DisableAntiforgery();
    }
    
    public static async Task<IResult> CreateMentorSkills(ISender sender, [FromForm] CommandV1.CreateMentorSkillsCommand command,
        HttpContext context, IJwtTokenService jwtTokenService)
    {
        var accessToken = await context.GetTokenAsync("access_token");
        var (claimPrincipal, _)  = jwtTokenService.GetPrincipalFromExpiredToken(accessToken!);
        var userId = claimPrincipal.Claims.FirstOrDefault(c => c.Type == "UserId")!.Value;
        
        command.MentorId = new Guid(userId);
        
        var result = await sender.Send(command);
         
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }
}