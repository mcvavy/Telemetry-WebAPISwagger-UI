using System;
using System.ComponentModel.DataAnnotations;

namespace MANOR.Infrastructure.DTOs
{
    public interface ITelemetryDto
    {
        DateTime TimeStamp { get; set; }
        LapDto Lap { get; set; }
        CarDto Car { get; set; }
    }

    public class TelemetryDto : ITelemetryDto
    {
        [Required]
        public DateTime TimeStamp { get; set; }
        [Required]
        public LapDto Lap { get; set; }
        [Required]
        public CarDto Car { get; set; }
    }
}