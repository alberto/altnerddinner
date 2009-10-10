using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NerdDinner.Models;
using NHibernate;

namespace NerdDinner.Infrastructure
{
    /// <summary>
    /// NHibernate session factory
    /// </summary>
    public class SessionFactory
    {
        public static ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure()
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Dinner>())
                .Database(MsSqlConfiguration.MsSql2005.ConnectionString(c => c.Is("Data Source=local;Initial Catalog=AltNerdDinner;Integrated Security=True")))                
                .BuildSessionFactory();
        }
    }
}
