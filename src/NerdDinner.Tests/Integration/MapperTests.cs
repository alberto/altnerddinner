using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NerdDinner.Infrastructure;
using NerdDinner.Models;
using NHibernate.Cfg;
using NUnit.Framework;

namespace NerdDinner.Tests.Integration
{
    public class MapperTests
    {
        private string _connString = "Data Source=localhost;Initial Catalog=AltNerdDinner;Integrated Security=True";

        [Test, Explicit]
        public void CanBuildSessionFactory()
        {
            IPersistenceConfigurer dbConfig =
                    new MsSqlPersistenceConfigurerFactory(_connString)
                    .GetPersistenceConfigurer();
            var builder = new SessionFactoryBuilder(dbConfig);
            builder.Build();
        }

        [Test, Explicit]
        public void CanExportMappings()
        {
            var cfg = new Configuration();
            IPersistenceConfigurer dbConfig =
                    new MsSqlPersistenceConfigurerFactory(_connString).GetPersistenceConfigurer();

            Fluently.Configure(cfg)
                    .Database(dbConfig)
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Dinner>().ExportTo(@"."));
        } 
    }
}
