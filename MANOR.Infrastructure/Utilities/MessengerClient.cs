using MANOR.Core.Interfaces;
using MANOR.Infrastructure.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace MANOR.Infrastructure.Utilities
{

    public abstract class MessengerClient : IMessenger<TelemetryDto>
    {
        private const string Mqname = @".\private$\ManorQueue";

        private readonly MessageQueue _mq;

        public MessengerClient()
        {
            var services = ServiceController.GetServices().ToList();
            ServiceController msQue = services.Find(o => o.ServiceName == "MSMQ");
            if (msQue?.Status == ServiceControllerStatus.Running)
            {
                if (!MessageQueue.Exists(Mqname))
                {
                    MessageQueue.Create(Mqname);
                }

                _mq = new MessageQueue(@"FormatName:direct=os:" + Mqname)
                {
                    Formatter = new XmlMessageFormatter(new Type[] { typeof(TelemetryDto) })
                };
            }
        }
        public void SendMessage(TelemetryDto telemetry)
        {
            _mq.Send(telemetry);
        }

        public async Task<List<TelemetryDto>> GetMessagesAsync()
        {
            var messages = _mq.GetAllMessages();

            return await Task.FromResult(messages.Select(entry => (TelemetryDto)_mq.Receive()?.Body).ToList());
        }
    }
}