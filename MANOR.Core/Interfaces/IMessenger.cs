using System.Collections.Generic;
using System.Threading.Tasks;

namespace MANOR.Core.Interfaces
{
    public interface IMessenger<T> where T : new()
    {
        void SendMessage(T telemetry);
        Task<List<T>> GetMessagesAsync();
    }
}