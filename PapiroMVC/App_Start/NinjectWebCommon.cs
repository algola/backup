[assembly: WebActivator.PreApplicationStartMethod(typeof(PapiroMVC.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(PapiroMVC.App_Start.NinjectWebCommon), "Stop")]

namespace PapiroMVC.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Services;

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
            kernel.Bind<IArticleRepository>().To<ArticleRepository>();
            kernel.Bind<ICustomerSupplierRepository>().To<CustomerSupplierRepository>();
            kernel.Bind<ICustomerSupplierBaseRepository>().To<CustomerSupplierBaseRepository>();
            kernel.Bind<ITypeOfBaseRepository>().To<TypeOfBaseRepository>();
            kernel.Bind<ITaskExecutorRepository>().To<TaskExecutorRepository>();
            kernel.Bind<IProfileRepository>().To<ProfileRepository>();
            kernel.Bind<IProductRepository>().To<ProductRepository>();
            kernel.Bind<IDocumentRepository>().To<DocumentRepository>();
            kernel.Bind<ITypeOfTaskRepository>().To<TypeOfTaskRepository>();
            kernel.Bind<IMenuProductRepository>().To<MenuProductRepository>();
            kernel.Bind<IProductTaskNameRepository>().To<ProductTaskNameRepository>();
            kernel.Bind<IFormatsNameRepository>().To<FormatsNameRepository>();
            kernel.Bind<ICostDetailRepository>().To<CostDetailRepository>();
            kernel.Bind<IWarehouseRepository>().To<WarehouseRepository>();
            kernel.Bind<ITaskCenterRepository>().To<TaskCenterRepository>();

        }        
    }
}
