using FluentNHibernate.Cfg.Db;
using NerdDinner.Infrastructure;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace NerdDinner.Tests.NhHelpers
{
    public class DbManagement
    {
        public void CreateDB(IPersistenceConfigurer dbConfig)
        {
            Configuration nhConfig = new SessionFactoryBuilder(dbConfig).GetDbConfiguration();
            new SchemaExport(nhConfig).Execute(true, true, false);
        }
    }
}
