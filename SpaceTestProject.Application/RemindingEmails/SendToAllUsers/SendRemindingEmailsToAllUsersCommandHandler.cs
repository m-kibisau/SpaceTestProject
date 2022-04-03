using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SpaceTestProject.Application.Models.ImdbApiService;
using SpaceTestProject.Application.Models.WatchListItems;
using SpaceTestProject.Application.Options;
using SpaceTestProject.Application.Services.ImdbApiService;
using SpaceTestProject.Application.Services.RemindingEmailService;
using SpaceTestProject.Application.WatchListEmailLogs.Specifications;
using SpaceTestProject.Application.WatchListItems.Queries.GetNotWatchedByUserId;
using SpaceTestProject.Domain;
using SpaceTestProject.Persistence.Abstractions;

namespace SpaceTestProject.Application.RemindingEmails.SendToAllUsers
{
    public class SendRemindingEmailsToAllUsersCommandHandler : IRequestHandler<SendRemindingEmailsToAllUsersCommand, bool>
    {
        private readonly ILogger<SendRemindingEmailsToAllUsersCommandHandler> _logger;
        private readonly IRepository<WatchListItem> _itemRepository;
        private readonly IRepository<WatchListEmailLog> _emailLogRepository;

        private readonly IMediator _mediator;
        private readonly IImdbApiService _imdbApiService;
        private readonly SmtpClientOptions _options;

        public SendRemindingEmailsToAllUsersCommandHandler(ILogger<SendRemindingEmailsToAllUsersCommandHandler> logger, 
            IRepository<WatchListItem> itemRepository,
            IRepository<WatchListEmailLog> emailLogRepository,
            IMediator mediator, 
            IImdbApiService imdbApiService, 
            IOptions<SmtpClientOptions> options)
        {
            _logger = logger;
            _itemRepository = itemRepository;
            _emailLogRepository = emailLogRepository;
            _mediator = mediator;
            _imdbApiService = imdbApiService;
            _options = options.Value;
        }

        public async Task<bool> Handle(SendRemindingEmailsToAllUsersCommand request,
            CancellationToken cancellationToken)
        {
            var userIds = (await _itemRepository.GetAllAsync())
                .Select(x => x.UserId)
                .Distinct();

            foreach (var userId in userIds)
            {
                var notWatchedTitleResult = await _mediator.Send(new GetNotWatchedWatchListItemsByUserIdQuery(userId));

                if (!notWatchedTitleResult.IsSuccess())
                {
                    _logger.LogError("Error to get not watched titles for user with id {0}", userId);
                    continue;
                }

                if (notWatchedTitleResult.Data.Count <= 3)
                {
                    continue;
                }

                var logsForPeriod = await _emailLogRepository.GetAllAsync(
                    new GetWatchListEmailLogsByDateRangeSpecification(DateTime.UtcNow,
                        DateTime.UtcNow.Add(-request.TimeToRepeat)));

                var unsentTitles =
                    notWatchedTitleResult.Data.Where(dto => logsForPeriod.All(log => log.WatchListId != dto.Id))
                        .ToList();

                var titleToRemind = (await GetTitleWithHighestRating(unsentTitles));

                var message = await CreateEmailFromTitle(titleToRemind.Title);

                var sendResult = await SendEmail(message);

                if (sendResult)
                {
                    await WriteEmailLog(titleToRemind.ItemId);
                }
            }
            return true;
        }

        private async Task<MailMessage> CreateEmailFromTitle(TitleData titleData)
        {
            var posterLink = (await _imdbApiService.GetPosterById(titleData.Id)).Posters.FirstOrDefault()?.Link;
            var wikiData = await _imdbApiService.GetWikipediaDescriptionById(titleData.Id);

            var body = string.Format("<html><body><p align=\"center\">{0}</p><p align=\"center\">IMDB Rating - {1}</p><p align=\"center\"><img src =\"{2}\" width=\"500\" height=\"750\"></p>{3}",
                titleData.FullTitle, titleData.IMDbRating, posterLink, wikiData.PlotShort.Html);

            var message = new MailMessage(new MailAddress(_options.Username, _options.SenderName),
                new MailAddress("m.kibisau@andersenlab.com"))
            {
                IsBodyHtml = true,
                Body = body,
                Subject = "Watch list reminding"
            };

            return message;
        }

        private async Task<bool> SendEmail(MailMessage message)
        {
            try
            {
                var client = new SmtpClient(_options.Host, _options.Port);
                client.Credentials = new NetworkCredential(_options.Username, _options.Password);
                client.EnableSsl = true;

                _logger.LogInformation("Sending message for email {Email}, with subject {Subject}",
                    message.To.First().Address, message.Subject);
                await client.SendMailAsync(message);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to send message for email {Email}, with subject {Subject}",
                    message.To.First().Address, message.Subject);

                return false;
            }
        }

        private async Task WriteEmailLog(Guid watchListItemId)
        {
            var createResult = await _emailLogRepository.CreateAsync(new WatchListEmailLog()
            {
                SendingTime = DateTime.UtcNow,
                WatchListId = watchListItemId
            });

            if (!createResult)
            {
                _logger.LogError("Failed to add email log to database");
            }
        }

        private async Task<WatchListItemDtoRating> GetTitleWithHighestRating(List<WatchListItemDto> watchListItemDtos)
        {
            WatchListItemDtoRating resultTitle = null;
            foreach (var item in watchListItemDtos)
            {
                var title = (await _imdbApiService.GetTitleById(item.TitleId));
                if (!string.IsNullOrEmpty(title.ErrorMessage))
                {
                    _logger.LogError("Error to get information about title with id {0}", item.TitleId);
                    continue;
                }
                
                double.TryParse(title.IMDbRating.Replace(".", ","), out var titleRating);

                if (resultTitle == null)
                {
                    resultTitle = new WatchListItemDtoRating(title, titleRating, item.Id);
                    continue;
                }

                if (titleRating > resultTitle.Rating)
                {
                    resultTitle = new WatchListItemDtoRating(title, titleRating, item.Id);
                }
            }

            return resultTitle;
        }

        private record WatchListItemDtoRating(TitleData Title, double Rating, Guid ItemId);
    }
}
