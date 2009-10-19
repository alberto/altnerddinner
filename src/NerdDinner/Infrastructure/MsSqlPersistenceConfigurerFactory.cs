using FluentNHibernate.Cfg.Db;
using NHibernate.ByteCode.Castle;

namespace NerdDinner.Infrastructure
{
    public class MsSqlPersistenceConfigurerFactory : IPersistenceConfigurerFactory
    {
        private string _connectionString;

        public MsSqlPersistenceConfigurerFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IPersistenceConfigurer GetPersistenceConfigurer()
        {
            return MsSqlConfiguration.MsSql2005
                    .ConnectionString(c => c.Is(_connectionString))
                    .ShowSql()
                    .FormatSql()
                    .ProxyFactoryFactory<ProxyFactoryFactory>();
        }
    }
}