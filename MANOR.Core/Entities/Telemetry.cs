using MANOR.Core.Interfaces;
using System;

namespace MANOR.Core.Entities
{
    public class Telemetry : ITelemetry
    {
        public IComparable Id { get; set; } = Guid.NewGuid();
        public DateTime TimeStamp { get; set; }
        public Lap Lap { get; set; }
        public Car Car { get; set; }
    }
}