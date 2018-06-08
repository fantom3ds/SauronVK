using System;
using System.Collections.Generic;

using System.Web.Mvc;
using Ninject;
using Project_Sauron.Controllers;
using Project_Sauron.DataAccesLayer;
using Project_Sauron.Logic;

namespace Project_Sauron.NinjectConfigurator
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;
        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            kernel.Bind<IForumDao>().To<ForumDao>().InSingletonScope();
            kernel.Bind<IForumLogic>().To<ForumLogic>().InSingletonScope();
        }
    }
}
