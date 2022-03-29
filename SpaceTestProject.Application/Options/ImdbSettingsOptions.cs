using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceTestProject.Application.Options
{
    public class ImdbSettingsOptions
    {
        public const string SECTION_NAME = "ImdbSettings";
        public string ApiKey { get; set; }
    }
}
