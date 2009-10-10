using FluentNHibernate.Cfg.Db;
using NHibernate.ByteCode.Castle;

namespace NerdDinner.Infrastructure
{
    public class DbConfigFactory
    {
        public IPersistenceConfigurer GetGetMsSqlConfig()
        {
            return MsSqlConfiguration.MsSql2005
                .ConnectionString(
                    c => c.Is("Data Source=localhost;Initial Catalog=AltNerdDinner;Integrated Security=True"))
                .ShowSql()
                .FormatSql()
                .ProxyFactoryFactory<ProxyFactoryFactory>();
        }

        public IPersistenceConfigurer GetSqlLiteConfig()
        {
            return SQLiteConfiguration.Standard.InMemory()
                .ShowSql()
                .FormatSql()
                .ProxyFactoryFactory<ProxyFactoryFactory>();
        }
    }
}
