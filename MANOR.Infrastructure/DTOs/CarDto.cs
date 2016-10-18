using System.ComponentModel.DataAnnotations;

namespace MANOR.Infrastructure.DTOs
{
    public class CarDto
    {
        [Required]
        public string Chassis { get; set; }
        [Required]
        public double TyreTemp { get; set; }
        [Required]
        public double TyreDeg { get; set; }
        [Required]
        public double Fuel { get; set; }
        [Required]
        public double Weight { get; set; }
    }
}