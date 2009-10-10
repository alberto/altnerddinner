using FluentNHibernate.Cfg.Db;
using NerdDinner.Infrastructure;
using NUnit.Framework;

namespace NerdDinner.Tests.Integration
{
    [TestFixture]
    public class DbManagementTests
    {
        [Test, Explicit]
        public void CanCreateMsSqlDB()
        {
            IPersistenceConfigurer dbConfig = new DbConfigFactory().GetGetMsSqlConfig();
            new NhHelpers.DbManagement().CreateDB(dbConfig);
        }

        [Test, Explicit]
        public void CanCreateSqlLiteDB()
        {
            IPersistenceConfigurer dbConfig = new DbConfigFactory().GetSqlLiteConfig();
            new NhHelpers.DbManagement().CreateDB(dbConfig);
        }
    }
}