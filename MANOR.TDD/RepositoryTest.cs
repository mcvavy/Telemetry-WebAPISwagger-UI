using MANOR.Core.Entities;
using MANOR.Core.Interfaces;
using MANOR.Infrastructure.Repository;
using MANOR.Infrastructure.Utilities;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Linq;

namespace MANOR.TDD
{
    [TestFixture]
    public class RepositoryTest
    {
        private TelemetryRepository _repository;
        private MockRepository _mockfactory;
        private Mock<ITelemetry> _fakeTelemetry;
        private Mock<ITelemetryRepository> _fakeTelemetryRepository;

        [SetUp]
        public void SetUp()
        {
            _mockfactory = new MockRepository(MockBehavior.Default);
            _fakeTelemetry = _mockfactory.Create<ITelemetry>();
            _fakeTelemetryRepository = _mockfactory.Create<ITelemetryRepository>();

            _fakeTelemetry.SetupGet(x => x.Id).Returns(It.IsAny<Guid>());
            _fakeTelemetry.SetupGet(x => x.Car).Returns(It.IsAny<Car>());
            _fakeTelemetry.SetupGet(x => x.Lap).Returns(It.IsAny<Lap>());
            _fakeTelemetry.SetupGet(x => x.TimeStamp).Returns(It.IsAny<DateTime>());


            _fakeTelemetryRepository.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(new Telemetry()
            {
                Car = _fakeTelemetry.Object.Car,
                Id = _fakeTelemetry.Object.Id,
                Lap = _fakeTelemetry.Object.Lap,
                TimeStamp = _fakeTelemetry.Object.TimeStamp
            });


            _repository = new TelemetryRepository(new ReadData());

        }

        [Test]
        public void TelemetryRepositoryShouldHaveGetMethod()
        {
            _repository.Get().ShouldNotBeNull();

        }


        [Test]
        public void TelemetryRepositoryShouldHaveGetByIdReturnsOneObject()
        {
            _fakeTelemetryRepository.Object.GetById(_fakeTelemetry.Object.Id).ShouldNotBeNull();
        }


        [Test]
        public void TelemetryRepositoryShouldBeAbleToAddNewObject()
        {
            _repository.NewTelemetries.Add(new Telemetry()
            {
                Car = _fakeTelemetry.Object.Car,
                Id = _fakeTelemetry.Object.Id,
                Lap = _fakeTelemetry.Object.Lap,
                TimeStamp = _fakeTelemetry.Object.TimeStamp
            });

            _repository.NewTelemetries.Count.ShouldBe(1);
        }


        [Test]
        public void TelemetryRepositoryShouldBeAbleToUpdateAnObject()
        {
            _repository.NewTelemetries.Add(new Telemetry()
            {
                Car = _fakeTelemetry.Object.Car,
                Id = _fakeTelemetry.Object.Id,
                Lap = _fakeTelemetry.Object.Lap,
                TimeStamp = _fakeTelemetry.Object.TimeStamp
            });

            var newObject = _repository.NewTelemetries.FirstOrDefault(x => Equals(x.Id, _fakeTelemetry.Object.Id));
            if (newObject != null)
            {
                newObject.TimeStamp = DateTime.Now;
                _repository.Update(_fakeTelemetry.Object.Id, newObject);
            }

            var updatedObject = _repository.NewTelemetries.FirstOrDefault(x => Equals(x.Id, _fakeTelemetry.Object.Id));

            updatedObject?.TimeStamp.ShouldNotBe(_fakeTelemetry.Object.TimeStamp);

            updatedObject?.Car.ShouldBe(_fakeTelemetry.Object.Car);


        }


        [Test]
        public void TelemetryRepositoryShouldBeAbleToDeleteAnObject()
        {
            _repository.NewTelemetries.Clear();

            _repository.NewTelemetries.Add(new Telemetry()
            {
                Car = _fakeTelemetry.Object.Car,
                Id = _fakeTelemetry.Object.Id,
                Lap = _fakeTelemetry.Object.Lap,
                TimeStamp = _fakeTelemetry.Object.TimeStamp
            });

            _repository.NewTelemetries.Remove(
                _repository.NewTelemetries.FirstOrDefault(x => x.Id.Equals(_fakeTelemetry.Object.Id)));

            _repository.NewTelemetries.Count.ShouldBe(0);


        }
    }
}
