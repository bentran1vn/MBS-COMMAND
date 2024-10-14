using MBS_COMMAND.Presentation.Abstractions;

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
            .Accepts<CommandV1.CreateMentorSkillsCommand>("multipart/form-data")
            .DisableAntiforgery();
    }
    
    public static async Task<IResult> CreateMentorSkills(ISender sender, [FromForm] CommandV1.CreateMentorSkillsCommand command)
    {
        command.MentorId = new Guid("c74174e2-ce31-48fb-c3ba-08dce3193e4b");
        var result = await sender.Send(command);
        
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }
}