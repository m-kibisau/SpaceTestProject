using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SpaceTestProject.Application.Options;
using SpaceTestProject.Application.RemindingEmails.SendToAllUsers;
using SpaceTestProject.Application.Services.RemindingEmailService;
using SpaceTestProject.Application.WatchListItems.Queries.GetNotWatchedByUserId;

namespace SpaceTestProject.Api.BackgroundServices
{
    public class EmailRemindingBackgroundService : BackgroundService
    {
        private readonly ILogger<EmailRemindingBackgroundService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly EmailRemindingBackgroundServiceOptions _options;

        public EmailRemindingBackgroundService(IServiceScopeFactory serviceScopeFactory, 
            ILogger<EmailRemindingBackgroundService> logger, 
            IOptions<EmailRemindingBackgroundServiceOptions> options)
        {
            _scopeFactory = serviceScopeFactory;
            _logger = logger;
            _options = options.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Background service is started");

            var nearestSendingTime = _options.TimeOfFirstRemindingEmail.ToUniversalTime();

            var nowTime = DateTime.UtcNow;

            if (nearestSendingTime < nowTime)
            {
                var difference = (nowTime - nearestSendingTime).Ticks;

                var countIntervals = Math.Ceiling((double)difference / (double)_options.IntervalBetweenRemindingEmails.Ticks);
                
                nearestSendingTime = nearestSendingTime.Add(_options.IntervalBetweenRemindingEmails * countIntervals);
            }

            var delayTime = nearestSendingTime - nowTime;

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Time to completion is {0}", delayTime);
                await Task.Delay(delayTime, stoppingToken);

                _logger.LogInformation("Start sending reminding emails");

                using var scope = _scopeFactory.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                var result = await mediator.Send(new SendRemindingEmailsToAllUsersCommand(_options.TimeToRepeatRemindingEmail), stoppingToken);
                
                delayTime = _options.IntervalBetweenRemindingEmails;
            }

            _logger.LogInformation("Background service is stoped");
        }

    }
}
