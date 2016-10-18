using MANOR.Core.Interfaces;
using MANOR.Infrastructure.Interfaces;
using MANOR.Infrastructure.Utilities;

namespace MANOR.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public ITelemetryRepository TelemetryRepository { get; }

        public IMessenger Messenger { get; }

        public UnitOfWork(IReadData readData)
        {
            Messenger = new Messenger();
            TelemetryRepository = new TelemetryRepository(readData);
        }
    }
}
