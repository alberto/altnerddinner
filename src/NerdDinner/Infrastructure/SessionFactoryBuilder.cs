using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NerdDinner.Models;
using NHibernate;
using NHibernate.Cfg;

namespace NerdDinner.Infrastructure
{
    /// <summary>
    /// Builds the NHibernate session factory
    /// </summary>
    public class SessionFactoryBuilder
    {
        private static ISessionFactory _sessionFactory;

        private readonly IPersistenceConfigurer _dbConfiguration;

        public SessionFactoryBuilder(IPersistenceConfigurer dbConfiguration)
        {
            _dbConfiguration = dbConfiguration;
        }

        public ISessionFactory Build()
        {
            if (_sessionFactory == null)
            {
                _sessionFactory  = GetDbConfiguration().BuildSessionFactory();   
            }

            return _sessionFactory;
        }

        public Configuration GetDbConfiguration()
        {
            return Fluently.Configure()
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Dinner>())
                    .Database(_dbConfiguration)                    
                    .BuildConfiguration();
        }
    }
}