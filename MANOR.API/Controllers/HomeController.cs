using AutoMapper;
using MANOR.API.ViewModels;
using MANOR.Infrastructure.DTOs;
using MANOR.Infrastructure.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MANOR.API.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public async Task<ActionResult> Compare()
        {

            return View(new LapViewModel
            {
                TelemetryModel = new TelemetryViewModel
                {
                    Chone = await SelectListItemCreator(
                            Mapper.Map<IEnumerable<TelemetryDto>>(_unitOfWork.TelemetryRepository.ListOfAllTelemetries),
                            "CH1"),
                    Chtwo = await SelectListItemCreator(
                            Mapper.Map<IEnumerable<TelemetryDto>>(_unitOfWork.TelemetryRepository.ListOfAllTelemetries),
                            "CH2")
                }
            });
        }

        [HttpPost]
        public async Task<ActionResult> Comparelaps(LapViewModel model)
        {
            if (ModelState.IsValid)
            {
                var ch1 = int.Parse(model.TelemetryModel.SelectedCh1);
                var ch2 = int.Parse(model.TelemetryModel.SelectedCh2);

                if (ch1 != 0 && ch2 != 0)
                {
                    return PartialView("_lapcomparison",
                        new LapViewModel
                        {
                            LapModel = new List<TelemetryDto>()
                            {
                                await GenerateComparisonFromFormDataAsync(ch1, "CH1"),
                                await GenerateComparisonFromFormDataAsync(ch2, "CH2")
                            }
                        });
                }
                else
                {
                    return RedirectToAction("Compare");

                }
            }

            return RedirectToAction("Compare");
        }

        private async Task<TelemetryDto> GenerateComparisonFromFormDataAsync(int lapnumber, string chasis)
        {
            return await Task.FromResult(Mapper.Map<TelemetryDto>(
                _unitOfWork.TelemetryRepository
                    .ListOfAllTelemetries
                    .FirstOrDefault(x => x.Lap.Number == lapnumber && x.Car.Chassis.Equals(chasis))));
        }


        private static async Task<IEnumerable<SelectListItem>> SelectListItemCreator(IEnumerable<TelemetryDto> telemetryList, string chasis)
        {
            return await Task.FromResult(telemetryList.Where(x => x.Car.Chassis.Equals(chasis)).Select(dto => new SelectListItem()
            {
                Text = dto.Lap.Number.ToString(),
                Value = dto.Lap.Number.ToString()
            }).ToList());

        }
    }
}
