using System;
using System.Configuration;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Facilities.FactorySupport;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using MvcContrib.Castle;
using NerdDinner.Infrastructure;
using NerdDinner.Models;
using NHibernate;

namespace NerdDinner {
    public class Global : HttpApplication
    {
        private static readonly ISessionFactory SessionFactory = CreateSessionFactory();

        private static ISessionFactory CreateSessionFactory()
        {
            string connString = ConfigurationManager.ConnectionStrings["AltNerdDinner"].ConnectionString;
            return new SessionFactoryBuilder(
                    new MsSqlPersistenceConfigurerFactory(connString)
                            .GetPersistenceConfigurer())
                    .Build();
        }

        private IWindsorContainer _container;

        private readonly NhSessionLifetimeModule _nhSessionLifetimeModule =
                new NhSessionLifetimeModule(SessionFactory);

        public override void Init()
        {
            base.Init();
            _nhSessionLifetimeModule.Init(this);
        }

        void Application_Start()
        {                       
            RegisterRoutes(RouteTable.Routes);
            RegisterComponents();
            log4net.Config.XmlConfigurator.Configure();
        }

        private void RegisterRoutes(RouteCollection routes)
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

        private void RegisterComponents()
        {
            _container = new WindsorContainer();
            _container.AddFacility<FactorySupportFacility>();

            ControllerBuilder.Current.SetControllerFactory(
                new WindsorControllerFactory(_container));

            _container.Register(
                    Component.For<ISession>()
                            .UsingFactoryMethod(() => NhSessionLifetimeModule.CurrentSession)
                            .LifeStyle.Transient);
         
            _container.Register(
                    Component.For<IDinnerRepository>()
                            .ImplementedBy<NhDinnerRepository>().LifeStyle.Transient);

            _container.RegisterControllers(Assembly.GetExecutingAssembly());
        }

        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            const string CULTURE = "en-US";
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(CULTURE);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }
    }
}