using FluentNHibernate.Cfg.Db;
using NerdDinner.Infrastructure;
using NerdDinner.Models;
using NerdDinner.Tests.NhHelpers;
using NUnit.Framework;

namespace NerdDinner.Tests.Integration
{
    [TestFixture]
    public class DbManagementTests
    {
        string connString = "Data Source=localhost;Initial Catalog=AltNerdDinner;Integrated Security=True";
        [Test, Explicit]
        public void CanCreateMsSqlDB()
        {
            IPersistenceConfigurer dbConfig =
                    new MsSqlPersistenceConfigurerFactory(connString)
                            .GetPersistenceConfigurer();
            DbManagement.UpdateDb(dbConfig);
        }

        [Test, Explicit]
        public void CanCreateSqlLiteDB()
        {
            IPersistenceConfigurer dbConfig =
                    new SqLitePersistenceConfigurerFactory().GetPersistenceConfigurer();
            DbManagement.UpdateDb(dbConfig);
        }

        [Test, Explicit]
        public void InitializeSqliteDb()
        {
            IPersistenceConfigurer dbConfig =
                    new SqLitePersistenceConfigurerFactory().GetPersistenceConfigurer();
            DbManagement.UpdateAndInitializeDb(dbConfig, FakeDinnerData.CreateTestDinners());
        }


        [Test, Explicit]
        public void InitializeMsSqlDb()
        {
            IPersistenceConfigurer dbConfig =
        new MsSqlPersistenceConfigurerFactory(connString)
                .GetPersistenceConfigurer();
            DbManagement.UpdateAndInitializeDb(dbConfig, FakeDinnerData.CreateTestDinners());
        }
    }
}