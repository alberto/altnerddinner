using System.Collections.Generic;
using FluentNHibernate.Cfg.Db;
using NerdDinner.Infrastructure;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace NerdDinner.Tests.NhHelpers
{
    public static class DbManagement
    {
        public static void UpdateDb(IPersistenceConfigurer dbConfig)
        {
            Configuration nhConfig = new SessionFactoryBuilder(dbConfig).GetDbConfiguration();
            new SchemaExport(nhConfig).Execute(true, true, false);
        }

        public static void UpdateAndInitializeDb<T>(IPersistenceConfigurer dbConfigurer, IEnumerable<T> initialData)
        {
            var sessionFactoryBuilder = new SessionFactoryBuilder(dbConfigurer);
            Configuration nhConfig = sessionFactoryBuilder.GetDbConfiguration();
            ISessionFactory sessionFactory = nhConfig.BuildSessionFactory();
            new SchemaExport(nhConfig).Execute(true, true, false);
            InitializeRepository(
                    sessionFactory.OpenStatelessSession(),
                    initialData);
        }

        private static void InitializeRepository<T>(IStatelessSession session, IEnumerable<T> entities)
        {            
            using (session)
            using (var tx = session.BeginTransaction())
            {
                foreach (T entity in entities)
                {
                    session.Insert(entity);
                }
                tx.Commit();
            }
        }
    }
}
