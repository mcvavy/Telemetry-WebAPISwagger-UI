using MANOR.API.Controllers;
using MANOR.API.ViewModels;
using MANOR.Infrastructure.DependencyResolution;
using MANOR.Infrastructure.Utilities;
using Moq;
using Ninject;
using NUnit.Framework;
using Shouldly;
using System.Collections.Generic;
using System.Web.Mvc;
using TestStack.FluentMVCTesting;

namespace MANOR.UnitTest
{
    [TestFixture]
    public class HomeControllerTest
    {
        private StandardKernel _kernel;
        private HomeController _controller;
        private MockRepository _factory;
        private LapViewModel _lapvm;
        private Mock<ITelemetryViewModel> _fakeTelemetryViewModel;

        [SetUp]
        public void SetUp()
        {

            _factory = new MockRepository(MockBehavior.Default);
            _fakeTelemetryViewModel = _factory.Create<ITelemetryViewModel>();
            _fakeTelemetryViewModel.SetupGet(x => x.Chone).Returns(It.IsAny<IEnumerable<SelectListItem>>());
            _fakeTelemetryViewModel.SetupGet(x => x.Chtwo).Returns(It.IsAny<IEnumerable<SelectListItem>>());
            _fakeTelemetryViewModel.SetupGet(x => x.SelectedCh1).Returns("20");
            _fakeTelemetryViewModel.SetupGet(x => x.SelectedCh2).Returns("17");


            MapperConfig.RegisterMapping();

            _kernel = new StandardKernel(new RepositoryModule());
            _controller = _kernel.Get<HomeController>();

            _lapvm = new LapViewModel()
            {
                TelemetryModel = new TelemetryViewModel()
                {
                    Chone = _fakeTelemetryViewModel.Object.Chone,
                    Chtwo = _fakeTelemetryViewModel.Object.Chtwo,
                    SelectedCh1 = _fakeTelemetryViewModel.Object.SelectedCh1,
                    SelectedCh2 = _fakeTelemetryViewModel.Object.SelectedCh2
                }
            };

        }

        [Test]
        public void ShouldRenderDefaultViewWithModel()
        {
            _controller.WithCallTo(x => x.Compare())
                .ShouldRenderDefaultView()
                .WithModel<LapViewModel>(x => x.TelemetryModel.Chone.ShouldNotBeNull());
        }

        [Test]
        public void ShouldRenderPartialViewWithModel()
        {
            _controller.WithCallTo(x => x.Comparelaps(_lapvm))
                .ShouldRenderPartialView("_lapcomparison")
                .WithModel<LapViewModel>(x => x.LapModel.Count.ShouldBe(2));
        }
    }
}
