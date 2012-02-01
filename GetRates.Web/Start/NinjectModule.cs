using GetRates.Services.Providers;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Mvc;
using GetRates.Services;
using System.Collections.Generic;

[assembly: WebActivator.PreApplicationStartMethod(typeof(GetRates.Web.Start.NinjectModule), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(GetRates.Web.Start.NinjectModule), "Stop")]

namespace GetRates.Web.Start
{
    public static class NinjectModule 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestModule));
            DynamicModuleUtility.RegisterModule(typeof(HttpApplicationInitializationModule));
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
            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IExchangeRateProvider>().To<MoneyToIndiaExchangeRateProvider>();
            kernel.Bind<IExchangeRateProvider>().To<RemitToIndiaExchangeRateProvider>();
            kernel.Bind<IExchangeRateProvider>().To<XoomExchangeRateProvider>();
            kernel.Bind<IExchangeRateProvider>().To<AxisExchangeRateProvider>();
            kernel.Bind<IExchangeRateProvider>().To<BoBExchangeRateProvider>();
            kernel.Bind<IExchangeRateProvider>().To<BoiExchangeRateProvider>();
            kernel.Bind<ICalculationService>().To<CalculationService>();
            kernel.Bind<IExchangeRateProvider>().To<GoogleExchangeRateProvider>().When(request =>
                                                                             request.Target.Name.StartsWith(
                                                                                 "reference"));
        }        
    }
}
