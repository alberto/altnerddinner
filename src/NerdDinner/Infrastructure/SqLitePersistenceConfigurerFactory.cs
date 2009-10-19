using FluentNHibernate.Cfg.Db;
using NHibernate.ByteCode.Castle;

namespace NerdDinner.Infrastructure
{
    public class SqLitePersistenceConfigurerFactory : IPersistenceConfigurerFactory
    {
        public IPersistenceConfigurer GetPersistenceConfigurer()
        {
            return SQLiteConfiguration.Standard.InMemory()
                .ShowSql()
                .FormatSql()
                .ProxyFactoryFactory<ProxyFactoryFactory>();
        }
    }
}
