using System;
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
using NHibernate.Context;

namespace NerdDinner {
    public class MvcApplication : HttpApplication
    {
        private IWindsorContainer _container;

        void Application_Start()
        {
            RegisterRoutes(RouteTable.Routes);
            RegisterComponents();
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
                            .UsingFactoryMethod(() => _CurrentSession)
                            .LifeStyle.Transient);
         
            _container.Register(
                    Component.For<IDinnerRepository>()
                            .ImplementedBy<NhDinnerRepository>().LifeStyle.Transient);

            _container.RegisterControllers(Assembly.GetExecutingAssembly());
        }

        private static ISession _CurrentSession
        {
            get { return (ISession)HttpContext.Current.Items["current.session"]; }
            set { HttpContext.Current.Items["current.session"] = value; }
        }

        private static readonly ISessionFactory SessionFactory = CreateSessionFactory();

        private static ISessionFactory CreateSessionFactory()
        {
            return new SessionFactoryBuilder(
                    new DbConfigFactory().GetGetMsSqlConfig())
                    .Build();
        }

        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            const string culturePref = "en-US";
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(culturePref);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            _CurrentSession = SessionFactory.OpenSession();
        }

        protected void Application_EndRequest(Object sender, EventArgs e)
        {
            if (_CurrentSession != null)
            {
                RollBackTransactionIfUncommited();

                CloseSessionIfOpen();

                _CurrentSession.Dispose();
            }
        }

        private void CloseSessionIfOpen()
        {
            if (_CurrentSession.IsOpen)
            {
                _CurrentSession.Close();       
            }
        }

        private void RollBackTransactionIfUncommited()
        {
            if (_CurrentSession.Transaction != null && _CurrentSession.Transaction.IsActive)
            {
                _CurrentSession.Transaction.Rollback();
            }
        }
    }
}