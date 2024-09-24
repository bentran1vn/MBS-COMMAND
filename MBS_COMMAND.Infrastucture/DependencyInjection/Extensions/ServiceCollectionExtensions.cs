using System.Reflection;
using MassTransit;
using MBS_COMMAND.Application.Abstractions;
using MBS_COMMAND.Contract.JsonConverters;
using MBS_COMMAND.Infrastucture.BackgroundJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Quartz;
using MBS_COMMAND.Infrastucture.Caching;
using MBS_COMMAND.Infrastucture.DependencyInjection.Options;
using MBS_COMMAND.Infrastucture.PipeObservers;

namespace MBS_COMMAND.Infrastucture.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    // Configure MongoDB
    // public static void ConfigureServicesInfrastructure(this IServiceCollection services, IConfiguration configuration)
    // {
    //     services.Configure<MongoDbSettings>(configuration.GetSection(nameof(MongoDbSettings)));
    //
    //     services.AddSingleton<IMongoDbSettings>(serviceProvider =>
    //         serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value);
    //
    //     services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));
    // }

    public static void AddServicesInfrastructure(this IServiceCollection services)
        => services.AddTransient<ICacheService, CacheService>();
    
    // Configure Redis
    public static void AddRedisInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(redisOptions =>
        {
            var connectionString = configuration.GetConnectionString("Redis");
            redisOptions.Configuration = connectionString;
        });
    }
    
    // Configure for masstransit with rabbitMQ
    public static IServiceCollection AddMasstransitRabbitMQInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var masstransitConfiguration = new MasstransitConfiguration();
        configuration.GetSection(nameof(MasstransitConfiguration)).Bind(masstransitConfiguration);
    
        var messageBusOption = new MessageBusOptions();
        configuration.GetSection(nameof(MessageBusOptions)).Bind(messageBusOption);
    
        services.AddMassTransit(cfg =>
        {
            // ===================== Setup for Consumer =====================
            cfg.AddConsumers(Assembly.GetExecutingAssembly()); // Add all of consumers to masstransit instead above command
    
            // ?? => Configure endpoint formatter. Not configure for producer Root Exchange
            cfg.SetKebabCaseEndpointNameFormatter(); // ??
    
            cfg.UsingRabbitMq((context, bus) =>
            {
                bus.Host(masstransitConfiguration.Host, masstransitConfiguration.Port, masstransitConfiguration.VHost, h =>
                {
                    h.Username(masstransitConfiguration.UserName);
                    h.Password(masstransitConfiguration.Password);
                });
    
                bus.UseMessageRetry(retry
                => retry.Incremental(
                           retryLimit: messageBusOption.RetryLimit,
                           initialInterval: messageBusOption.InitialInterval,
                           intervalIncrement: messageBusOption.IntervalIncrement));
    
                bus.UseNewtonsoftJsonSerializer();
    
                bus.ConfigureNewtonsoftJsonSerializer(settings =>
                {
                    settings.Converters.Add(new TypeNameHandlingConverter(TypeNameHandling.Objects));
                    settings.Converters.Add(new DateOnlyJsonConverter());
                    settings.Converters.Add(new ExpirationDateOnlyJsonConverter());
                    return settings;
                });
    
                bus.ConfigureNewtonsoftJsonDeserializer(settings =>
                {
                    settings.Converters.Add(new TypeNameHandlingConverter(TypeNameHandling.Objects));
                    settings.Converters.Add(new DateOnlyJsonConverter());
                    settings.Converters.Add(new ExpirationDateOnlyJsonConverter());
                    return settings;
                });
    
                bus.ConnectReceiveObserver(new LoggingReceiveObserver());
                bus.ConnectConsumeObserver(new LoggingConsumeObserver());
                bus.ConnectPublishObserver(new LoggingPublishObserver());
                bus.ConnectSendObserver(new LoggingSendObserver());
    
                // Rename for Root Exchange and setup for consumer also
                bus.MessageTopology.SetEntityNameFormatter(new KebabCaseEntityNameFormatter());
    
                // ===================== Setup for Consumer =====================
    
                // Importantce to create Echange and Queue
                bus.ConfigureEndpoints(context);
            });
        });
    
        return services;
    }
    
    // Configure Job
    public static void AddQuartzInfrastructure(this IServiceCollection services)
    {
        services.AddQuartz(configure =>
        {
            var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));
    
            configure
                .AddJob<ProcessOutboxMessagesJob>(jobKey)
                .AddTrigger(
                    trigger =>
                        trigger.ForJob(jobKey)
                            .WithSimpleSchedule(
                                schedule =>
                                    schedule.WithInterval(TimeSpan.FromMicroseconds(100))
                                        .RepeatForever()));
    
            configure.UseMicrosoftDependencyInjectionJobFactory();
        });
    
        services.AddQuartzHostedService();
    }
    
    // Configure MediatR
    public static void AddMediatRInfrastructure(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(AssemblyReference.Assembly));
    }
}