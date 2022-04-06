namespace SpaceTestProject.Application.Options
{
    public class ImdbSettingsOptions
    {
        public const string SECTION_NAME = "ImdbSettings";

        public string ApiKey { get; set; }
        public string BaseImdbUrl { get; set; }
    }
}
