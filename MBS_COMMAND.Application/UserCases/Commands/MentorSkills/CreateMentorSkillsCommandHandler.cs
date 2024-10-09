using MBS_AUTHORIZATION.Domain.Entities;
using MBS_COMMAND.Application.Abstractions;
using MBS_COMMAND.Contract.Abstractions.Messages;
using MBS_COMMAND.Contract.Abstractions.Shared;
using MBS_COMMAND.Contract.Services.MentorSkills;
using MBS_COMMAND.Domain.Abstractions.Repositories;
using MBS_COMMAND.Domain.Entities;

namespace MBS_COMMAND.Application.UserCases.Commands.MentorSkills;

public class CreateMentorSkillsCommandHandler : ICommandHandler<Command.CreateMentorSkillsCommand>
{
    private readonly IRepositoryBase<Domain.Entities.MentorSkills, Guid> _mentorSkillsRepository;
    private readonly IRepositoryBase<Skill, Guid> _skillsRepository;
    private readonly IRepositoryBase<User, Guid> _userRepository;
    private readonly IRepositoryBase<Certificate, Guid> _cetificateRepository;
    private readonly IMediaService _mediaService;

    public CreateMentorSkillsCommandHandler(IRepositoryBase<Domain.Entities.MentorSkills, Guid> mentorSkillsRepository, IRepositoryBase<Skill, Guid> skillsRepository, IMediaService mediaService, IRepositoryBase<Certificate, Guid> cetificateRepository, IRepositoryBase<User, Guid> userRepository)
    {
        _mentorSkillsRepository = mentorSkillsRepository;
        _skillsRepository = skillsRepository;
        _mediaService = mediaService;
        _cetificateRepository = cetificateRepository;
        _userRepository = userRepository;
    }

    public async Task<Result> Handle(Command.CreateMentorSkillsCommand request, CancellationToken cancellationToken)
    {
        var skill = await _skillsRepository.FindByIdAsync(request.SkillId, cancellationToken, x => x.Category);

        if (skill is null) return Result.Failure(new Error("404", $"Not Exist Skill with Id: {request.SkillId}"));
        
        var mentor = await _userRepository.FindSingleAsync(x => x.Id.Equals(request.MentorId) && x.IsDeleted == false, cancellationToken);

        if (mentor is null) return Result.Failure(new Error("404", $"Not Mentor with Id: {request.MentorId}"));
        
        var mentorSkill = new Domain.Entities.MentorSkills()
        {
            Id = Guid.NewGuid(),
            SkillId = request.SkillId,
            UserId = request.MentorId,
        };
        
        _mentorSkillsRepository.Add(mentorSkill);
        
        var tasks = request.ProductImages.Select(x => _mediaService.UploadImageAsync(x));
        var result = await Task.WhenAll(tasks);
        
        var certificates = result.Select(x => new Certificate()
        {
            Id = Guid.NewGuid(),
            MentorSkillsId = mentorSkill.Id,
            Name = "12312",
            Description = "123123",
            ImageUrl = x
        }).ToList();
        
        _cetificateRepository.AddRange(certificates);
        
        // mentorSkill.CreateMentor(mentor);
        mentorSkill.CreateMentorSkills(mentor.Id, skill, certificates);

        return Result.Success("Adding Skill For Mentor Successfully !");
    }
}