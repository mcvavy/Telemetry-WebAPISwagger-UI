using System;
using MANOR.Core.Entities;

namespace MANOR.Core.Interfaces
{
    public interface ITelemetry
    {
        IComparable Id { get; set; }
        DateTime TimeStamp { get; set; }
        Lap Lap { get; set; }
        Car Car { get; set; }
    }
}