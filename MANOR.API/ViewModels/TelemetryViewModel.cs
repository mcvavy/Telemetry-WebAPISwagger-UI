using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MANOR.API.ViewModels
{
    public interface ITelemetryViewModel
    {
        string SelectedCh1 { get; set; }
        string SelectedCh2 { get; set; }
        IEnumerable<SelectListItem> Chone { get; set; }
        IEnumerable<SelectListItem> Chtwo { get; set; }
    }

    public class TelemetryViewModel : ITelemetryViewModel
    {
        [Required]
        public string SelectedCh1 { get; set; }
        [Required]
        public string SelectedCh2 { get; set; }

        public IEnumerable<SelectListItem> Chone { get; set; }
        public IEnumerable<SelectListItem> Chtwo { get; set; }
    }
}