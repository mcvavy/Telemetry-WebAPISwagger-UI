using MANOR.Infrastructure.DTOs;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MANOR.API.ViewModels
{

    public class LapViewModel
    {
        [Required]
        public TelemetryViewModel TelemetryModel { get; set; }
        public List<TelemetryDto> LapModel { get; set; }
    }
}