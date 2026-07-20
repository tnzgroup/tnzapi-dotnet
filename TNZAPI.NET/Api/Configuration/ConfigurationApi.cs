using TNZAPI.NET.Api.Configuration.OptOut;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces;
using TNZAPI.NET.Core.Interfaces.Configuration;

namespace TNZAPI.NET.Api.Configuration
{
    public class ConfigurationApi : IConfigurationApi
    {
        public IOptOutApi OptOut { get; set; }

        public ConfigurationApi(ITNZAuth auth)
        {
            OptOut = new OptOutApi(auth);
        }
    }
}