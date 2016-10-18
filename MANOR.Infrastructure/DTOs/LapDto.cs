using System;
using System.ComponentModel.DataAnnotations;

namespace MANOR.Infrastructure.DTOs
{
    public class LapDto
    {
        [Required]
        public int Number { get; set; }
        [Required]
        public TimeSpan Timespan { get; set; }
    }
}