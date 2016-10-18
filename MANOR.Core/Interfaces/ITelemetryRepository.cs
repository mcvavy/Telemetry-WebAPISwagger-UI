using MANOR.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MANOR.Core.Interfaces
{
    public interface ITelemetryRepository
    {
        List<Telemetry> ListOfAllTelemetries { get; set; }
        List<Telemetry> NewTelemetries { get; set; }
        IQueryable<Telemetry> Get();
        Telemetry GetById(IComparable id);
        void Create(Telemetry entity);
        void Update(IComparable id, Telemetry entity);
        void Delete(IComparable id);
    }
}