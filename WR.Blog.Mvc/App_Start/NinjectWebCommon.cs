[assembly: WebActivator.PreApplicationStartMethod(typeof(WR.Blog.Mvc.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(WR.Blog.Mvc.App_Start.NinjectWebCommon), "Stop")]

namespace WR.Blog.Mvc.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Web.Mvc.FilterBindingSyntax;
    using WR.Blog.Business.Services;
    using WR.Blog.Data.Repositories;
    using WR.Blog.Mvc.Filters;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            
            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IBaseService>().To<BaseService>().InRequestScope();
            kernel.Bind<ISiteManagerService>().To<SiteManagerService>().InRequestScope();
            kernel.Bind<IBlogService>().To<BlogService>().InRequestScope();
            kernel.Bind<IBlogRepository>().To<BlogRepository>().InRequestScope();

            kernel.BindFilter<LoadSiteSettingsFilter>(System.Web.Mvc.FilterScope.Global, 0).InRequestScope();
        }        
    }
}
