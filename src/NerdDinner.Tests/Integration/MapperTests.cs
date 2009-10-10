using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NerdDinner.Models;
using NHibernate;
using NHibernate.ByteCode.Castle;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace NerdDinner.Tests.Integration
{
    [TestFixture]
    public class MapperTests
    {
        [Test, Explicit]
        public void CanCreateMsSqlDB()
        {
            IPersistenceConfigurer dbConfig = GetGetMsSqlConfig();
            CreateDB(dbConfig);
        }

        [Test, Explicit]
        public void CanCreateSqlLiteDB()
        {
            IPersistenceConfigurer dbConfig = GetSqlLiteConfig();
            CreateDB(dbConfig);
        }

        [Test]
        public void CanBuildSessionFactory()
        {
            IPersistenceConfigurer dbConfig = GetGetMsSqlConfig();
            var configuration = BuildNhConfiguration(dbConfig);
            configuration.BuildSessionFactory();
        }

        [Test]
        public void CanExportMappings()
        {
            var cfg = new Configuration();
            IPersistenceConfigurer dbConfig = GetGetMsSqlConfig();

            ISessionFactory configuration = Fluently.Configure(cfg)
                    .Database(dbConfig)
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Dinner>().ExportTo(@"."))
                    .BuildSessionFactory();
        } 

        private IPersistenceConfigurer GetGetMsSqlConfig()
        {
            return MsSqlConfiguration.MsSql2005
                .ConnectionString(
                    c => c.Is("Data Source=localhost;Initial Catalog=AltNerdDinner;Integrated Security=True"))
                .ShowSql()
                .FormatSql()
                .ProxyFactoryFactory<ProxyFactoryFactory>();
        }

        private IPersistenceConfigurer GetSqlLiteConfig()
        {
            return SQLiteConfiguration.Standard.InMemory()
                .ShowSql()
                .FormatSql()
                .ProxyFactoryFactory<ProxyFactoryFactory>();
        }

        private void CreateDB(IPersistenceConfigurer dbConfig)
        {
            Configuration nhConfig = BuildNhConfiguration(dbConfig);
            new SchemaExport(nhConfig).Execute(true, true, false);
        }

        private Configuration BuildNhConfiguration(IPersistenceConfigurer dbConfig)
        {
            return Fluently.Configure()
                    .Database(dbConfig)
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Dinner>())
                    .BuildConfiguration();
        }
    }
}