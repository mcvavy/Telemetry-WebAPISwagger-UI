using MANOR.Core.Entities;
using MANOR.Core.Interfaces;
using MANOR.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MANOR.Infrastructure.Repository
{
    public class TelemetryRepository : IRepository<Telemetry>, ITelemetryRepository
    {

        private IReadData _readData;
        public List<Telemetry> ListOfAllTelemetries { get; set; }
        public List<Telemetry> NewTelemetries { get; set; }

        public TelemetryRepository(IReadData readData)
        {
            _readData = readData;
            ListOfAllTelemetries = new List<Telemetry>();
            NewTelemetries = new List<Telemetry>();
            try
            {
                ListOfAllTelemetries = _readData.ReadAllData();
            }
            catch (Exception ex)
            {
                var messh = ex.InnerException.Message;
            }
        }



        public virtual IQueryable<Telemetry> Get()
        {

            return ListOfAllTelemetries.AsQueryable();
        }

        public virtual Telemetry GetById(IComparable id)
        {
            return ListOfAllTelemetries.FirstOrDefault(x => x.Id.Equals(id));
        }

        public virtual void Create(Telemetry entity)
        {
            NewTelemetries.Add(entity);
        }

        public virtual void Update(IComparable id, Telemetry entity)
        {
            var toUpdate = NewTelemetries.FirstOrDefault(x => x.Id.Equals(id));

            if (toUpdate != null)
                toUpdate = entity;
        }

        public virtual void Delete(IComparable id)
        {
            ListOfAllTelemetries.Remove(ListOfAllTelemetries.FirstOrDefault(x => x.Id.Equals(id)));
        }
    }
}