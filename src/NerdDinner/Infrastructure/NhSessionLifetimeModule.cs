using System.Web;
using NHibernate;

namespace NerdDinner.Infrastructure
{
    public class NhSessionLifetimeModule : IHttpModule
    {
        private readonly ISessionFactory _sessionFactory;

        public NhSessionLifetimeModule(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }
        public void Init(HttpApplication application)
        {
            application.BeginRequest += delegate
                                        {
                                            CurrentSession = _sessionFactory.OpenSession();
                                        };

            application.EndRequest += delegate
                                      {
                                          if (CurrentSession != null)
                                          {
                                              CurrentSession.Dispose();
                                          }
                                      };
        }

        public static ISession CurrentSession
        {
            get { return (ISession)HttpContext.Current.Items["current.session"]; }
            private set { HttpContext.Current.Items["current.session"] = value; }
        }

        public void Dispose()
        {        
        }
    }
}