using MassTransit;
using MBS_COMMAND.Contract.Abstractions.Messages;
using MBS_COMMAND.Contract.Services.MentorSkills;
using MBS_COMMAND.Persistence;
using MBS_COMMAND.Persistence.Outbox;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quartz;
using MessageShared = MBS_CONTRACT.SHARE.Abstractions.Messages;
using ServicesShared = MBS_CONTRACT.SHARE.Services;

namespace MBS_COMMAND.Infrastucture.BackgroundJobs;
[DisallowConcurrentExecution]
public class ProcessOutboxMessagesJob : IJob
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IPublishEndpoint _publishEndpoint; // Maybe can use more Rebus library

    public ProcessOutboxMessagesJob(ApplicationDbContext dbContext, IPublishEndpoint publishEndpoint)
    {
        _dbContext = dbContext;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        List<OutboxMessage> messages = await _dbContext
            .Set<OutboxMessage>()
            .Where(m => m.ProcessedOnUtc == null)
            .OrderBy(m => m.OccurredOnUtc)
            .Take(20)
            .ToListAsync(context.CancellationToken);

        foreach (OutboxMessage outboxMessage in messages)
        {
            MessageShared.IDomainEvent? domainEvent = JsonConvert
                .DeserializeObject<MessageShared.IDomainEvent>(
                    outboxMessage.Content,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });

            if (domainEvent is null)
                continue;

            try
            {
                switch (domainEvent.GetType().Name)
                {
                    case nameof(ServicesShared.MentorSkills.DomainEvent.MentorSkillsCreated):
                        var mentorSkillsCreated =
                            JsonConvert.DeserializeObject<ServicesShared.MentorSkills.DomainEvent.MentorSkillsCreated>(
                                outboxMessage.Content,
                                new JsonSerializerSettings
                                {
                                    TypeNameHandling = TypeNameHandling.All
                                });
                        await _publishEndpoint.Publish(mentorSkillsCreated, context.CancellationToken);
                        break;

                    case nameof(ServicesShared.Mentors.DomainEvent.MentorCreated):
                        var mentorCreated =
                            JsonConvert.DeserializeObject<ServicesShared.Mentors.DomainEvent.MentorCreated>(
                                outboxMessage.Content,
                                new JsonSerializerSettings
                                {
                                    TypeNameHandling = TypeNameHandling.All
                                });
                        await _publishEndpoint.Publish(mentorCreated, context.CancellationToken);
                        break;
                    case nameof(ServicesShared.Users.DomainEvent.MentorSlotCreated):
                        var MentorSlotCreated =
                            JsonConvert.DeserializeObject<ServicesShared.Users.DomainEvent.MentorSlotCreated>(
                                outboxMessage.Content,
                                new JsonSerializerSettings
                                {
                                    TypeNameHandling = TypeNameHandling.All
                                });
                        await _publishEndpoint.Publish(MentorSlotCreated, context.CancellationToken);
                        break;
                    case nameof(ServicesShared.Slots.DomainEvent.ChangeSlotStatusInToBooked):
                        var ChangeSlotStatusInToBooked =
                            JsonConvert.DeserializeObject<ServicesShared.Slots.DomainEvent.ChangeSlotStatusInToBooked>(
                                outboxMessage.Content,
                                new JsonSerializerSettings
                                {
                                    TypeNameHandling = TypeNameHandling.All
                                });


                        await _publishEndpoint.Publish(ChangeSlotStatusInToBooked, context.CancellationToken);
                        break;
                    case nameof(ServicesShared.Slots.DomainEvent.SlotUpdated):
                        var SlotUpdated =
                            JsonConvert.DeserializeObject<ServicesShared.Slots.DomainEvent.SlotUpdated>(
                                outboxMessage.Content,
                                new JsonSerializerSettings
                                {
                                    TypeNameHandling = TypeNameHandling.All
                                });
                        await _publishEndpoint.Publish(SlotUpdated, context.CancellationToken);
                        Console.WriteLine("SlotUpdated");
                        Console.BackgroundColor = ConsoleColor.Red;
                        break;
                }

                outboxMessage.ProcessedOnUtc = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                outboxMessage.Error = ex.Message;
            }
        }

        await _dbContext.SaveChangesAsync();
    }
}