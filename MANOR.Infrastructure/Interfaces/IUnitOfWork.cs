using MANOR.Core.Interfaces;

namespace MANOR.Infrastructure.Interfaces
{
    public interface IUnitOfWork
    {
        ITelemetryRepository TelemetryRepository { get; }
        IMessenger Messenger { get; }
    }
}