using MANOR.Infrastructure.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MANOR.Infrastructure.Interfaces
{
    public interface IMessenger
    {
        void SendMessage(TelemetryDto telemetry);
        Task<List<TelemetryDto>> GetMessagesAsync();
    }
}