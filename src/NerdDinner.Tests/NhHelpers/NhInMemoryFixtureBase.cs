using FluentNHibernate.Cfg.Db;
using NerdDinner.Infrastructure;
using NerdDinner.Models;
using NHibernate;
using NUnit.Framework;

namespace NerdDinner.Tests.NhHelpers
{
    public abstract class NhInMemoryFixtureBase
    {
        protected ISessionFactory factory;

        private IPersistenceConfigurer _configurer;

        public ISession Session { get; set; }

        [TestFixtureSetUp]
        public void OnlyOnce()
        {
            _configurer = new SqLitePersistenceConfigurerFactory()
                    .GetPersistenceConfigurer();
            factory = new SessionFactoryBuilder(_configurer)
                    .Build();
        }

        [SetUp]
        public void Init()
        {
            DbManagement.UpdateAndInitializeDb(
                _configurer, 
                FakeDinnerData.CreateTestDinners());
            Session = factory.OpenSession();
        }

        [TearDown]
        public void Clean()
        {
            if (Session != null)
            {
                Session.Dispose();
            }
        }

        [TestFixtureTearDown]
        public void CleanUp()
        {
            factory.Dispose();
        }
    }
}
