namespace SpaceTestProject.Application.Options
{
    public class SmtpClientOptions
    {
        public const string SECTION_NAME = "SmtpClientOptions";

        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string SenderName { get; set; }
    }
}
