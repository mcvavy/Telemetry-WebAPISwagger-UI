using MANOR.Infrastructure.Interfaces;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace MANOR.API.Controllers
{
    public class VisualizedataController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public VisualizedataController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        // GET: Visualizedata
        public ActionResult Visuallize()
        {
            GenerateChartData();

            return View();
        }

        private void GenerateChartData()
        {
            var dataSource =
                _unitOfWork.TelemetryRepository.ListOfAllTelemetries.OrderBy(x => x.Car.Chassis).GroupBy(x => x.Lap.Number);

            StringBuilder strScript = new StringBuilder();
            strScript.Append(@"<script type='text/javascript'>  
                    google.load('visualization', '1', {packages: ['corechart']});</script>  
  
                    <script type='text/javascript'>  
                    function drawVisualization() {         
                    var data = google.visualization.arrayToDataTable([  
                    ['Lap', 'CH1', 'CH2', 'Average'],");

            var mainstrings = new StringBuilder();

            foreach (var data in dataSource)
            {
                var innerstrings = new StringBuilder();

                innerstrings.Append("['" + "Lap " + data.Key + "',");

                int count = 0;
                double sum = 0;

                foreach (var x in data)
                {
                    sum += x.Car.TyreTemp;
                    innerstrings.Append(x.Car.TyreTemp + ",");

                    count++;
                }

                if (count == 1)
                {
                    innerstrings.Append(0 + ",");
                }

                innerstrings.Append((double)sum / count + "],");

                mainstrings.Append(innerstrings);
                count = 0;
                sum = 0;
            }

            mainstrings.Remove(mainstrings.Length - 1, 1);
            strScript.Append(mainstrings).Append("]);");

            strScript.Append(
                "var options = { title : 'Tyre Temperatures for CH1 and CH2', vAxis: {title: 'Temperatures'},  chartArea: {width: 900,height: 350},hAxis: {title: 'Laps'}, seriesType: 'lines', series: {3: {type: 'line'}} };");
            strScript.Append(
                " var chart = new google.visualization.ComboChart(document.getElementById('chart_div'));  chart.draw(data, options); } google.setOnLoadCallback(drawVisualization);");
            strScript.Append(" </script>");

            string final = strScript.ToString();

            ViewBag.chartstring = final;
        }
    }
}
