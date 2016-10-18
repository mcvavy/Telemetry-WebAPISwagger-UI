using MANOR.API.Controllers;
using MANOR.Core.Entities;
using MANOR.Infrastructure.DependencyResolution;
using MANOR.Infrastructure.DTOs;
using MANOR.Infrastructure.Utilities;
using Ninject;
using NUnit.Framework;
using Shouldly;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace MANOR.UnitTest
{
    [TestFixture]
    public class TelemetryControllerTest
    {
        private TelemetryController _controller;
        private StandardKernel _kernel;
        private TelemetryDto _dto;
        private TelemetryDto _dto2;



        [SetUp]
        public void SetUp()
        {

            _kernel = new StandardKernel(new RepositoryModule());
            _controller = _kernel.Get<TelemetryController>();
            MapperConfig.RegisterMapping();


            _dto = new TelemetryDto()
            {
                Car = new CarDto() { Chassis = "CH1", TyreTemp = 89.22, TyreDeg = 0.084334, Fuel = 98.59, Weight = 842.59 },
                Lap = new LapDto() { Number = 100, Timespan = new TimeSpan(0, 1, 45, 9) },
                TimeStamp = DateTime.Parse("2016-09-17T13:02:55.696Z")
            };

            _dto2 = new TelemetryDto()
            {
                Car = new CarDto() { Chassis = "CH2", TyreTemp = 89.22, TyreDeg = 0.084334, Fuel = 98.59, Weight = 842.59 },
                Lap = new LapDto() { Number = 100, Timespan = new TimeSpan(0, 1, 45, 9) },
                TimeStamp = DateTime.Parse("2016-09-17T13:02:55.696Z")
            };





        }

        [TearDown]
        public void TearDown()
        {
            //_controller.Dispose();
        }

        [Test]
        public async Task ShouldReturnAllTelemetryData()
        {

            var result = await _controller.Get();

            result.ShouldNotBeNull();
        }


        [TestCase("CH1")]
        [TestCase("CH2")]
        public async Task ShouldReturnAllTelemetryForASingleCar(string chasis)
        {
            var result = await _controller.GetByChassis(chasis);

            result.ShouldNotBeNull();
        }


        [Test]
        public async Task ShouldReturnAllTelemetryForASingleLap([Range(1, 20)]int lap)
        {
            var result = await _controller.GetByLap(lap);

            result.ShouldNotBeNull();
        }

        [Test]
        public async Task ShouldReturnAllTelemetryForTheFatestLap()
        {
            var result = await _controller.FastestLap();
            result.ShouldNotBeNull();
        }


        [Test]
        public async Task ShouldAddNewTelemetry()
        {
            using (new FakeHttpContext.FakeHttpContext())
            {
                IHttpActionResult result = await _controller.Post(_dto);
                IHttpActionResult res2 = await _controller.Post(_dto2);


                var createdResult = result as CreatedAtRouteNegotiatedContentResult<Telemetry>;
                var createdResult2 = res2 as CreatedAtRouteNegotiatedContentResult<Telemetry>;


                createdResult.ShouldNotBeNull();
                createdResult?.RouteName.ShouldBe("CreateLap");

                createdResult2.ShouldNotBeNull();
                createdResult2?.RouteName.ShouldBe("CreateLap");
            }

        }

        [Test]
        public async Task ShouldReturnBadReQuestIfDuplicateTelemetryPosted()
        {
            using (new FakeHttpContext.FakeHttpContext())
            {
                IHttpActionResult result = await _controller.Post(_dto);
                IHttpActionResult res2 = await _controller.Post(_dto2);
                IHttpActionResult res3 = await _controller.Post(_dto2);

                var createdResult = result as CreatedAtRouteNegotiatedContentResult<Telemetry>;
                var createdResult2 = res2 as CreatedAtRouteNegotiatedContentResult<Telemetry>;



                res3.ShouldBeOfType<BadRequestErrorMessageResult>();
                BadRequestErrorMessageResult doubeAddResult = res3.ShouldBeOfType<BadRequestErrorMessageResult>();

                createdResult.ShouldNotBeNull();
                createdResult2.ShouldNotBeNull();

                doubeAddResult.Message.ShouldBe("Duplicate not allowed. A lap can track a car at most once");
            }



        }


    }
}
