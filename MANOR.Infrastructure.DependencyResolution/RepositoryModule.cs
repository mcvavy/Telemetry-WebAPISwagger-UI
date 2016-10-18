using MANOR.Infrastructure.Interfaces;
using MANOR.Infrastructure.Repository;
using MANOR.Infrastructure.Utilities;
using Ninject.Modules;

namespace MANOR.Infrastructure.DependencyResolution
{
    public class RepositoryModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IReadData>().To<ReadData>();
            Bind<IUnitOfWork>().To<UnitOfWork>();

        }
    }
}
