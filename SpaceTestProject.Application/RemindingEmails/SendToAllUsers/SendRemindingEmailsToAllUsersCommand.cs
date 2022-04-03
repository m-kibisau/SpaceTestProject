using System;
using System.Threading.Tasks;
using MediatR;

namespace SpaceTestProject.Application.RemindingEmails.SendToAllUsers
{
    public class SendRemindingEmailsToAllUsersCommand : IRequest<bool>
    {
        public TimeSpan TimeToRepeat { get; set; }

        public SendRemindingEmailsToAllUsersCommand(TimeSpan timeToRepeat)
        {
            TimeToRepeat = timeToRepeat;
        }
    }
}
