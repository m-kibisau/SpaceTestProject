using System;

namespace SpaceTestProject.Application.Options
{
    public class EmailRemindingBackgroundServiceOptions
    {
        public const string SECTION_NAME = "EmailRemindingBackgroundServiceSettings";

        public DateTime TimeOfFirstRemindingEmail { get; set; }
        public TimeSpan IntervalBetweenRemindingEmails { get; set; }
        public TimeSpan TimeToRepeatRemindingEmail { get; set; }
    }
}
