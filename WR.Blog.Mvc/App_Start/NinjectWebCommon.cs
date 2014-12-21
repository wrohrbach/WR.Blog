using System;
using System.Web;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Common;
using Ninject.Web.Mvc.FilterBindingSyntax;
using WR.Blog.Business.Services;
using WR.Blog.Data.Repositories;
using WR.Blog.Mvc.Filters;
    
[assembly: WebActivator.PreApplicationStartMethod(typeof(WR.Blog.Mvc.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(WR.Blog.Mvc.NinjectWebCommon), "Stop")]

namespace WR.Blog.Mvc
{

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
            kernel.Load<BlogModule>();
        }        
    }

    public class BlogModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IBaseService>().To<BaseService>().InRequestScope();
            this.Bind<ISiteManagerService>().To<SiteManagerService>().InRequestScope();
            this.Bind<IBlogService>().To<BlogService>().InRequestScope();
            this.Bind<IBlogRepository>().To<BlogRepository>().InRequestScope();

            this.BindFilter<LoadSiteSettingsFilter>(System.Web.Mvc.FilterScope.Global, 0)
                .InRequestScope();
            this.BindFilter<NotificationFilter>(System.Web.Mvc.FilterScope.Action, 0)
                .WhenActionMethodHas<NotificationFilter>();
        }
    }

    public class SiteManagerSingletonModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<ISiteManagerService>().To<SiteManagerService>().InSingletonScope();
            this.Bind<IBlogRepository>().To<BlogRepository>().InSingletonScope();
        }
    }
}
