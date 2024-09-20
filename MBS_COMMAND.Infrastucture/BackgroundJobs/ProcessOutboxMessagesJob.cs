using MassTransit;
using MBS_COMMAND.Persistence;
using Quartz;

namespace MBS_COMMAND.Infrastucture.BackgroundJobs;

public class ProcessOutboxMessagesJob : IJob
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IPublishEndpoint _publishEndpoint; // Maybe can use more Rebus library

    public ProcessOutboxMessagesJob(ApplicationDbContext dbContext, IPublishEndpoint publishEndpoint)
    {
        _dbContext = dbContext;
        _publishEndpoint = publishEndpoint;
    }

    public Task Execute(IJobExecutionContext context)
    {
        throw new NotImplementedException();
    }
}