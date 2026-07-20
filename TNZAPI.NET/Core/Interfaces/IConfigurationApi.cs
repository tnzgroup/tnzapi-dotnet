using TNZAPI.NET.Core.Interfaces.Configuration;

namespace TNZAPI.NET.Core.Interfaces
{
    public interface IConfigurationApi
    {
        IOptOutApi OptOut { get; set; }
    }
}