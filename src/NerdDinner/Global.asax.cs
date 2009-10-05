using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using MvcContrib.Castle;
using NerdDinner.Models;

namespace NerdDinner {

    public class MvcApplication : System.Web.HttpApplication
    {
        private IWindsorContainer _container;

        public void RegisterRoutes(RouteCollection routes)
        {

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "UpcomingDinners",
                "Dinners/Page/{page}",
                new { controller = "Dinners", action = "Index" }
            );

            routes.MapRoute(
                "Default",                                              // Route name
                "{controller}/{action}/{id}",                           // URL with parameters
                new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
            );
        }

        void Application_Start()
        {
            RegisterRoutes(RouteTable.Routes);
            RegisterComponents();
            InitializeDinnerRepository(FakeDinnerData.CreateTestDinners());
        }

        private void InitializeDinnerRepository(IEnumerable<Dinner> dinners)
        {
            var dinnerRepository = _container.Resolve<IDinnerRepository>();
            foreach (var dinner in dinners)
            {
                dinnerRepository.Add(dinner);    
            }
        }

        private void RegisterComponents()
        {
            _container = new WindsorContainer();
            ControllerBuilder.Current.SetControllerFactory(
                new WindsorControllerFactory(_container));
            _container.RegisterControllers(Assembly.GetExecutingAssembly());
            _container.Register(
                    Component.For<IDinnerRepository>()
                    .ImplementedBy<InMemoryDinnerRepository>());
        }

        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            const string culturePref = "en-US";
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(culturePref);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }
    }
}