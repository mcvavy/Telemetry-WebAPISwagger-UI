using AutoMapper;
using MANOR.Core.Entities;
using MANOR.Infrastructure.DTOs;
using MANOR.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;


namespace MANOR.API.Controllers
{
    [RoutePrefix("api/Telemetry")]
    public class TelemetryController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;


        public TelemetryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        //In memory stub for adding new telemetry data
        //Not best practice for using session in web api, 
        //but it serves the purpose of the functionality required
        public List<Telemetry> NewLyAddedtelemetries
        {
            get
            {

                if (HttpContext.Current.Session["newtelemetry"] == null)
                    HttpContext.Current.Session["newtelemetry"] = _unitOfWork.TelemetryRepository.ListOfAllTelemetries;

                return (List<Telemetry>)HttpContext.Current.Session["newtelemetry"];
            }
            set { HttpContext.Current.Session["newtelemetry"] = value; }
        }

        // GET: api/telemetry
        [HttpGet]
        [Route("")]
        [ResponseType(typeof(TelemetryDto))]
        public async Task<IEnumerable<TelemetryDto>> Get()
        {
            return await Task.FromResult(Mapper.Map<IEnumerable<TelemetryDto>>(_unitOfWork.TelemetryRepository.Get()));

        }

        // GET api/telemetry/chassis/CH1
        [HttpGet]
        [Route("chassis/{chassis}")]
        [ResponseType(typeof(TelemetryDto))]
        public async Task<IEnumerable<TelemetryDto>> GetByChassis(string chassis)
        {
            return
                await
                    Task.FromResult(
                        Mapper.Map<IEnumerable<TelemetryDto>>(
                            _unitOfWork.TelemetryRepository.Get().Where(p => p.Car.Chassis == chassis.ToUpper())));
        }

        // GET api/telemetry/lap/23
        [HttpGet]
        [Route("lap/{lap:int}")]
        [ResponseType(typeof(TelemetryDto))]
        public async Task<IEnumerable<TelemetryDto>> GetByLap(int lap)
        {
            return
                await
                    Task.FromResult(
                        Mapper.Map<IEnumerable<TelemetryDto>>(
                            _unitOfWork.TelemetryRepository.Get().Where(x => x.Lap.Number == lap)));
        }

        //GET api/telemetry/lap/fast
        [HttpGet]
        [Route("lap/fast")]
        [ResponseType(typeof(TelemetryDto))]
        public async Task<TelemetryDto> FastestLap()
        {


            return
                await
                    Task.FromResult(
                        Mapper.Map<List<TelemetryDto>>(_unitOfWork.TelemetryRepository.Get())
                            .OrderBy(x => x.Lap.Timespan)
                            .FirstOrDefault());
        }

        // GET api/telemetry/lap/{newlyAddedTelemetry}/lap
        [HttpGet]
        [Route("lap/{newlyAddedTelemetry:int}/lap")]
        [ResponseType(typeof(TelemetryDto))]
        public async Task<IEnumerable<TelemetryDto>> GetByNewLap(int newlyAddedTelemetry)
        {
            return
                await
                    Task.FromResult(
                        Mapper.Map<IEnumerable<TelemetryDto>>(NewLyAddedtelemetries.Where(x => x.Lap.Number == newlyAddedTelemetry)));
        }

        //POST api/Telemetry
        [HttpPost]
        [Route("", Name = "CreateLap")]
        [ResponseType(typeof(TelemetryDto))]
        public async Task<IHttpActionResult> Post([FromBody] TelemetryDto telemetry)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //check the message Queue for messages
            await CheckQueueForTelemetries();

            if (CheckTelemetryRules(telemetry, "CH1"))
                return await Task.FromResult(HttpActionResult(telemetry));

            if (CheckTelemetryRules(telemetry, "CH2"))
                return await Task.FromResult(HttpActionResult(telemetry));

            return new BadRequestErrorMessageResult("Duplicate not allowed. A lap can track a car at most once", this);
        }

        #region PRIVATE METHODS FOR HANDLING TELEMETRY POST REQUESTS

        private bool CheckTelemetryRules(ITelemetryDto telemetry, string chasis)
        {
            return NewLyAddedtelemetries.Where(x => x.Lap.Number == telemetry.Lap.Number)
                       .Count(x => x.Car.Chassis == chasis)
                       .Equals(0) && telemetry.Car.Chassis == chasis;
        }

        private IHttpActionResult HttpActionResult(TelemetryDto telemetry)
        {
            var newTelemetryData = Mapper.Map<Telemetry>(telemetry);
            try
            {
                //throw  new ServerException("Repository is down!");
                NewLyAddedtelemetries.Add(newTelemetryData);
            }
            catch (Exception ex)
            {
                _unitOfWork.Messenger.SendMessage(telemetry);

                return Ok(new
                {
                    Information =
                    "Repository currently offline. Your data is in a queue and will be stored in the Repo as soon as it's back online"
                });
            }


            return CreatedAtRoute("CreateLap", new { Id = Mapper.Map<Telemetry>(telemetry).Id },
                Mapper.Map<Telemetry>(telemetry));
        }
        private async Task CheckQueueForTelemetries()
        {
            try
            {
                var messagesFromQueue = await _unitOfWork.Messenger.GetMessagesAsync();
                if (messagesFromQueue != null && messagesFromQueue.Count >= 1)
                    NewLyAddedtelemetries.AddRange(Mapper.Map<List<Telemetry>>(messagesFromQueue));
            }
            catch (Exception ex)
            {
                var queuefail = ex.InnerException.Message;
            }
        }

        #endregion
    }
}
